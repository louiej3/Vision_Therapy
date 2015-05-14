using SQLite4Unity3d;
using MySql.Data;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The Database acts as a wrapper around the SQLite4Unity3d library in order
/// to make it more straightforward to use.
/// </summary>
public class Database : MonoBehaviour {

    const string MYSQLCONNECTIONSTRING = "server=vergil.u.washington.edu;uid=basic;pwd=ChildVision;database=ChildVision;port=50290;";

    const string DatabaseName = "testDB2.db";

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
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Final PATH: " + dbPath);
	}

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        _connection.CreateTable<TargetData>();
        _connection.CreateTable<TargetManData>();
        _connection.CreateTable<MovingTargetsManData>();
        _connection.CreateTable<GameInstance>();
        _connection.CreateTable<User>();
        _connection.CreateTable<Sync>();
        _connection.Insert(new Sync() { syncID = 0, syncDate = System.DateTime.MinValue });
    }
	
	// Update is called once per frame
    
    void Update () {

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
        Debug.Log(syncDate.ToString());
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
        ArrayList mechanics = new ArrayList(), managers = new ArrayList()
            , targetList = new ArrayList();
        foreach (GameInstance g in games)
        {
            IEnumerable<MovingTargetsManData> gameMan = _connection.Table<MovingTargetsManData>().Where(x => x.gameInstanceID == g.gameInstanceID);
            foreach (MovingTargetsManData m in gameMan)
            {
                mechanics.Add(m);
            }
        }

        foreach (MovingTargetsManData m in mechanics)
        {
            IEnumerable<TargetManData> targetMan = _connection.Table<TargetManData>().Where(x => x.gameManID == m.gameManID);
            foreach (TargetManData t in targetMan)
            {
                managers.Add(t);
            }
        }

        foreach (TargetManData t in managers)
        {
            IEnumerable<TargetData> targets = _connection.Table<TargetData>().Where(x => x.managerID == t.targetManID);
            foreach (TargetData target in targets)
            {
                targetList.Add(target);
            }
        }

        // do inserts for all of this data

        foreach (GameInstance i in games)
        {
            addInstances.CommandText = i.generateInsert();
            int editNum = addInstances.ExecuteNonQuery();
            Debug.Log(string.Format("added {0} rows", editNum));
        }

        foreach (MovingTargetsManData m in mechanics)
        {
            addInstances.CommandText = m.generateInsert();
            int editNum = addInstances.ExecuteNonQuery();
            Debug.Log(string.Format("added {0} rows", editNum));
        }

        foreach (TargetManData t in managers)
        {
            addInstances.CommandText = t.generateInsert();
            int editNum = addInstances.ExecuteNonQuery();
            Debug.Log(string.Format("added {0} rows", editNum));
        }

        foreach (TargetData t in targetList)
        {
            addInstances.CommandText = t.generateInsert();
            int editNum = addInstances.ExecuteNonQuery();
            Debug.Log(string.Format("added {0} rows", editNum));
        }


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

}
