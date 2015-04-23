using Mono.Data.SqliteClient;
using System.Data;
using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
public class Database : MonoBehaviour {

    const string dbName = "testDB";
    private SqliteConnection _connection;
    private IDbCommand _command;
    private IDataReader _reader;

    public const string CREATE_TABLE = "CREATE TABLE IF NOT EXISTS {0}({1});";
    public const string OVERWRITE_TABLE = "CREATE TABLE {0}({1});";
    public const string INSERT_ROW = "INSERT OR FAIL INTO {0}({1}) VALUES({2});";
    public const string SELECT_ALL = "SELECT * FROM {0}";
	// Use this for initialization
	void Awake () 
    {
#if UNITY_EDITOR
        //var dbPath = string.Format("URI=file:{1}/Assets/StreamingAssets/{0}", DatabaseName, Application.persistentDataPath);
        var dbPath = string.Format("URI=file:{0}/{1}", Application.streamingAssetsPath, dbName);
        Debug.Log(dbPath);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, dbName);
        Debug.Log("android: " + filepath);
        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->
            
#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + dbName);  // this is the path to your StreamingAssets in android
            StopWatch timer = new StopWatch();
            timer.start();
            while (!loadDb.isDone) {
                if (timer.lap() > 5) {
                    Debug.Log("failed to get app data");
                    break;
                }   
            }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            Debug.Log(Application.dataPath);
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

        var dbPath = "URI=file:" + filepath;
#endif
        Debug.Log("Final PATH: " + dbPath);
        _connection = (IDbConnection)new SqliteConnection(dbPath);
        _connection.Open();
	}

    void Start()
    {
        string addressProp = "addrID int PRIMARY KEY,zip int, street string, state string, city string";
        createTable("Address", addressProp);
    }
	
	// Update is called once per frame
    
    void Update () {

    }

    /// <summary>
    /// Creates a table in the database if it does not exist
    /// </summary>
    /// <param name="tableName">Name of the table to be created</param>
    /// <param name="tableAttr">All the properties of the table to be created</param>
    /// <param name="overwrite">Do you want to overwrite any existing table? (default = false)</param>
    public void createTable(string tableName, string tableAttr, bool overwrite = false)
    {
        _command = _connection.CreateCommand();
        if (overwrite)
        {
            _command.CommandText = string.Format(OVERWRITE_TABLE, tableName, tableAttr);
        }
        else
        {
            _command.CommandText = string.Format(CREATE_TABLE, tableName, tableAttr);
        }
        _command.ExecuteNonQuery();

    }

    /// <summary>
    /// Inserts a row into a table, assumes the sql has been passed in by the user correctly
    /// </summary>
    /// <param name="sql">The sql code to be run by the database</param>
    /// <returns>returns true if more than 0 rows were edited</returns>
    public bool insertFail(string sql)
    {
        _command = _connection.CreateCommand();
        _command.CommandText = sql;
        int result = _command.ExecuteNonQuery();
        return result > 0;
    }

    public IDataReader select(string sql)
    {
        _command = _connection.CreateCommand();
        _command.CommandText = sql;
        _reader = _command.ExecuteReader();
        return _reader;
    }
}
