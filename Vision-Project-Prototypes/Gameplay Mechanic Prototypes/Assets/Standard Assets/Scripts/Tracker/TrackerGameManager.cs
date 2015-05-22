using UnityEngine;
using System.Collections;

/// <summary>
/// Game manager for tracker game
/// </summary>
public class TrackerGameManager : MonoBehaviour 
{
    
	// The scale of the targets. The targets are squares.
	private float targetScale;
	// The transperancy of the targets
	private float targetOpacity;
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

	private StopWatch timer;

	private TargetManager targetMan;

	private ArrayList targets;

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
		targetScale = TrackerSettings.targetScale;
		targetOpacity = TrackerSettings.targetOpacity;
		minChangeTime = TrackerSettings.minChangeTime;
		maxChangeTime = TrackerSettings.maxChangeTime;
		numberOfTrackTargets = TrackerSettings.numberOfTrackTargets;
		numberOfDummyTargets = TrackerSettings.numberOfDummyTargets;
		shuffleTime = TrackerSettings.shuffleTime;
		startUpTime = TrackerSettings.startUpTime;
		targetSpeed = TrackerSettings.targetSpeed;

		timer = new StopWatch();

		targetMan = GetComponent<TargetManager>();

		targets = targetMan.Targets;

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

	private void playBehavior()
	{
		int found = 0;
		
		foreach (Target t in targets)
		{
			if (t.tag == "Track" && t.IsTapped == true)
			{
				found++;
			}
		}

		if (found == numberOfTrackTargets)
		{
			CurrentState = TrackerState.WIN;
		}
	}

	private void winBehavior()
	{
		Debug.Log("You win!");
		foreach (Target t in targets)
		{
			t.gameObject.SetActive(false);
		}
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
		track.gameObject.GetComponent<RandomStraightMove>().enabled = false;

        // Add target to target manager
		targetMan.addTarget(track);
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
		dummy.gameObject.GetComponent<RandomStraightMove>().enabled = false;

		// Add target to target manager
		targetMan.addTarget(dummy);
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
		
		yield return new WaitForSeconds(waitTime);
		
		// Spawn dummies
		for (int i = 0; i < numberOfDummyTargets; i++)
		{
			spawnDummy();
		}
		
		yield return new WaitForSeconds(waitTime);

		foreach (Target t in targets)
		{
			t.gameObject.GetComponent<RandomStraightMove>().enabled = true;
		}

		yield return new WaitForSeconds(shuffleTime);

		foreach (Target t in targets)
		{
			t.gameObject.GetComponent<RandomStraightMove>().enabled = false;
			t.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			t.GetComponent<Rigidbody2D>().fixedAngle = true;
		}

		CurrentState = TrackerState.PLAY;
	}
}
