using UnityEngine;
using System.Collections;

public class ConvergingGameManager : MonoBehaviour 
{

	private StopWatch timer;

	private ConObjectManager conMan;

	// The maximum amount of converging objects on the screen
	// at one time
	private int maxConvergeOnScreen;
	// The time between each converging object beign spawned
	private float convergeSpawnInterval;
	// The number of boomerangs for each converging object
	private int numberOfBoomerangs;
	// The minimum time it takes for the booomerangs to converge
	private float minConvergeTime;
	// The maximum time it takes for the booomerangs to converge
	private float maxConvergeTime;
	// The amount of converging objects that need to be tapped
	// before the user wins
	private int convergesToWin;
	
	// The current state of the game
	private ConvergeState currentState;

	public ConvergingObjects convergePrefab;

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
		maxConvergeOnScreen = ConvergingSettings.maxConvergeOnScreen;
		convergeSpawnInterval = ConvergingSettings.convergeSpawnInterval;
		numberOfBoomerangs = ConvergingSettings.numberOfBoomerangs;
		minConvergeTime = ConvergingSettings.minConvergeTime;
		maxConvergeTime = ConvergingSettings.maxConvergeTime;
		convergesToWin = ConvergingSettings.convergesToWin;
		
		timer = new StopWatch();
		conMan = GetComponent<ConObjectManager>();
		currentState = ConvergeState.PLAY;
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

	private void playBehavior()
	{
		ArrayList con = conMan.getConverging();
		int activeConverges = 0;

		if (conMan.getSuccessfulHits() >= convergesToWin)
		{
			currentState = ConvergeState.WIN;
		}

		foreach (ConvergingObjects co in con)
		{
			if (co.isActiveAndEnabled)
			{
				activeConverges++;
			}
		}
		
		if (timer.lap() >= convergeSpawnInterval 
			&& activeConverges < maxConvergeOnScreen)
		{
			spawnConverge();
		}
	}

	private void winBehavior()
	{

	}

	private void spawnConverge()
	{
		ConvergingObjects co = Instantiate(convergePrefab) as ConvergingObjects;
		
		float convergeTime = Random.Range(minConvergeTime, maxConvergeTime);

		float worldHeight = Camera.main.orthographicSize - co.getScale() / 2;
		float x = Random.Range(-worldHeight, worldHeight);

		float worldWidth = (Camera.main.orthographicSize / Camera.main.aspect) - co.getScale() / 2;
		float y = Random.Range(-worldWidth, worldWidth);

		co.set(numberOfBoomerangs, new Vector2(x, y), convergeTime);

		conMan.addConverge(co);

		timer.start();
	}
}
