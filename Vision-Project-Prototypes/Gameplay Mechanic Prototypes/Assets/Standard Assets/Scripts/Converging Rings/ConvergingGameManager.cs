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

	public ConvergingObjects convergePrefab;

	// Use this for initialization
	void Start () 
	{
		timer = new StopWatch();
		conMan = GetComponent<ConObjectManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (timer.lap() >= convergeSpawnRate)
		{
			spawnConverge();
		}
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
