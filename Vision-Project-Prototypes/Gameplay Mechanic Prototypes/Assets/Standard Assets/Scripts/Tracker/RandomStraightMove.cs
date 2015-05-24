using UnityEngine;
using System.Collections;

public class RandomStraightMove : Movement 
{

	private StopWatch timer;
	private float changeTime = 0f;
	private float height;
	private float width;

	private float _minChangeTime;
	private float _maxChangeTime;

	public float Speed { get; set; }

	// Use this for initialization
	void Start () 
	{
		timer = new StopWatch();
		timer.start();

		height = Camera.main.orthographicSize;
		width = height * Camera.main.aspect;
	}

	// Update is called once per frame
	void Update ()
	{
		if (timer.lap() >= changeTime)
		{
			float x = Random.Range(-1f, 1f);
			float y = Random.Range(-1f, 1f);
			location.up = new Vector2(x, y);

			float xPercent = Mathf.Abs(location.position.x / width);
			float yPercent = Mathf.Abs(location.position.y / height);
			
			if (xPercent > 0.6f || yPercent > 0.6f)
			{
				float rand = Random.value;
				
				if (rand <= xPercent || rand <= yPercent)
				{
					Vector2 look = Vector2.zero - (Vector2)location.position;
					location.LookAt(location.position + new Vector3(0, 0, 1), look);
				}
			}

			changeTime = Random.Range(_minChangeTime, _maxChangeTime);

			timer.start();
		}
	}

	void FixedUpdate () 
	{
		location.GetComponent<Rigidbody2D>().AddForce(location.up.normalized * Speed);
	}

	public float MinimumChangeTime
	{
		get
		{
			return _minChangeTime;
		}
		set
		{
			if (value >= 0)
			{
				_minChangeTime = value;
			}
			else
			{
				throw new System.Exception("Value cannot be negative");
			}
		}
	}

	public float MaximumChangeTime
	{
		get
		{
			return _maxChangeTime;
		}
		set
		{
			if (value >= 0)
			{
				_maxChangeTime = value;
			}
			else
			{
				throw new System.Exception("Value cannot be negative");
			}
		}
	}
}
