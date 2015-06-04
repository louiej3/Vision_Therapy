using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DifficultySettings : MonoBehaviour {
	
	private UnityEngine.UI.Slider my_slider;		// For the sliders being manipulated
	public int GameNum = 3;							// For the number of games for EYE
	public int SlideNum = 0;						// Slider Number for each games' difficulty settings
	
	static int TotalPlayers = 3;
	static int TotalGames = 3;
	static int MaxSliders = 12;						// Max number of difficulty sliders for the game(s) including master
	static int MaxMasterLvl = 11;					// Max number of levels the Master slider has plus 1 more (ex. 1-10 = 11)

	static bool PTOToSave = false;					// Need to save 4D array bools
	static bool EDBToSave = false;					//
	static bool RingsToSave = false;				//
	static bool LoadValues = true;					// Determines if the 4D array was loaded once already (no duplicates)

	static int[, , ,] AllDiff4D;					// 4D array for Poke the owls (Moving Targets)
	
	// Use this for initialization
	void Start ()
	{
		// Creates the slider for EACH slider the script is attached to (master or individual difficulty)
		my_slider = gameObject.GetComponent<UnityEngine.UI.Slider>();

		// Delete PlayersPrefs
		//PlayerPrefs.DeleteAll();

		if (LoadValues == true)
		{
			// Instantiation of the 2D arrays
			AllDiff4D = new int[TotalPlayers,TotalGames,MaxSliders,MaxMasterLvl];

			// Load the players setting preference string
			string p0diff = PlayerPrefs.GetString("P0DIFF");
			string p1diff = PlayerPrefs.GetString("P1DIFF");
			string p2diff = PlayerPrefs.GetString("P2DIFF");

			// Analyze if the string, if not null, load the values. If it is null, load default values.
			LoadDiffFiles (AllDiff4D, p0diff, 0); // 4D array, player pref string, player number
			LoadDiffFiles (AllDiff4D, p1diff, 1);
			LoadDiffFiles (AllDiff4D, p2diff, 2);

			LoadValues = false;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void MasterUPDATE( ) // For use with the Save button on the game's edit screen (Saves changes to 2D array)
	{
		int PlayerNum = PlayerInfo.playerNumber;		// Grabs the player number from the PlayerInfo script

		// Save each sliders current difficulty level back to the 2D array
		if (GameNum == 0) // Poke the Owls / Moving Targets
		{
			AllDiff4D [PlayerNum, GameNum, SlideNum, (int)GameObject.Find ("PTOMasterSlideredit").GetComponent<Slider> ().value] = (int)my_slider.value;
			PTOToSave = true;
		}
		if (GameNum == 1) // Executive Delivery Boy / Crowd game
		{
			AllDiff4D [PlayerNum, GameNum, SlideNum, (int)GameObject.Find ("EDBMasterSlideredit").GetComponent<Slider> ().value] = (int)my_slider.value;
			EDBToSave = true;
		}
		if (GameNum == 2) // Converging Rings
		{
			AllDiff4D [PlayerNum, GameNum, SlideNum, (int)GameObject.Find ("RingsMasterSlideredit").GetComponent<Slider> ().value] = (int)my_slider.value;
			RingsToSave = true;
		}
	}

	public void CHANGE( )// Changes the individual settings on master slider change
	{
		// SlideNum is the particular slider number (for each slider) and the MasterSlider value is the particular difficulty
		// These two comprise the location for the 2D array of difficulty values

		int PlayerNum = PlayerInfo.playerNumber;			// Grabs the player number from the PlayerInfo script

		if (GameNum == 0) // Moving Targets
		{
			int difficulty = AllDiff4D [PlayerNum, GameNum, SlideNum, (int)GameObject.Find ("PTOMasterSlider").GetComponent<Slider>().value];
			if (my_slider) { my_slider.value = difficulty; } // For each individual difficulty slider
		}
		if (GameNum == 1) // Tracker
		{
			int difficulty = AllDiff4D [PlayerNum, GameNum, SlideNum, (int)GameObject.Find ("EDBMasterSlider").GetComponent<Slider>().value];
			if (my_slider) { my_slider.value = difficulty; } // For each individual difficulty slider
		}
		if (GameNum == 2) // Converging Rings
		{
			int difficulty = AllDiff4D [PlayerNum, GameNum, SlideNum, (int)GameObject.Find ("RingsMasterSlider").GetComponent<Slider>().value];
			if (my_slider) { my_slider.value = difficulty; } // For each individual difficulty slider
		}
	}

	public void CHANGEedit( ) // Changes the individual settings on master slider change
	{
		// SlideNum is the particular slider number (for each slider) and the MasterSlider value is the particular difficulty
		// These two comprise the location for the 2D array of difficulty values

		int PlayerNum = PlayerInfo.playerNumber;		// Grabs the player number from the PlayerInfo script

		if (GameNum == 0) // Poke the Owls / Moving Targets
		{
			int difficulty = AllDiff4D [PlayerNum, GameNum, SlideNum, (int)GameObject.Find ("PTOMasterSlideredit").GetComponent<Slider>().value];
			if (my_slider) { my_slider.value = difficulty; }
		}
		if (GameNum == 1) // Executive Delivery Boy / Crowd game
		{
			int difficulty = AllDiff4D [PlayerNum, GameNum, SlideNum, (int)GameObject.Find ("EDBMasterSlideredit").GetComponent<Slider>().value];
			if (my_slider) { my_slider.value = difficulty; }
		}
		if (GameNum == 2) // Converging Rings
		{
			int difficulty = AllDiff4D [PlayerNum, GameNum, SlideNum, (int)GameObject.Find ("RingsMasterSlideredit").GetComponent<Slider>().value];
			if (my_slider) { my_slider.value = difficulty; }
		}
	}

	public void Default()	// Loads all of the default difficulty settings for the particular game (Restore Default Button)
	{
		if(GameNum == 0)
		{ LoadDefault(AllDiff4D, PlayerInfo.playerNumber, 0); }
		if(GameNum == 1)
		{ LoadDefault(AllDiff4D, PlayerInfo.playerNumber, 1); }
		if(GameNum == 2)
		{ LoadDefault(AllDiff4D, PlayerInfo.playerNumber, 2); }
	}

	public void LoadDefault(int [, , ,] my4DArray, int pNumber, int game) 	// pNumber is player Number & game is TotalGames the specific game
	{
		// X = Slider Number (Master = 0) and Y = Difficulty level of that slider

		if( game == TotalGames) // Default load all games for player pNumber
		{
			for (int w = 0; w < TotalGames; w++)
			{
				for (int x = 0; x < MaxSliders; x++)
				{
					for (int y = 0; y < MaxMasterLvl; y++) 		// Misc values to begin with in each 4D array
					{ my4DArray[pNumber,w,x,y] = y*2; }			// CHANGE LOAD DEFAULT VALUES HERE
				}
			}

		}
		else  // Load particular game's default values for player pNumber 
		{
			for (int x = 0; x < MaxSliders; x++)
			{
				for (int y = 0; y < MaxMasterLvl; y++) 		// Misc values to begin with in each 4D array
				{ my4DArray[pNumber,game,x,y] = y*2; }		// CHANGE LOAD DEFAULT VALUES HERE
			}
		}

	}

	public void SavePrefs() // Records the adjusted difficulties back to a players preference "file" 
	{
		string player = "P";
		int PlayerNum = PlayerInfo.playerNumber;		// Grabs the player number from the PlayerInfo script
		player += PlayerNum.ToString();					// For player number
		player += "DIFF";

		// Debug.Log("Player String to save is: " + player);

		// Send 4D array to get saved
		string saveString = SaveDiffFiles( AllDiff4D );	// Convert 4D array to comma separated string (for a specific player)
		PlayerPrefs.SetString(player, saveString);		// Saves the string to the players preference

		if (GameNum == 0 && PTOToSave == true)		// Save PTOdiff for Poke the Owls / Moving Targets
		{ PTOToSave = false; }						// Resets the need to save bool

		if (GameNum == 1 && EDBToSave == true) 		// Save EDBdiff for Executive Delivery Boy / Crowd game
		{ EDBToSave = false; }						// Resets the need to save bool

		if (GameNum == 2 && RingsToSave == true)	// Save Ringsdiff for Converging Rings
		{ RingsToSave = false; }					// Resets the need to save bool
	}

	public void LoadDiffFiles(int [, , ,] my4DArray, string saveprefString, int myPlayer ) // Loads the 4D array specified with the supplied string array 
	{
		char[] delimiterChars = { ',' };								// Delimiter characters to look for and removes them
		string[] wordsDiff = saveprefString.Split(delimiterChars);		// Splits the playerspref string into the individial bits (One big array)

		// Checks the elements to verify it is the same size as the saved 4D array
		if (wordsDiff [0] == TotalGames.ToString() && wordsDiff [1] == MaxSliders.ToString () && wordsDiff [2] == MaxMasterLvl.ToString ())
		{

			int z = 3;													// Starts after the first 4 elements check
			int stringLength = wordsDiff.Length;						// Length of the given string of user saved data
			for (int w = 0; w < TotalGames; w++)						// For each game
			{
				for (int x = 0; x < MaxSliders; x++)					// For each Slider
				{
					for (int y = 0; y < MaxMasterLvl; y++)				// For each difficulty of the slider
					{
						if (z < stringLength)
						{
							// Converts each string element to an int in the 4D array
							my4DArray [myPlayer,w,x,y] = int.Parse (wordsDiff [z]); ///
							z++;
						}
					}
				}
			}
		}
		else 	// Array changed size and all games must be reset with defaults
		{
			// Loads default values for the 4D array, player number, and all games
			if(myPlayer == 0)
			{ LoadDefault(AllDiff4D, 0, TotalGames); }
			if(myPlayer == 1)
			{ LoadDefault(AllDiff4D, 1, TotalGames); }
			if(myPlayer == 2)
			{ LoadDefault(AllDiff4D, 2, TotalGames); } 
		}							
	}

	public string SaveDiffFiles( int [, , ,] my4DArray ) // Saves the 4D array specified with the supplied string array 
	{
		// Player number is already known by here, and this will save ALL games' settings for the current player
		// Save array to a string but first inserts the 4D array's size (total games, width, length)
		string str = "";								// To start the string
		int PlayerNum = PlayerInfo.playerNumber;		// Grabs the player number from the PlayerInfo script
		int num;										// To hold the number from the 4D array to convert to a string

		str += TotalGames.ToString();					// And total number of games held
		str += ',';
		str += MaxSliders.ToString();					// And the slider lengths
		str += ',';
		str += MaxMasterLvl.ToString();
		str += ',';

		// W = Game number, X = Slider Number (Master = 0), and Y = Difficulty level of that slider
		for (int w = 0; w < TotalGames; w++)
		{
			for (int x = 0; x < MaxSliders; x++)
			{
				for (int y = 0; y < MaxMasterLvl; y++)
				{
					// Misc values to begin with in each 2D array
					num = my4DArray [PlayerNum,w,x,y];
					str += num.ToString();
					str += ',';
				}
			}
		}

		return str;

	}
	
}
