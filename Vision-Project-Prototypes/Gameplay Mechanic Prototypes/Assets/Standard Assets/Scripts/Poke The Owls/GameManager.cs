using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{

	// The maximum number of owls that can be on the
	// screen at once.
	private int maxOwlsOnScreen;
	// The transperancy of the owls
	private float owlOpacity;
	// The range that the owls' speed can be
	private float minOwlSpeed;
	private float maxOwlSpeed;
	// The time between each new owl being spawned
	private float owlSpawnInterval;

	// The transperancy of the spiral
	private float spiralOpacity;
	// The speed that the spiral spins
	private float spiralSpeed;

	// The number of owls needed to win
	private int owlsToWin;

	private StopWatch timer;

	private TargetManager targetMan;
	
	// Use this for initialization
	void Start () 
	{
		maxOwlsOnScreen = GetComponent<DifficultySettings>().maxOwlsOnScreen;
		owlOpacity = GetComponent<DifficultySettings>().owlOpacity;
		minOwlSpeed = GetComponent<DifficultySettings>().minOwlSpeed;
		maxOwlSpeed = GetComponent<DifficultySettings>().maxOwlSpeed;
		owlSpawnInterval = GetComponent<DifficultySettings>().owlSpawnInterval;
		spiralOpacity = GetComponent<DifficultySettings>().spiralOpacity;
		owlsToWin = GetComponent<DifficultySettings>().owlsToWin;

		timer = new StopWatch();
		targetMan = GetComponent<TargetManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (timer.lap() >= owlSpawnInterval)
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
		float speed = Random.Range(minOwlSpeed, maxOwlSpeed);

		// Position and set owl speed
		owl.transform.position = new Vector2(x, y);
		owl.GetComponent<OrbitMove>().SPEEDFACTOR = speed;

		// Add owl to target manager
		targetMan.addTarget(owl);
		
		// Restart the spawn timer
		timer.start();
	}
}
