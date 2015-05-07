using UnityEngine;
using System.Collections;

public class ConvergingGameManager : MonoBehaviour 
{

	private StopWatch timer;

	private ConvergingManager conMan;

	// The time in seconds between each converging object spawn
	public float convergeSpawnRate = 5f;
	// The number of boomerangs for each converging object
	public float convergeNumber = 5f;
	// The minimum time it takes for the booomerangs to converge
	public float minConvergeTime = 1f;
	// The maximum time it takes for the booomerangs to converge
	public float maxConvergeTime = 4f;

	// Use this for initialization
	void Start () 
	{
		timer = new StopWatch();
		conMan = GetComponent<ConvergingManager>();
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
		float convergeTime = Random.Range(minConvergeTime, maxConvergeTime);
		
		//ConvergingObjects co = GetComponent<ConvergingObjects>().set();
	}
}
