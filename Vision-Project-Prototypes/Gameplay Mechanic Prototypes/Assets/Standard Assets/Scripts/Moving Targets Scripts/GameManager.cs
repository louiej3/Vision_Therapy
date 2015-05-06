using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{

	// The maximum number of owls that can be on the
	// screen at once.
	private int maxOwlsOnScreen;
	// The range that the owls' speed can be
	private float minOwlSpeed;
	private float maxOwlSpeed;
	// The time between each new owl being spawned
	private float owlSpawnInterval;

	// The number of owls needed to win
	private int owlsToWin;

	private StopWatch timer;

	private TargetManager targetMan;

	// The current state of the game
	private state currentState;

	public enum state
	{
		PLAY,
		PAUSE,
		WIN,
		LOSE
	}
	
	// Use this for initialization
	void Start () 
	{
		maxOwlsOnScreen = GetComponent<DifficultySettings>().maxOwlsOnScreen;
		minOwlSpeed = GetComponent<DifficultySettings>().minOwlSpeed;
		maxOwlSpeed = GetComponent<DifficultySettings>().maxOwlSpeed;
		owlSpawnInterval = GetComponent<DifficultySettings>().owlSpawnInterval;
		owlsToWin = GetComponent<DifficultySettings>().owlsToWin;

		timer = new StopWatch();
		targetMan = GetComponent<TargetManager>();

		currentState = state.PLAY;
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (currentState)
		{ 
			case state.PLAY:
				playBehavior();
				break;

			case state.WIN:
				winBehavior();
				break;
		}
	}

	private void playBehavior()
	{
		ArrayList targets = targetMan.getTargets();
		int activeTargets = 0;

		if (targetMan.getHits() >= owlsToWin)
		{
			currentState = state.WIN;
		}

		foreach (Target t in targets)
		{
			if (t.isActiveAndEnabled)
			{
				activeTargets++;
			}
		}

		if (timer.lap() >= owlSpawnInterval && activeTargets < maxOwlsOnScreen)
		{
			spawnOwl();
		}
	}

	private void winBehavior()
	{

	}

	public void spawnOwl()
	{
		// Instantiate the owl prefab
		Target owl = Instantiate(Resources.Load("Moving Targets Prefabs/Owl", typeof(Target))) as Target;
		
		// Generate random x position
		float worldHeight = Camera.main.orthographicSize - owl.transform.lossyScale.y / 2;
		float x = Random.Range(-worldHeight, worldHeight);
		
		// Generate random y position
		float worldWidth = Mathf.Sqrt(Mathf.Pow(worldHeight, 2) - Mathf.Pow(x, 2));
		float y = Random.Range(-worldWidth, worldWidth);

		// Generate random speed
		float speed = Random.Range(minOwlSpeed, maxOwlSpeed);

		// Position and set owl speed
		owl.transform.position = new Vector2(x, y);
		owl.GetComponent<OrbitMove>().SPEEDFACTOR = speed;

		// Add owl to target manager
		targetMan.addTarget(owl);
		
		// Restart the spawn timer
		timer.start();
	}

	public state getState()
	{
		return currentState;
	}
}
