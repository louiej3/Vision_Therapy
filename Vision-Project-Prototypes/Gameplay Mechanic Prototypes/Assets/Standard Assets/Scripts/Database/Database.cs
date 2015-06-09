using SQLite4Unity3d;
using MySql.Data;
using UnityEngine;
#if !UNITY_EDITOR
using System.IO;
#endif
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The Database acts as a wrapper around the SQLite4Unity3d library in order
/// to make it more straightforward to use.
/// </summary>
public class Database : MonoBehaviour {

    const string MYSQLCONNECTIONSTRING = "server=128.95.242.208;uid=basic;pwd=ChildVision;database=ChildVision;port=50290;";

    const string DatabaseName = "testDB.db";

    private StopWatch timer;
    private const float SYNC_TIME = 300f; // check for sync data every 30 seconds

    private SQLiteConnection _connection;
    private SQLiteCommand _command;

	void Awake () 
    {

        Debug.Log("Persistent: " + Application.persistentDataPath);
        Debug.Log("Normal: " + Application.dataPath);

#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID 
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
			var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
			// then save to Application.persistentDataPath
			File.Copy(loadDb, filepath);
#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
		Debug.Log("Final PATH: " + dbPath);
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
	}

    void Start()
    {
        // Make sure the database gameobject is always up
        DontDestroyOnLoad(this.gameObject);

        // Create all the talbes (if they don't exist)
        _connection.CreateTable<ObjectData>();
        _connection.CreateTable<ManagerData>();
        _connection.CreateTable<MechanicData>();
        _connection.CreateTable<GameInstance>();
        _connection.CreateTable<User>();
        _connection.CreateTable<Sync>();
        _connection.Insert(new Sync() { syncID = 0, syncDate = System.DateTime.MinValue });

        timer = new StopWatch();
        timer.start();
    }
	
	// Update is called once per frame
    
    void Update () {

        // occasionally  check to sync data
        
        if (timer.lap() > SYNC_TIME)
        {
            syncData();
            timer.start();
        }


    }

    /// <summary>
    /// Inserts a row into a table
    /// </summary>
    /// <param name="sql">The sql code to be run by the database</param>
    /// <returns>returns true if more than 0 rows were edited</returns>
    public bool insert(object obj)
    {
        return _connection.Insert(obj) > 0;
    }
    /// <summary>
    /// Inserts all rows into a table
    /// </summary>
    /// <param name="sql">The sql code to be run by the database</param>
    /// <returns>returns true if more than 0 rows were edited</returns>
    public bool insertAll(System.Collections.IEnumerable objs )
    {
        return _connection.InsertAll(objs) > 0;
    }
    /// <summary>
    /// Syncs data to the main server, which is hardcoded in
    /// </summary>
    /// <returns>returns true on successfull sync</returns>
    public bool syncData() {
        // check for internet connection
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Sync failed: no internet connection");
            return false;
        }
        Debug.Log("Network reachable");
        // try to make a connection to MySQL server
        MySql.Data.MySqlClient.MySqlConnection mySQLConnection;
        MySql.Data.MySqlClient.MySqlCommand addUsers;
        MySql.Data.MySqlClient.MySqlCommand addInstances;
        try
        {
            mySQLConnection = new MySql.Data.MySqlClient.MySqlConnection(MYSQLCONNECTIONSTRING);
            mySQLConnection.Open();
            addInstances = mySQLConnection.CreateCommand();
            addUsers = mySQLConnection.CreateCommand();
        }
        catch (MySql.Data.MySqlClient.MySqlException ex)
        {
            Debug.Log( "SQL connection failed");
            Debug.Log( ex.Message );
            return false;
        }

        Debug.Log("Connection Established");
        addUsers.CommandText = "";
        addInstances.CommandText = "";
        
        // check for latest sync
        System.DateTime syncDate = lastSync();

        // find any users added after this date.
        IEnumerable<User> users = _connection.Table<User>().Where( x => x.creationDate > syncDate);
        foreach( User u in users ) 
        {
            addUsers.CommandText = u.generateInsert();
            // add users 
            int result = addUsers.ExecuteNonQuery();
            Debug.Log(string.Format("added {0} users", result));

        }

        // find all game instances that took place after that
        // query to get these instances
        IEnumerable<GameInstance> games = _connection.Table<GameInstance>().Where( x => x.completionDate > syncDate);
        
        // if there are no new games we are done
        if (games == null)
        {
            return true;
        }
        
        ArrayList mechanics = new ArrayList(), managers = new ArrayList()
            , targetList = new ArrayList();
        foreach (GameInstance g in games)
        {
            Debug.Log(g.generateInsert());
            IEnumerable<MechanicData> gameMan = _connection.Table<MechanicData>().Where(x => x.gameInstanceID == g.gameInstanceID);
            foreach (MechanicData m in gameMan)
            {
                mechanics.Add(m);
            }
        }

        foreach (MechanicData m in mechanics)
        {
            IEnumerable<ManagerData> targetMan = _connection.Table<ManagerData>().Where(x => x.mechanicID == m.mechanicID);
            foreach (ManagerData t in targetMan)
            {
                managers.Add(t);
            }
        }

        foreach (ManagerData t in managers)
        {
            IEnumerable<ObjectData> targets = _connection.Table<ObjectData>().Where(x => x.managerID == t.managerID);
            foreach (ObjectData target in targets)
            {
                targetList.Add(target);
            }
        }

        // do inserts for all of this data

        int rowsAffected = 0;
        foreach (GameInstance i in games)
        {
            addInstances.CommandText = i.generateInsert();
            int editNum = addInstances.ExecuteNonQuery();
            rowsAffected += editNum;
        }
        Debug.Log(string.Format("{0} Instances added", rowsAffected));

        rowsAffected = 0;
        foreach (MechanicData m in mechanics)
        {
            addInstances.CommandText = m.generateInsert();
            int editNum = addInstances.ExecuteNonQuery();
            rowsAffected += editNum;
        }
        Debug.Log(string.Format("{0} mechanics added", rowsAffected));

        rowsAffected = 0;
        foreach (ManagerData t in managers)
        {
            addInstances.CommandText = t.generateInsert();
            int editNum = addInstances.ExecuteNonQuery();
            rowsAffected += editNum;
        }
        Debug.Log(string.Format("{0} managers added", rowsAffected));

        rowsAffected = 0;
        foreach (ObjectData t in targetList)
        {
            addInstances.CommandText = t.generateInsert();
            int editNum = addInstances.ExecuteNonQuery();
            rowsAffected += editNum;
        }
        Debug.Log(string.Format("{0} Targets added", rowsAffected));


        // Add a new sync Object
        Sync syncData = new Sync();
        syncData.syncDate = System.DateTime.Now;
        _connection.Insert(syncData);

        return true;

    }

    private System.DateTime lastSync() 
    {   
        System.DateTime last;
        last = _connection.Table<Sync>().OrderByDescending( x => x.syncDate).First().syncDate;
        return last;
    }

    /// <summary>
    /// A function that can be called in case you need todrop all tables, all information will be lost.
    /// </summary>
    private void DropallTables()
    {
        _connection.DropTable<ObjectData>();
        _connection.DropTable<ManagerData>();
        _connection.DropTable<MechanicData>();
        _connection.DropTable<GameInstance>();
        _connection.DropTable<User>();
        _connection.DropTable<Sync>();
    }
}
