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
	private float minTargetSpeed;
	private float maxTargetSpeed;
	// The time before a target disappears
	private float targetTimeout;
	// The time between each new target being spawned
	private float targetSpawnInterval;
	// The number of targets needed to win
	private int targetsToWin;

	private int numberOfTrackTargets;
	private int numberOfDummyTargets;

	private StopWatch timer;

	private TargetManager targetMan;

	public Target targetPrefab;

	// The current state of the game
	public TrackerState CurrentState { get; private set; }

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
		minTargetSpeed = TrackerSettings.minTargetSpeed;
		maxTargetSpeed = TrackerSettings.maxTargetSpeed;
		targetTimeout = TrackerSettings.targetTimeout;
		targetSpawnInterval = TrackerSettings.targetSpawnInterval;
		targetsToWin = TrackerSettings.targetsToWin;

		numberOfTrackTargets = TrackerSettings.numberOfTrackTargets;
		numberOfDummyTargets = TrackerSettings.numberOfDummyTargets;

		timer = new StopWatch();

		targetMan = GetComponent<TargetManager>();
		
		CurrentState = TrackerState.STARTUP;

		StartCoroutine(startUp(3));
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
		
	}

	private void winBehavior()
	{
		
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

        // Generate random speed
        float speed = Random.Range(minTargetSpeed, maxTargetSpeed);

        // Position and set target speed
        target.transform.position = new Vector2(x, y);
        //target.GetComponent<OrbitMove>().SpeedFactor = speed;
		target.Scale = targetScale;
		target.Opacity = targetOpacity;
		target.TimeOut = targetTimeout;

        // Add target to target manager
        targetMan.addTarget(target);

        // Restart the spawn timer
        timer.start();
    }

	IEnumerator startUp(float waitTime)
	{
		for (int i = 0; i < numberOfTrackTargets; i++)
		{
			spawnTarget();
		}
		
		yield return new WaitForSeconds(waitTime);
		
		for (int i = 0; i < numberOfDummyTargets; i++)
		{
			spawnTarget();
		}
		
		yield return new WaitForSeconds(waitTime);
		
		CurrentState = TrackerState.PLAY;
	}
}
