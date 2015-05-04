using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{

	private StopWatch timer;
	
	private TargetManager targetMan;
	
	// Determines the owls' speed
	public float minSpeed = 0.5f;
	public float maxSpeed = 1.5f;

	// Owl spawn time in seconds
	public float owlInterval = 3f;
	
	// Use this for initialization
	void Start () 
	{
		timer = new StopWatch();
		targetMan = GetComponent<TargetManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (timer.lap() >= owlInterval)
		{
			spawnOwl();
		}
	}

	public void spawnOwl()
	{
		// Instantiate the owl prefab
		Target owl = Instantiate(Resources.Load("Poke The Owls/Owl", typeof(Target))) as Target;
		
		// Generate random x position
		float worldHeight = Camera.main.orthographicSize - owl.transform.lossyScale.y / 2;
		float x = Random.Range(-worldHeight, worldHeight);
		
		// Generate random y position
		float worldWidth = Mathf.Sqrt(Mathf.Pow(worldHeight, 2) - Mathf.Pow(x, 2));
		float y = Random.Range(-worldWidth, worldWidth);

		// Generate random speed
		float speed = Random.Range(minSpeed, maxSpeed);

		// Position and set owl speed
		owl.transform.position = new Vector2(x, y);
		owl.GetComponent<OrbitMove>().SPEEDFACTOR = speed;

		// Add owl to target manager
		targetMan.addTarget(owl);
		
		// Restart the spawn timer
		timer.start();
	}
}
