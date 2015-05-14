using UnityEngine;
using System.Collections;

public class ConvergingGameManager : MonoBehaviour 
{

	private StopWatch timer;

	private ConObjectManager conMan;
	
	// The current state of the game
	private ConvergeState currentState;

	public ConvergingObjects convergePrefab;

	// The maximum amount of converging objects on the screen
	// at one time
	public int maxConvergeOnScreen;
	// The time between each converging object beign spawned
	public float convergeSpawnInterval;
	// The minimum time it takes for the boomerangs to intersect
	public float minConvergeTime;
	// The maximum time it takes for the boomerangs to intersect
	public float maxConvergeTime;
	// The number of boomerangs for each converging object
	public int numberOfBoomerangs;
	// The amount of converging objects that need to be tapped
	// before the user wins
	public int convergesToWin;

	// The transparency of the boomerangs
	public float boomerangOpacity = 1f;
	// The scale of the boomerangs, boomerangs are square
	public float boomerangScale = 1f;

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
		minConvergeTime = ConvergingSettings.minConvergeTime;
		maxConvergeTime = ConvergingSettings.maxConvergeTime;
		boomerangOpacity = ConvergingSettings.boomerangOpacity;
		boomerangScale = ConvergingSettings.boomerangScale;
		numberOfBoomerangs = ConvergingSettings.numberOfBoomerangs;
		convergesToWin = ConvergingSettings.convergesToWin;
		
		timer = new StopWatch();
		
		conMan = GetComponent<ConObjectManager>();
		conMan.MarginOfError = ConvergingSettings.marginOfError;

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
		ArrayList con = conMan.Converging;
		
		int activeConverges = 0;

		if (conMan.SuccessfulHits >= convergesToWin)
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
		co.TimeOut = ConvergingSettings.convergeTimeOut;
		co.Scale = ConvergingSettings.centerScale;
		co.Opacity = ConvergingSettings.centerOpacity;

		float convergeTime = Random.Range(minConvergeTime, maxConvergeTime);

		float worldHeight = Camera.main.orthographicSize - co.Scale / 2;
		float x = Random.Range(-worldHeight, worldHeight);

		float worldWidth = (Camera.main.orthographicSize / Camera.main.aspect) - co.Scale / 2;
		float y = Random.Range(-worldWidth, worldWidth);

		co.set(numberOfBoomerangs, new Vector2(x, y), convergeTime);

		conMan.addConverge(co);

		timer.start();
	}
}
