using UnityEngine;
using System.Collections;

/// <summary>
/// Game manager for tracker game
/// </summary>
public class TrackerGameManager : Mechanic 
{
    
	// The range that the targets' speed can be
	private float minChangeTime;
	private float maxChangeTime;
	// Number of targets that the user needs to track
	private int numberOfTrackTargets;
	// Number of dummy targets in the scene
	private int numberOfDummyTargets;
	// The amount of time that targets are allowed to move around
	private float shuffleTime;
	// The time between tracks spawning, dummies spawning, and the
	// game starting
	private float startUpTime;
	// The max speed of the targets
	private float targetSpeed;
	// The transperancy of the background
	private float backgroundOpacity = 1f;

	private TrackManager trackMan;

	private TextMesh score;
	private GameObject winText;

	private Background background;

	public Target trackPrefab;
	public Target dummyPrefab;

	// The current state of the game
	public static TrackerState CurrentState { get; private set; }

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
		string playerdiff = PlayerPrefs.GetString("P" + playernum + "DIFF");
		Debug.Log("PDIFF is: " + playernum );

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
		int CurretDiff = 2;

		numberOfTrackTargets = my2DArray[1,CurretDiff];
		numberOfDummyTargets = my2DArray[2,CurretDiff];

		targetScale = ((float)my2DArray[3,CurretDiff])/4;	// Target Size slider
		targetOpacity = ((float)my2DArray[5,CurretDiff])/10;
		targetSpeed = (float)my2DArray[7,CurretDiff]*2;
		shuffleTime = (float)my2DArray[4,CurretDiff];
		backgroundOpacity = ((float)my2DArray[6,CurretDiff])/10;

		minChangeTime = TrackerSettings.minChangeTime;
		maxChangeTime = TrackerSettings.maxChangeTime;
		startUpTime = TrackerSettings.startUpTime;


//		targetScale = TrackerSettings.targetScale;
//		targetOpacity = TrackerSettings.targetOpacity;
//		minChangeTime = TrackerSettings.minChangeTime;
//		maxChangeTime = TrackerSettings.maxChangeTime;
//		numberOfTrackTargets = TrackerSettings.numberOfTrackTargets;
//		numberOfDummyTargets = TrackerSettings.numberOfDummyTargets;
//		shuffleTime = TrackerSettings.shuffleTime;
//		startUpTime = TrackerSettings.startUpTime;
//		targetSpeed = TrackerSettings.targetSpeed;
//		backgroundOpacity = TrackerSettings.backgroundOpacity;

		trackMan = GetComponent<TrackManager>();
		targetMan = trackMan;

		score = GameObject.Find("Score").GetComponent<TextMesh>();

		winText = GameObject.Find("WinText");

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
		score.text = trackMan.SuccessfulHits + " / " + numberOfTrackTargets + " targets found";
		
		if (trackMan.SuccessfulHits == numberOfTrackTargets)
		{
			CurrentState = TrackerState.WIN;
		}
	}

	protected override void winBehavior()
	{
		trackMan.disableAllTargets();
		winText.transform.position = Vector2.zero;

		base.winBehavior();
	}

    private void spawnTrack()
    {
        // Instantiate the target prefab
        Target track = Instantiate(trackPrefab) as Target;

        // Generate random x position
        float worldHeight = Camera.main.orthographicSize - track.transform.lossyScale.y / 2;
		float x = Random.Range(-worldHeight, worldHeight);

        // Generate random y position
        float worldWidth = Mathf.Sqrt(Mathf.Pow(worldHeight, 2) - Mathf.Pow(x, 2));
        float y = Random.Range(-worldWidth, worldWidth);

        // Position tracker object
        track.transform.position = new Vector2(x, y);
		track.Scale = targetScale;
		track.Opacity = targetOpacity;

		track.gameObject.GetComponent<RandomStraightMove>().MinimumChangeTime = minChangeTime;
		track.gameObject.GetComponent<RandomStraightMove>().MaximumChangeTime = maxChangeTime;
		track.gameObject.GetComponent<RandomStraightMove>().Speed = targetSpeed;

        // Add target to target manager
		trackMan.addTarget(track);
    }

	private void spawnDummy()
	{
		// Instantiate the target prefab
		Target dummy = Instantiate(dummyPrefab) as Target;

		// Generate random x position
		float worldHeight = Camera.main.orthographicSize - dummy.transform.lossyScale.y / 2;
		float x = Random.Range(-worldHeight, worldHeight);

		// Generate random y position
		float worldWidth = Mathf.Sqrt(Mathf.Pow(worldHeight, 2) - Mathf.Pow(x, 2));
		float y = Random.Range(-worldWidth, worldWidth);

		// Position tracker object
		dummy.transform.position = new Vector2(x, y);
		dummy.Scale = targetScale;
		dummy.Opacity = targetOpacity;

		dummy.gameObject.GetComponent<RandomStraightMove>().MinimumChangeTime = minChangeTime;
		dummy.gameObject.GetComponent<RandomStraightMove>().MaximumChangeTime = maxChangeTime;
		dummy.gameObject.GetComponent<RandomStraightMove>().Speed = targetSpeed;

		// Add target to target manager
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
