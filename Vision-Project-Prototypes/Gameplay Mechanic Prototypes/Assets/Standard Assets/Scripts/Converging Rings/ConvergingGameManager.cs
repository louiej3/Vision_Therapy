using UnityEngine;
using System.Collections;

public class ConvergingGameManager : MonoBehaviour 
{

	private StopWatch timer;

	private ConObjectManager conMan;

	// The time in seconds between each converging object spawn
	public float convergeSpawnRate = 5f;
	// The number of boomerangs for each converging object
	public int convergeNumber = 5;
	// The minimum time it takes for the booomerangs to converge
	public float minConvergeTime = 1f;
	// The maximum time it takes for the booomerangs to converge
	public float maxConvergeTime = 4f;

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
		ConvergingObjects conObj = Instantiate(Resources.Load("Converging Rings Prefabs/ConvergingObject", 
			typeof(ConvergingObjects))) as ConvergingObjects;
		
		float convergeTime = Random.Range(minConvergeTime, maxConvergeTime);

		float worldHeight = Camera.main.orthographicSize - conObj.getScale() / 2;
		float x = Random.Range(-worldHeight, worldHeight);

		float worldWidth = (Camera.main.orthographicSize / Camera.main.aspect) - conObj.getScale() / 2;
		float y = Random.Range(-worldWidth, worldWidth);

		conObj.set(convergeNumber, new Vector2(x, y), convergeTime);

		conMan.addConverge(conObj);

		timer.start();
	}
}
