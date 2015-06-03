using UnityEngine;
using System.Collections;

public class PlayerInfo : MonoBehaviour {
	
	public int player = 0;					// Number of each user
	public static int playerNumber;			// Number for which user is playing/training
	static string Player0Name;				// Saved user name for player 1
	static string Player1Name;				// Saved user name for player 2
	static string Player2Name;				// Saved user name for player 3
	static bool Loaded = true;				// Determines if the 4D array was loaded once already (no duplicates)

	// Use this for initialization
	void Start ()
	{
		if (Loaded == true)
		{
			// Sets the players name once, then load it everytime thereafter
			if (PlayerPrefs.GetString("USER0") == null)
			{ PlayerPrefs.SetString("USER0", "Player0"); }
			if (PlayerPrefs.GetString("USER1") == null)
			{ PlayerPrefs.SetString("USER1", "Player1"); }
			if (PlayerPrefs.GetString("USER2") == null)
			{ PlayerPrefs.SetString("USER2", "Player2"); }

			// Loads the players name to the static strings
			if (PlayerPrefs.GetString("USER0") != null)
			{ Player0Name = PlayerPrefs.GetString("USER0"); }
			if (PlayerPrefs.GetString("USER1") != null)
			{ Player1Name = PlayerPrefs.GetString("USER1"); }
			if (PlayerPrefs.GetString("USER2") != null)
			{ Player2Name = PlayerPrefs.GetString("USER2"); }

			Loaded = false;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	// 
	public void LoadUsername ()
	{ 
		if (player == 0)
		{ Player0Name = PlayerPrefs.GetString ("USER0"); }
		if (player == 1)
		{ Player1Name = PlayerPrefs.GetString ("USER1"); }
		if (player == 2)
		{ Player2Name = PlayerPrefs.GetString ("USER2"); }
	}

	// Saves the username with the given string
	public void SaveUsername ( string myname )
	{ PlayerPrefs.SetString ("USER" + playerNumber.ToString(), myname); }

	// Update is called once per frame
	public void LoadPlayerNumber()
	{ playerNumber = player; }

	public int GetPlayerNumber()
	{ return playerNumber; }
	
}
