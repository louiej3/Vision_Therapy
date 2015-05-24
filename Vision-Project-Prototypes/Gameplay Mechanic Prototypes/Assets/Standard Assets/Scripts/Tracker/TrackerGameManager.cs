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

	private TrackManager trackMan;

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
		
		targetScale = TrackerSettings.targetScale;
		targetOpacity = TrackerSettings.targetOpacity;
		minChangeTime = TrackerSettings.minChangeTime;
		maxChangeTime = TrackerSettings.maxChangeTime;
		numberOfTrackTargets = TrackerSettings.numberOfTrackTargets;
		numberOfDummyTargets = TrackerSettings.numberOfDummyTargets;
		shuffleTime = TrackerSettings.shuffleTime;
		startUpTime = TrackerSettings.startUpTime;
		targetSpeed = TrackerSettings.targetSpeed;

		trackMan = GetComponent<TrackManager>();

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
		if (trackMan.SuccessfulHits == numberOfTrackTargets)
		{
			CurrentState = TrackerState.WIN;
		}
	}

	protected override void winBehavior()
	{
		base.winBehavior();
		Debug.Log("You win!");
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

		gameTime.start();
		CurrentState = TrackerState.PLAY;
	}
}
