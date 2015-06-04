using UnityEngine;
using System.Collections;

public class GameSession : MonoBehaviour {

    protected string userID { get; set; }
    protected string gameInstanceID { get; set; }
    protected string gameType { get; set; }
    public System.DateTime startTime { protected get; set; }

	// Use this for initialization
	void Start () {

        // This is loaded from the players preference string "PlayerNumber" Set by LoadSCene script
		userID = PlayerPrefs.GetInt("PlayerNumber").ToString();
		Debug.Log("Player loaded is: " + userID);


        gameType = "default";
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
        data.startDate = startTime;
        data.gameType = gameType;
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
