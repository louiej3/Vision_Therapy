using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Game manager for moving targets game
/// </summary>
public class MovingTargetsGameManager : Mechanic 
{
	private Background background;
    // The speed that the background spins
    protected float backgroundSpeed;

	private TextMesh score;
	private GameObject winMenu;

	// A target prefab. This object is set in the Unity
	// scene by dragging an existing target prefab into
	// the target field in the object with this script
	// attached to it.
	public Target targetPrefab;

	// The current state of the game
	public MovingTargetsState CurrentState { get; private set; }

	// The different states of the Moving Targets game
	public enum MovingTargetsState
	{
		PLAY,
		PAUSE,
		WIN,
		LOSE
	}
	
	// Use this for initialization
	public override void Start () 
	{
        base.Start();

		// Load the players setting preference string (all sliders and their difficulty levels)
		string playernum = PlayerPrefs.GetInt("PlayerNumber").ToString();
		string playerdiff = PlayerPrefs.GetString("P" + playernum + "DIFFTEMP");
		Debug.Log("PDIFFTEMP is: " + playernum );
		
		// Unpack string into 2D array for settings
		char[] delimiterChars = { ',' };							// Delimiter characters to look for and removes them
		string[] wordsDiff = playerdiff.Split(delimiterChars);		// Splits the playerspref string into the individial bits (One big array)
		
		int [,] my2DArray;											// Create 2D array to hold the setting for this players game
		my2DArray = new int[int.Parse (wordsDiff [1]),int.Parse (wordsDiff [2])]; // Length of the sizes in the 2nd and 3rd element of the array
		
		// Starts after the first 3 elements check (First 3 are game#, Slider#, and Diff#)
		int z = 3;		// Game difficulty array data is right after the first 3 elements
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
		int CurrentDiff = PlayerPrefs.GetInt("P" + playernum + "G0LVL");

		// Difficulty settings assignment
		maxTargetsOnScreen = my2DArray[1,CurrentDiff];				// Number of targets on the screen
		minTargetSpeed = ((float)my2DArray[2,CurrentDiff])/2;		// Minimum target speed
		maxTargetSpeed = ((float)my2DArray[3,CurrentDiff])/2;		// Maximum target speed
		targetScale = ((float)my2DArray[4,CurrentDiff])/4;			// Size of targets
		targetTimeout = (float)my2DArray[5,CurrentDiff];			// Target timeout in seconds
		targetOpacity = ((float)my2DArray[6,CurrentDiff])/10;		// Clarity of target
		backgroundOpacity = ((float)my2DArray[7,CurrentDiff])/10;	// Clarity of background
		targetSpawnInterval = (float)my2DArray[8,CurrentDiff];		// Spawn interval of targets
		backgroundSpeed = (float)my2DArray[9,CurrentDiff];			// Speed of background image
		targetsToWin = my2DArray[10,CurrentDiff];					// Number of successful hits to win

		targetMan = GetComponent<TargetManager>();
		
		background = GameObject.Find("Background").GetComponent<Background>();
		background.GetComponent<SpinMove>().Speed = backgroundSpeed;
		background.Opacity = backgroundOpacity;

		score = GameObject.Find("Score").GetComponent<TextMesh>();

		// The height and width of the camera view
		float height = Camera.main.orthographicSize;
		float width = height * Camera.main.aspect;
		
		// Position the score in the top left of the screen
		score.transform.position = new Vector3(-width + 2.5f, 
			height - score.transform.localScale.y, score.transform.position.z);

		winMenu = GameObject.Find("WinMenu");

		CurrentState = MovingTargetsState.PLAY;
        mechanicType = "Moving Targets";
    }
	
	// Update is called once per frame
	void Update () 
	{
		switch (CurrentState)
		{ 
			case MovingTargetsState.PLAY:
				playBehavior();
				break;

			case MovingTargetsState.WIN:
				winBehavior();
				break;
		}
	}

	protected override void playBehavior()
	{
		// Displays the user's current score
		score.text = targetMan.Hits + " / " + targetsToWin + " targets hit";
		
		// Transition to WIN state if the required amount of targets
		// has been hit
		if (targetMan.Hits >= targetsToWin)
		{
			CurrentState = MovingTargetsState.WIN;
		}

		// Spawn a target if the spawn interval is over and the number of
		// targets on the screen does not exceed the maximum number allowed
		if (gameTime.lap() >= targetSpawnInterval && 
			targetMan.NumberOfActiveObjects < maxTargetsOnScreen)
		{
			spawnTarget();
		}
	}

    protected override void winBehavior()
    {
        // Clear the screen of targets so we don't collect extra data
		targetMan.disableAllTargets();
		// Position the win text in the center of the screen
		winMenu.transform.position = Vector2.zero;

		base.winBehavior();
    }

    public void spawnTarget()
    {
        // Instantiate the target prefab
        Target target = Instantiate(targetPrefab) as Target;
		target.Scale = targetScale;
		target.Opacity = targetOpacity;
		target.TimeOut = targetTimeout;

		// Generate random speed
		float speed = Random.Range(minTargetSpeed, maxTargetSpeed);
		target.GetComponent<OrbitMove>().SpeedFactor = speed;

        // Generate random x position
        float worldHeight = Camera.main.orthographicSize - target.Scale / 2;
		float y = Random.Range(-worldHeight, worldHeight);

        // Generate random y position within a circle with 'x' radius
        float worldWidth = Mathf.Sqrt(Mathf.Pow(worldHeight, 2) - Mathf.Pow(y, 2));
        float x = Random.Range(-worldWidth, worldWidth);

        // Position target
        target.transform.position = new Vector2(x, y);
		
		// Pick a float between 0 and 1, >= 0.5f clockwise
		// counter clockwise is default
        if (Random.value >= 0.5f)
        {
            target.GetComponent<OrbitMove>().IsClockwise = true;
        }

        // Add target to target manager
        targetMan.addTarget(target);

        // Restart the spawn timer
        gameTime.start();
    }

    public override MechanicData packData()
    {
        MechanicData data = base.packData();
        data.backgroundOpacity = backgroundOpacity;
        data.backgroundSpeed = backgroundSpeed;
        return data;
    }
}
