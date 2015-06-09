using UnityEngine;
using System.Collections;

/// <summary>
/// Game manager for tracker game
/// </summary>
public class TrackerGameManager : Mechanic 
{
	// The range that the targets' speed can be
	private float minChangeTime = 0.5f;
	private float maxChangeTime = 1.5f;
	// The time between tracks spawning, dummies spawning, and the
	// game starting
	private float startUpTime = 3f;

	// Number of targets that the user needs to track
	private int numberOfTrackTargets;
	// Number of dummy targets in the scene
	private int numberOfDummyTargets;
	// The amount of time that targets are allowed to move around
	private float shuffleTime;
	// The max speed of the targets
	private float targetSpeed;

	private TrackManager trackMan;

	private TextMesh score;
	private GameObject winMenu;

	private Background background;

	// Prefabs for the dummy and track game objects. These prefabs
	// are set in the Unity scene by dragging the appropriate prefab
	// into either the trackPrefab or dummyPrefab field in the object
	// with this script attached to it.
	public Target trackPrefab;
	public Target dummyPrefab;

	// The current state of the Tracker game
	public static TrackerState CurrentState { get; private set; }

	// The different states for the Tracker game
	public enum TrackerState
	{
		STARTUP,
		PLAY,
		PAUSE,
		WIN,
		LOSE
	}
	
	// Use this for initialization
	void Start () 
	{
		base.Start();

		// Load the players setting preference string (all sliders and their difficulty level
		string playernum = PlayerPrefs.GetInt("PlayerNumber").ToString();
		string playerdiff = PlayerPrefs.GetString("P" + playernum + "DIFFTEMP");
		Debug.Log("PDIFFTEMP is: " + playernum );

		// Unpack string into 2D array for settings
		char[] delimiterChars = { ',' };							// Delimiter characters to look for and removes them
		string[] wordsDiff = playerdiff.Split(delimiterChars);		// Splits the playerspref string into the individial bits (One big array)

		int [,] my2DArray;											// Create 2D array to hold the setting for this players game
		my2DArray = new int[int.Parse (wordsDiff [1]),int.Parse (wordsDiff [2])]; // Length of the sizes in the 2nd and 3rd element of the array

		// Starts after the first 3 elements check (First 3 are game#, Slider#, and Diff#)
		int z = 3 + ( int.Parse (wordsDiff [1]) * int.Parse (wordsDiff [2]) );
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
		int CurrentDiff = PlayerPrefs.GetInt("P" + playernum + "G1LVL");

		// Difficulty settings assignment
		numberOfTrackTargets = my2DArray[1,CurrentDiff];			// Number of targets
		numberOfDummyTargets = my2DArray[2,CurrentDiff];			// Number of false targets
		targetScale = ((float)my2DArray[3,CurrentDiff])/4;			// Target Size slider
		shuffleTime = (float)my2DArray[4,CurrentDiff];				// Time spent mixing together
		targetOpacity = ((float)my2DArray[5,CurrentDiff])/10;		// Clarity of targets
		backgroundOpacity = ((float)my2DArray[6,CurrentDiff])/10;	// Clarity of background
		targetSpeed = (float)my2DArray[7,CurrentDiff]*2;			// Speed of the targets movement

		trackMan = GetComponent<TrackManager>();
		targetMan = trackMan;

		score = GameObject.Find("Score").GetComponent<TextMesh>();

		winMenu = GameObject.Find("WinMenu");

		background = GameObject.Find("Background").GetComponent<Background>();
		background.Opacity = backgroundOpacity;

		mechanicType = "Tracker";

		CurrentState = TrackerState.STARTUP;

		StartCoroutine(startUp(startUpTime));
    }
	
	// Update is called once per frame
	void Update () 
	{
		switch (CurrentState)
		{
			case TrackerState.STARTUP:
				break;
			
			case TrackerState.PLAY:
				playBehavior();
				break;

			case TrackerState.WIN:
				winBehavior();
				break;
		}
	}

	protected override void playBehavior()
	{
		score.text = trackMan.Hits + " / " + numberOfTrackTargets + " targets found";
		
		if (trackMan.Hits == numberOfTrackTargets)
		{
			CurrentState = TrackerState.WIN;
		}
	}

