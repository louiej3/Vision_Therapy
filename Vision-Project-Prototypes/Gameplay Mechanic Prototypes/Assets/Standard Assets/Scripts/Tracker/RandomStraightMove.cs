using UnityEngine;
using System.Collections;

public class RandomStraightMove : Movement 
{

	private StopWatch timer;
	private float changeTime = 3f;
	private float speed = 0.5f;
	private float height;
	private float width;

	// Use this for initialization
	void Start () 
	{
		timer = new StopWatch();
		timer.start();
		changeTime = Random.Range(1f, 3f);

		height = Camera.main.orthographicSize;
		width = height * Camera.main.aspect;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (TrackerGameManager.CurrentState == TrackerGameManager.TrackerState.PLAY)
		{
			location.GetComponent<Rigidbody2D>().AddForce(location.up.normalized * speed);
			
			if (timer.lap() >= changeTime)
			{	
				float x = Random.Range(-1f, 1f);
				float y = Random.Range(-1f, 1f);
				location.up = new Vector2(x, y);

				float xPercent = Mathf.Abs(location.position.x / width);
				float yPercent = Mathf.Abs(location.position.y / height);

				float rand = Random.value;

				if (rand <= xPercent || rand <= yPercent)
				{
					//Quaternion rotation = Quaternion.LookRotation(Vector2.zero);
					//Vector2 newDirection = Vector3.RotateTowards(location.up, Vector2.zero, 20f, 0f);
					//location.up = newDirection;
					Vector2 look = Vector2.zero - (Vector2)location.position;
					location.LookAt(location.position + new Vector3(0, 0, 1), look);
				}

				changeTime = Random.Range(1f, 3f);

				timer.start();
			}
		}
	}
}
