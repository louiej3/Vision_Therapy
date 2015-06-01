using UnityEngine;
using System.Collections;

/// <summary>
/// Game manager for moving targets game
/// </summary>
public class MovingTargetsGameManager : Mechanic 
{
	private Background background;

    protected float backgroundOpacity;
    // The speed that the background spins
    protected float backgroundSpeed;
    // The number of targets needed to win

	private TextMesh score;
	private GameObject winText;

	public Target targetPrefab;

	// The current state of the game
	public MovingTargetsState CurrentState { get; private set; }

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

		// Load the players setting preference string (all sliders and their difficulty level
		string playernum = PlayerPrefs.GetInt("PlayerNumber").ToString();
		string playerdiff = PlayerPrefs.GetString("P" + playernum + "DIFF");
		Debug.Log("PDIFF is: " + playernum );
		
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
					// Converts each string element to an int in the 4D array
					my2DArray [x,y] = int.Parse (wordsDiff [z]); ///
					z++;
				}
			}
		}
		
		// Associate each difficulty for each setting
		int CurretDiff = 2;

		// Difficulty settings assignment
		maxTargetsOnScreen = my2DArray[1,CurretDiff];				// Number of targets on the screen
		minTargetSpeed = ((float)my2DArray[2,CurretDiff])/2;		// Minimum target speed
		maxTargetSpeed = ((float)my2DArray[3,CurretDiff])/2;		// Maximum target speed
		targetScale = ((float)my2DArray[4,CurretDiff])/4;			// Size of targets
		targetTimeout = (float)my2DArray[5,CurretDiff];				// Target timeout in seconds
		targetOpacity = ((float)my2DArray[6,CurretDiff])/10;		// Clarity of target
		backgroundOpacity = ((float)my2DArray[7,CurretDiff])/10;	// Clarity of background
		targetSpawnInterval = (float)my2DArray[8,CurretDiff];		// Spawn interval of targets
		backgroundSpeed = (float)my2DArray[9,CurretDiff];			// Speed of background image
		targetsToWin = my2DArray[10,CurretDiff];					// Number of successful hits to win

		// Original settings
//		maxTargetsOnScreen = MovingTargetsSettings.maxTargetsOnScreen;
//		targetScale = MovingTargetsSettings.targetScale;
//		targetOpacity = MovingTargetsSettings.targetOpacity;
//		minTargetSpeed = MovingTargetsSettings.minTargetSpeed;
//		maxTargetSpeed = MovingTargetsSettings.maxTargetSpeed;
//		targetTimeout = MovingTargetsSettings.targetTimeout;
//		targetSpawnInterval = MovingTargetsSettings.targetSpawnInterval;
//		backgroundOpacity = MovingTargetsSettings.backgroundOpacity;
//		backgroundSpeed = MovingTargetsSettings.backgroundSpeed;
//		targetsToWin = MovingTargetsSettings.targetsToWin;
		
		targetMan = GetComponent<TargetManager>();
		background = GameObject.Find("Background").GetComponent<Background>();
		background.GetComponent<SpinMove>().Speed = backgroundSpeed;
		background.Opacity = backgroundOpacity;

		score = GameObject.Find("Score").GetComponent<TextMesh>();

		float height = Camera.main.orthographicSize;
		float width = height * Camera.main.aspect;

		score.transform.position = new Vector3(-width + 2.5f, 
			height - score.transform.localScale.y, score.transform.position.z);

		winText = GameObject.Find("WinText");

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
		score.text = targetMan.Hits + " / " + targetsToWin + " targets hit";
		
		if (targetMan.Hits >= targetsToWin)
		{
			CurrentState = MovingTargetsState.WIN;
		}

		if (gameTime.lap() >= targetSpawnInterval && 
			targetMan.NumberOfActiveObjects < maxTargetsOnScreen)
		{
			spawnTarget();
		}
	}

    protected override void winBehavior()
    {
        targetMan.disableAllTargets();
		winText.transform.position = Vector2.zero;
		
		base.winBehavior();
    }

    public void spawnTarget()
    {
        // Instantiate the target prefab
        Target target = Instantiate(targetPrefab) as Target;

        // Generate random x position
        float worldHeight = Camera.main.orthographicSize - target.transform.lossyScale.y / 2;
		float x = Random.Range(-worldHeight, worldHeight);

        // Generate random y position
        float worldWidth = Mathf.Sqrt(Mathf.Pow(worldHeight, 2) - Mathf.Pow(x, 2));
        float y = Random.Range(-worldWidth, worldWidth);

        Debug.Log(string.Format("X:{0}, Y:{1}", x, y));
        // Generate random speed
        float speed = Random.Range(minTargetSpeed, maxTargetSpeed);

        // Position and set target speed
        target.transform.position = new Vector2(x, y);

        target.GetComponent<OrbitMove>().SpeedFactor = speed;
		target.Scale = targetScale;
		target.Opacity = targetOpacity;
		target.TimeOut = targetTimeout;

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