	protected override void winBehavior()
	{
		// Clear the screen
		trackMan.disableAllTargets();
		// Move the win message to the center of the screen
		winMenu.transform.position = Vector2.zero;

		base.winBehavior();
	}

    private void spawnTrack()
    {
        // Instantiate the track prefab and set attributes
        Target track = Instantiate(trackPrefab) as Target;
		track.Scale = targetScale;
		track.Opacity = targetOpacity;
		track.gameObject.GetComponent<RandomStraightMove>().MinimumChangeTime = minChangeTime;
		track.gameObject.GetComponent<RandomStraightMove>().MaximumChangeTime = maxChangeTime;
		track.gameObject.GetComponent<RandomStraightMove>().Speed = targetSpeed;

        // Generate random x position within camera view
        float worldHeight = Camera.main.orthographicSize - track.Scale / 2;
		float y = Random.Range(-worldHeight, worldHeight);

		// Generate random y position within camera view
		float worldWidth = (Camera.main.orthographicSize * Camera.main.aspect) - track.Scale / 2;
		float x = Random.Range(-worldWidth, worldWidth);

        // Position track object
        track.transform.position = new Vector2(x, y);

        // Add track to target manager
		trackMan.addTarget(track);
    }

	private void spawnDummy()
	{
		// Instantiate the dummy prefab and set attributes
		Target dummy = Instantiate(dummyPrefab) as Target;
		dummy.Scale = targetScale;
		dummy.Opacity = targetOpacity;
		dummy.gameObject.GetComponent<RandomStraightMove>().MinimumChangeTime = minChangeTime;
		dummy.gameObject.GetComponent<RandomStraightMove>().MaximumChangeTime = maxChangeTime;
		dummy.gameObject.GetComponent<RandomStraightMove>().Speed = targetSpeed;

		// Generate random x position within camera view
		float worldHeight = Camera.main.orthographicSize - dummy.Scale / 2;
		float y = Random.Range(-worldHeight, worldHeight);

		// Generate random y position within camera view
		float worldWidth = (Camera.main.orthographicSize * Camera.main.aspect) - dummy.Scale / 2;
		float x = Random.Range(-worldWidth, worldWidth);

		// Position dummy object
		dummy.transform.position = new Vector2(x, y);

		// Add dummy to target manager
		trackMan.addTarget(dummy);
	}

	IEnumerator startUp(float waitTime)
	{
		// Position walls
		float height = Camera.main.orthographicSize;
		float width = height * Camera.main.aspect;

		GameObject leftWall = GameObject.Find("LeftWall");
		leftWall.transform.position = new Vector2(-width - leftWall.transform.localScale.x / 2, 0f);
		leftWall.transform.localScale = new Vector2(1f, height * 2);

		GameObject rightWall = GameObject.Find("RightWall");
		rightWall.transform.position = new Vector2(width + rightWall.transform.localScale.x / 2, 0f);
		rightWall.transform.localScale = new Vector2(1f, height * 2);

		GameObject topWall = GameObject.Find("TopWall");
		topWall.transform.position = new Vector2(0f, height + topWall.transform.localScale.y / 2);
		topWall.transform.localScale = new Vector2(width * 2, 1f);

		GameObject bottomWall = GameObject.Find("BottomWall");
		bottomWall.transform.position = new Vector2(0f, -height - bottomWall.transform.localScale.y / 2);
		bottomWall.transform.localScale = new Vector2(width * 2, 1f);

		// Spawn track objects
		for (int i = 0; i < numberOfTrackTargets; i++)
		{
			spawnTrack();
		}

		// Freeze track objects
		trackMan.freezeTargets();
		
		yield return new WaitForSeconds(waitTime);
		
		// Spawn dummies
		for (int i = 0; i < numberOfDummyTargets; i++)
		{
			spawnDummy();
		}

		// Freeze dummies
		trackMan.freezeTargets();
		
		yield return new WaitForSeconds(waitTime);

		// Unfreeze all targets
		trackMan.unfreezeTargets();

		// Let the targets shuffle around
		yield return new WaitForSeconds(shuffleTime);

		// Freeze targets so user can select the right ones
		trackMan.freezeTargets();

		// Position the score text
		score.transform.position = new Vector3(0f, Camera.main.orthographicSize
			- score.transform.localScale.y, score.transform.position.z);

		gameTime.start();
		CurrentState = TrackerState.PLAY;
	}
}
