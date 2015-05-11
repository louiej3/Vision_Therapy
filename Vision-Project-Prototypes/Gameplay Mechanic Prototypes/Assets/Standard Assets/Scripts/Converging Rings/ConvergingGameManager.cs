using UnityEngine;
using System.Collections;

public class ConvergingGameManager : MonoBehaviour 
{

	private StopWatch timer;

	private ConObjectManager conMan;

	// The time in seconds between each converging object spawn
	private float convergeSpawnRate = 5f;
	// The number of boomerangs for each converging object
	private int convergeNumber = 5;
	// The minimum time it takes for the booomerangs to converge
	private float minConvergeTime = 1f;
	// The maximum time it takes for the booomerangs to converge
	private float maxConvergeTime = 4f;
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
		if (timer.lap() >= convergeSpawnRate)
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

		co.set(convergeNumber, new Vector2(x, y), convergeTime);

		conMan.addConverge(co);

		timer.start();
	}
}
