using UnityEngine;
using System.Collections;

/// <summary>
/// Game manager for moving targets game
/// </summary>
public class MovingTargetsGameManager : MonoBehaviour 
{

	// The maximum number of targets that can be on the
	// screen at once.
	private int maxTargetsOnScreen;
	// The range that the targets' speed can be
	private float minTargetSpeed;
	private float maxTargetSpeed;
	// The time between each new target being spawned
	private float targetSpawnInterval;

	// The number of targets needed to win
	private int targetsToWin;

	private StopWatch timer;

	private TargetManager targetMan;

	private Background background;

	public Target targetPrefab;

	// The current state of the game
	private MovingTargetsState currentState;

	public enum MovingTargetsState
	{
		PLAY,
		PAUSE,
		WIN,
		LOSE
	}
	
	// Use this for initialization
	void Start () 
	{
		maxTargetsOnScreen = MovingTargetsSettings.maxTargetsOnScreen;
		minTargetSpeed = MovingTargetsSettings.minTargetSpeed;
		maxTargetSpeed = MovingTargetsSettings.maxTargetSpeed;
		targetSpawnInterval = MovingTargetsSettings.targetSpawnInterval;
		targetsToWin = MovingTargetsSettings.targetsToWin;

		timer = new StopWatch();
		targetMan = GetComponent<TargetManager>();
		background = GameObject.Find("Background").GetComponent<Background>();

		currentState = MovingTargetsState.PLAY;
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (currentState)
		{ 
			case MovingTargetsState.PLAY:
				playBehavior();
				background.spin();
				break;

			case MovingTargetsState.WIN:
				winBehavior();
				break;
		}
	}

	private void playBehavior()
	{
		ArrayList targets = targetMan.Targets;
		int activeTargets = 0;

		if (targetMan.Hits >= targetsToWin)
		{
			currentState = MovingTargetsState.WIN;
		}

		foreach (Target t in targets)
		{
			if (t.isActiveAndEnabled)
			{
				activeTargets++;
			}
		}

		if (timer.lap() >= targetSpawnInterval && activeTargets < maxTargetsOnScreen)
		{
			spawnTarget();
		}
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
		target.GetComponent<OrbitMove>().SPEEDFACTOR = speed;

		// Add target to target manager
		targetMan.addTarget(target);
		
		// Restart the spawn timer
		timer.start();
	}

	public MovingTargetsState getState()
	{
		return currentState;
	}
}
