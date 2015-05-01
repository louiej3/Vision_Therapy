using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{

	private StopWatch timer;
	public float owlInterval = 5f;
	private TargetManager targetMan;
	
	// Use this for initialization
	void Start () 
	{
		timer = new StopWatch();
		targetMan = GetComponent<TargetManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (timer.lap() >= 5f)
		{
			spawnOwl();
		}
	}

	public void spawnOwl()
	{
		Target owl = Instantiate(Resources.Load("Poke The Owls/Owl", typeof(Target))) as Target;
		
		float worldHeight = Camera.main.orthographicSize - owl.transform.lossyScale.y / 2;
		float x = Random.Range(-worldHeight, worldHeight);
		
		float worldWidth = Mathf.Sqrt(Mathf.Pow(worldHeight, 2) - Mathf.Pow(x, 2));
		float y = Random.Range(-worldWidth, worldWidth);

		owl.transform.position = new Vector2(x, y);

		targetMan.addTarget(owl);
		
		timer.start();
	}
}
