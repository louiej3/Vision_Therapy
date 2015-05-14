using UnityEngine;
using System.Collections;

public class GameSession : MonoBehaviour {

    protected string userID { get; set; }
    protected string gameInstanceID { get; set; }

	// Use this for initialization
	void Start () {

        // This should be set by by something else
        userID = "1";

        DontDestroyOnLoad(this.gameObject);
        gameInstanceID = System.Guid.NewGuid().ToString();

	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public GameInstance packData()
    {
        GameInstance data = new GameInstance();
        data.gameInstanceID = gameInstanceID;
        data.userID = userID;
        data.completionDate = System.DateTime.Now;
        return data;
    }

    public string getID()
    {
        return gameInstanceID;
    }

    public string getUserID()
    {
        return userID;
    }
}
