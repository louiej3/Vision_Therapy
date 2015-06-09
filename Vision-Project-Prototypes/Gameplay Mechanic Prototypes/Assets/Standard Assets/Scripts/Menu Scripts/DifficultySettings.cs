using UnityEngine;
using System.Collections;
using UnityEngine.UI;								// Needed for UI elements

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

	static int[, , ,] AllDiff4D;					// 4D array for the games
	static int[, , ,] AllDiff4DTemp;				// 4D array for the games
	
	// Use this for initialization
	void Start ()
	{
		// Creates the slider for EACH slider the script is attached to (master or individual difficulty)
		my_slider = gameObject.GetComponent<UnityEngine.UI.Slider>();

		// Delete PlayersPrefs, uncomment below to wipe all player preferences
		//PlayerPrefs.DeleteAll();

		if (LoadValues == true)
		{
			// Instantiation of the 4D arrays (regular holds saved values, temp holds the temporary)
			AllDiff4D = new int[TotalPlayers,TotalGames,MaxSliders,MaxMasterLvl];
			AllDiff4DTemp = new int[TotalPlayers,TotalGames,MaxSliders,MaxMasterLvl];

			// Load the players setting preference string (Master difficulty settings)
			string P0DIFF = PlayerPrefs.GetString("P0DIFF");
			string P1DIFF = PlayerPrefs.GetString("P1DIFF");
			string P2DIFF = PlayerPrefs.GetString("P2DIFF");

			// Analyze if the string, if not null, loads the values. If it is null, load default values.
			LoadDiffFiles (AllDiff4D, P0DIFF, 0); // 4D array, players pref string, player number
			LoadDiffFiles (AllDiff4D, P1DIFF, 1);
			LoadDiffFiles (AllDiff4D, P2DIFF, 2);

			// Copies Primary 4D array to the Temp Array
			CopyArrayToTemp();

			LoadValues = false;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void MasterUPDATE( ) // For use with the Save button on the game's edit screen (Saves changes to 4D array)
	{
		int PlayerNum = PlayerInfo.playerNumber;		// Grabs the player number from the PlayerInfo script

		// Save each sliders current difficulty level back to the 4D array
		if (GameNum == 0) // Moving Targets
		{
			AllDiff4D [PlayerNum, GameNum, SlideNum, (int)GameObject.Find ("PTOMasterSlideredit").GetComponent<Slider> ().value] = (int)my_slider.value;
			PTOToSave = true;
		}
		if (GameNum == 1) // Tracker
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

	public void CHANGE( )// Changes the individual settings on master slider change on normal screen from non-temp array
	{
		// SlideNum is the particular slider number (for each slider) and the MasterSlider value is the particular difficulty
		// These two comprise the location for the array of difficulty values

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

	public void CHANGEedit( ) // Changes the individual settings on master slider change on edit screen
	{
		// SlideNum is the particular slider number (for each slider) and the MasterSlider value is the particular difficulty
		// These two comprise the location for the array of difficulty values

		int PlayerNum = PlayerInfo.playerNumber;		// Grabs the player number from the PlayerInfo script

		if (GameNum == 0) // Moving Targets
		{
			int difficulty = AllDiff4D [PlayerNum, GameNum, SlideNum, (int)GameObject.Find ("PTOMasterSlideredit").GetComponent<Slider>().value];
			if (my_slider) { my_slider.value = difficulty; }
		}
		if (GameNum == 1) // Tracker
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

	public void ChangeMasterEdit( ) // Changes the Master slider on edit screen from regular master slider
	{		
		int PlayerNum = PlayerInfo.playerNumber;		// Grabs the player number from the PlayerInfo script
		
		if (GameNum == 0) // Moving Targets
		{ my_slider.value = GameObject.Find ("PTOMasterSlider").GetComponent<Slider>().value; }
		if (GameNum == 1) // Tracker
		{ my_slider.value = GameObject.Find ("EDBMasterSlider").GetComponent<Slider>().value; }
		if (GameNum == 2) // Converging Rings
		{ my_slider.value = GameObject.Find ("RingsMasterSlider").GetComponent<Slider>().value; }
	}

	public void ChangeMaster( ) // Changes the Master slider on regular screen from edit master slider
	{		
		int PlayerNum = PlayerInfo.playerNumber;		// Grabs the player number from the PlayerInfo script
		
		if (GameNum == 0) // Moving Targets
		{ my_slider.value = GameObject.Find ("PTOMasterSlideredit").GetComponent<Slider>().value; }
		if (GameNum == 1) // Tracker
		{ my_slider.value = GameObject.Find ("EDBMasterSlideredit").GetComponent<Slider>().value; }
		if (GameNum == 2) // Converging Rings
		{ my_slider.value = GameObject.Find ("RingsMasterSlideredit").GetComponent<Slider>().value; }
	}

	public void SliderTempUpdate( ) // Changes the current difficulty individual settings on Temp 4D array
	{
		int PlayerNum = PlayerInfo.playerNumber;		// Grabs the player number from the PlayerInfo script
		
		if (GameNum == 0) // Moving Targets
		{
			AllDiff4DTemp [PlayerNum, GameNum, SlideNum, (int)GameObject.Find ("PTOMasterSlider").GetComponent<Slider>().value] = (int)my_slider.value;
		}
		if (GameNum == 1) // Tracker
		{
			AllDiff4DTemp [PlayerNum, GameNum, SlideNum, (int)GameObject.Find ("EDBMasterSlider").GetComponent<Slider>().value] = (int)my_slider.value;
		}
		if (GameNum == 2) // Converging Rings
		{
			AllDiff4DTemp [PlayerNum, GameNum, SlideNum, (int)GameObject.Find ("RingsMasterSlider").GetComponent<Slider>().value] = (int)my_slider.value;
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

		if(game == TotalGames) // Default load all games for player pNumber
		{
			for (int w = 0; w < TotalGames; w++)
			{
				for (int x = 0; x < MaxSliders; x++)
				{
					for (int y = 0; y < MaxMasterLvl; y++) 	// Misc values to begin with in each 4D array
					{ my4DArray[pNumber,w,x,y] = y*2; }		// CHANGE LOAD DEFAULT VALUES HERE
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
		string player = "P";							// Sets up the P#DIFF string to save to difficulty setting string to players preference
		int PlayerNum = PlayerInfo.playerNumber;		// Grabs the player number from the PlayerInfo script
		player += PlayerNum.ToString();					// For player number
		player += "DIFF";

		// Debug.Log("Player String to save is: " + player);

		// Send 4D array to get saved
		string saveString = SaveDiffFiles( AllDiff4D );	// Convert 4D array to comma separated string (for a specific player) to a string
		PlayerPrefs.SetString(player, saveString);		// Saves the string to the players preference

		// Resets the boolean, if saved 
		if (GameNum == 0 && PTOToSave == true)		// Save PTOdiff for Moving Targets
		{ PTOToSave = false; }						// Resets the need to save bool

		if (GameNum == 1 && EDBToSave == true) 		// Save EDBdiff for Tracker
		{ EDBToSave = false; }						// Resets the need to save bool

		if (GameNum == 2 && RingsToSave == true)	// Save Ringsdiff for Converging Rings
		{ RingsToSave = false; }					// Resets the need to save bool
	}

	public void SavePrefsTemp() // Records the adjusted difficulties back to a players preference temp "file" 
	{
		string player = "P";							// Sets up the P#DIFF string to save to difficulty setting string to players preference
		int PlayerNum = PlayerInfo.playerNumber;		// Grabs the player number from the PlayerInfo script
		player += PlayerNum.ToString();					// For player number
		player += "DIFFTEMP";

		// Send 4D array to get saved
		string saveString = SaveDiffFiles( AllDiff4DTemp );	// Convert 4D array to comma separated string (for a specific player) to a string
		PlayerPrefs.SetString(player, saveString);		// Saves the string to the players preference
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

	// Copies the Master array to the Temp 4D array
	public void CopyArrayToTemp( ) 
	{
		for (int w = 0; w < TotalPlayers; w++)
		{
			for (int x = 0; x < TotalGames; x++)
			{
				for (int y = 0; y < MaxSliders; y++)
				{
					for (int z = 0; z < MaxMasterLvl; z++)
					{
						AllDiff4DTemp [w,x,y,z] = AllDiff4D [w,x,y,z];
					}
				}
			}
		}
	}

	// Grabs the difficulty level of the master slider and saves it to the players preference P#G#LVL and saves settings to players prefs string
	public void SetPlayerDiffLvl( ) 
	{
		string pNum = PlayerInfo.playerNumber.ToString();
		string gNum = GameNum.ToString();
		int currentDiff = 1;

		// Sets current difficulty level based on game and master slider value
		if (GameNum == 0)
		{ currentDiff = (int)GameObject.Find ("PTOMasterSlider").GetComponent<Slider>().value;}
		if (GameNum == 1)
		{ currentDiff = (int)GameObject.Find ("EDBMasterSlider").GetComponent<Slider>().value;}
		if (GameNum == 2)
		{ currentDiff = (int)GameObject.Find ("RingsMasterSlider").GetComponent<Slider>().value;}

		// Saves the current difficulty level to the players preference
		PlayerPrefs.SetInt("P" + pNum + "G" + gNum + "LVL", currentDiff);

		// Save the updated Temp 4D array to players prefs
		string player = "P";							// Sets up the P#DIFF string to save to difficulty setting string to players preference
		int PlayerNum = PlayerInfo.playerNumber;		// Grabs the player number from the PlayerInfo script
		player += PlayerNum.ToString();					// For player number
		player += "DIFFTEMP";
		
		// Debug.Log("Player String to save is: " + player);
		
		// Send 4D array to get saved
		string saveString = SaveDiffFiles( AllDiff4DTemp );	// Convert 4D array to comma separated string (for a specific player) to a string
		PlayerPrefs.SetString(player, saveString);			// Saves the string to the players preference
	}

	// Sets the master difficulty setting on the normal screen upon screen change
	public void SetGameLevel( )
	{
		string pnum = PlayerInfo.playerNumber.ToString();
		string gnum = GameNum.ToString();
		int lvl = PlayerPrefs.GetInt("P" + pnum + "G" + gnum + "LVL");
		my_slider.value = (float)lvl;

		// Issue where if user level is 1, slider setting values are not set correctly, must "move" the master slider, then move it back
		if (lvl == 1) // For all levels
		{
			my_slider.value = (float)2;
			my_slider.value = (float)lvl;
		}
	}

	// Sets the master difficulty setting on the normal screen upon screen change
	public void SetGameSliders( )
	{
		int PlayerNum = PlayerInfo.playerNumber;			// Grabs the player number from the PlayerInfo script
		string MasterSliderName = "";
		
		if (GameNum == 0) // Moving Targets
		{ MasterSliderName = "PTOMasterSlider"; }
		if (GameNum == 1) // Moving Targets
		{ MasterSliderName = "EDBMasterSlider"; }
		if (GameNum == 2) // Moving Targets
		{ MasterSliderName = "RingsMasterSlider"; }
		
		int difficulty = AllDiff4DTemp [PlayerNum, GameNum, SlideNum, (int)GameObject.Find (MasterSliderName).GetComponent<Slider>().value];
		if (my_slider) { my_slider.value = difficulty; } // For each individual difficulty slider
	}
	
}
