using UnityEngine;
using System.Collections;

public class ConvergingGameManager : Mechanic 
{
	// The current state of the game
	public static ConvergeState currentState { get; private set; }

    private ConObjectManager conMan;

	private TextMesh score;
	private GameObject winMenu;

	private Background background;

	// A converging object prefab. This object is set in the Unity
	// scene by dragging an existing converging object prefab into
	// the convergePrefab field in the object with this script
	// attached to it.
	public ConvergingObjects convergePrefab;
	
	// The opacity of the boomerangs
	private float boomerangOpacity;
	// The scale of the boomerangs, boomerangs are square
	private float boomerangScale;
	// The number of boomerangs for each converging object
	private int numberOfBoomerangs;

	// Multiplies converge time by this value to determine how
	// far off the user can be when they tap the object
	private float marginOfError = 0.1f;

	// Game states for this mechanic
	public enum ConvergeState
	{
		PLAY,
		PAUSE,
		WIN,
		LOSE
	}

	// Use this for initialization
	void Start () 
	{
		base.Start();

		// Load the players setting preference string (all sliders and their difficulty level)
		string playernum = PlayerPrefs.GetInt("PlayerNumber").ToString();
		string playerdiff = PlayerPrefs.GetString("P" + playernum + "DIFFTEMP");
		Debug.Log("PDIFFTEMP is: " + playernum );
		
		// Unpack string into 2D array for settings
		char[] delimiterChars = { ',' };							// Delimiter characters to look for and removes them
		string[] wordsDiff = playerdiff.Split(delimiterChars);		// Splits the playerspref string into the individial bits (One big array)
		
		int [,] my2DArray;											// Create 2D array to hold the setting for this players game
		my2DArray = new int[int.Parse (wordsDiff [1]),int.Parse (wordsDiff [2])]; // Length of the sizes in the 2nd and 3rd element of the array
		
		// Starts after the first 3 elements check (First 3 are game#, Slider#, and Diff#) & first 2 games' data
		Debug.Log("Game number is: " + wordsDiff [0] );
		int z = 3 + (2 * int.Parse (wordsDiff [1]) * int.Parse (wordsDiff [2]) ); // Times 2 to get to the 3rd part of the string
		int stringLength = wordsDiff.Length;						// Length of the given string of user saved data
		for (int x = 0; x < int.Parse (wordsDiff[1]); x++)			// For each Slider
		{
			for (int y = 0; y < int.Parse (wordsDiff[2]); y++)		// For each difficulty of the slider
			{
				if (z < stringLength)
				{
					// Converts each string element to an int in the 2D array
					my2DArray [x,y] = int.Parse (wordsDiff [z]); ///
					z++;
				}
			}
		}
		
		// Associate each difficulty for each setting
		int CurrentDiff = PlayerPrefs.GetInt("P" + playernum + "G2LVL");

		// Difficulty settings assignment
		maxTargetsOnScreen = my2DArray[1,CurrentDiff];				// Maximum number of rings sets
		minTargetSpeed = (float)my2DArray[2,CurrentDiff];			// Minimum ring speed
		maxTargetSpeed = (float)my2DArray[3,CurrentDiff];			// Maximum ring speed
		targetScale = ((float)my2DArray[4,CurrentDiff])/4;			// Size of targets (scaled)
		boomerangScale = ((float)my2DArray[4,CurrentDiff])/4;		// Size of distractors
		targetTimeout = (float)my2DArray[5,CurrentDiff];			// Ring time out in seconds
		boomerangOpacity = ((float)my2DArray[6,CurrentDiff])/10;	// Clarity of distractors (scaled)
		numberOfBoomerangs = my2DArray[7,CurrentDiff];				// Number of distractors
		targetSpawnInterval = (float)my2DArray[8,CurrentDiff];		// Ring set spawn interval
		targetsToWin = my2DArray[9,CurrentDiff];					// Number of successful hits to win
		targetOpacity = ((float)my2DArray[10,CurrentDiff])/10;		// Clarity of the center point (scaled)
		backgroundOpacity = ((float)my2DArray[11,CurrentDiff])/10;	// Clarity of the background (scaled)

		// To check for min/max speed, if min speed is larger, set min speed to max speed (which is lower)
		if(minTargetSpeed > maxTargetSpeed)
		{ minTargetSpeed = maxTargetSpeed; }
		
		conMan = GetComponent<ConObjectManager>();
        targetMan = conMan;
		
		conMan.MarginOfError = marginOfError;

		// Put the score at the top center of the screen
		score = GameObject.Find("Score").GetComponent<TextMesh>();
		score.transform.position = new Vector3(0f, Camera.main.orthographicSize
			- score.transform.localScale.y, score.transform.position.z);

		winMenu = GameObject.Find("WinMenu");

		currentState = ConvergeState.PLAY;

        gameSession = GameObject.Find("GameSession").GetComponent<GameSession>();
        dbConnection = GameObject.Find("Database").GetComponent<Database>();

		background = GameObject.Find("Background").GetComponent<Background>();
		background.Opacity = backgroundOpacity;

        mechanicType = "Converging Rings";
		
		spawnConverge();

		gameTime.start();
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (currentState)
		{
			case ConvergeState.PLAY:
				playBehavior();
				break;

			case ConvergeState.WIN:
				winBehavior();
				break;
		}
	}

	protected override void playBehavior()
	{
		// Displays the user's current score while they are playing
		score.text = conMan.Hits + " / " + targetsToWin + " targets hit";
		
		// Reached the number of required targets, go to WIN state
		if (conMan.Hits >= targetsToWin)
		{
			currentState = ConvergeState.WIN;
		}
		
		// Spawn a new converging object if the spawn interval is finished
		// and the number of converging objects on the screen does not
		// exceed the maximum amount of converging objects allowed.
		if (gameTime.lap() >= targetSpawnInterval 
			&& conMan.NumberOfActiveObjects < maxTargetsOnScreen)
		{
			spawnConverge();
		}
	}

	protected override void winBehavior()
	{
		// Clear the screen of all targets so extra data is not collected
		conMan.disableAllTargets();
		// Move the win message into place
		winMenu.transform.position = Vector2.zero;
		
		base.winBehavior();
	}

	private void spawnConverge()
	{
		// Instantiate the converging object prefab of choice and set its
		// values
		ConvergingObjects co = Instantiate(convergePrefab) as ConvergingObjects;
		co.TimeOut = targetTimeout;
		co.Scale = targetScale;
		co.Opacity = targetOpacity;

		// How long it should take the boomerangs to reach the center
		// starting from their farthest point
		float convergeTime = Random.Range(minTargetSpeed, maxTargetSpeed);

		// Spawn the converging object within the inner 80% of the screen
		float worldHeight = 0.8f * (Camera.main.orthographicSize - co.Scale / 2);
		float y = Random.Range(-worldHeight, worldHeight);

		// Spawn the converging object within the inner 80% of the screen
		float worldWidth = 0.8f * ((Camera.main.orthographicSize * Camera.main.aspect) - co.Scale / 2);
		float x = Random.Range(-worldWidth, worldWidth);

		// Set the center of the converging object and its boomerangs
		co.set(numberOfBoomerangs, new Vector2(x, y), convergeTime, boomerangScale, boomerangOpacity);
		
		conMan.addTarget(co);

		gameTime.start();
	}

    public override MechanicData packData()
    {
        MechanicData data = base.packData();

        data.secondaryOpacity = boomerangOpacity;
        data.secondaryScale = boomerangScale;

        return data;
    }
}
