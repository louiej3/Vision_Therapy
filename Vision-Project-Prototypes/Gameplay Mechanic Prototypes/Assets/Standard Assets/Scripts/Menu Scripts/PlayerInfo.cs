using UnityEngine;
using System.Collections;

public class PlayerInfo : MonoBehaviour {
	
	public int player = 0;					// Number for each user button
	public static int playerNumber;			// Number for which current user is playing/training
	static string Player0Name;				// Saved user name for player 1
	static string Player1Name;				// Saved user name for player 2
	static string Player2Name;				// Saved user name for player 3
	static bool Loaded = true;				// Determines if the 4D array was loaded once already (no duplicates)

	// Use this for initialization
	void Start ()
	{
		if (Loaded == true)
		{
			// Sets the players name once if non existant, then loads it everytime thereafter (not finished)
			if (PlayerPrefs.GetString("USER0NAME") == null)
			{ PlayerPrefs.SetString("USER0NAME", "Player0"); }
			if (PlayerPrefs.GetString("USER1NAME") == null)
			{ PlayerPrefs.SetString("USER1NAME", "Player1"); }
			if (PlayerPrefs.GetString("USER2NAME") == null)
			{ PlayerPrefs.SetString("USER2NAME", "Player2"); }


			for (int i = 0; i < 3; i++) // For number of players
			{
				for (int j = 0; j < 3; j++) // For number of games
				{
					string inum = i.ToString();
					string jnum = j.ToString();
					if (PlayerPrefs.GetInt("P" + inum + "G" + jnum + "LVL") == null)
					{ PlayerPrefs.SetInt("P" + inum + "G" + jnum + "LVL", 1); }
				}
			}
			Loaded = false;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	// Saves the player # to static playerNumber
	public void LoadPlayerNumber ()
	{ playerNumber = player; }

	// Saves the username with the given string
	public void SaveUsername ( string myname )
	{ PlayerPrefs.SetString ("USER" + playerNumber.ToString() + "NAME", myname); }

}
