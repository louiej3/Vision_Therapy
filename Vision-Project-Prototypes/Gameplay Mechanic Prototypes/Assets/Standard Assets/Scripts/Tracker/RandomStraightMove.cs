using UnityEngine;
using System.Collections;

/// <summary>
/// This class moves an object in a smooth, straight, random pattern.
/// Force is applied to the object to move it so the object cannot
/// make sharp turns.
/// </summary>

public class RandomStraightMove : Movement 
{
	private StopWatch timer;
	
	// The amount of time before this object changes direction
	private float changeTime = 0f;
	
	// The height and width of the camera view
	private float height;
	private float width;

	// The range that changeTime can be within
	private float _minChangeTime;
	private float _maxChangeTime;

	// The speed of the object
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
			// Get a new random direction for the object
			float x = Random.Range(-1f, 1f);
			float y = Random.Range(-1f, 1f);
			location.up = new Vector2(x, y);

			// Calculate how close this object is to the edge of the screen
			// as a percentage
			float xPercent = Mathf.Abs(location.position.x / width);
			float yPercent = Mathf.Abs(location.position.y / height);
			
			// If only 40 percent of the screen is left in the x or y direction,
			// get a random value and compare it to xPercent and yPercent. The
			// closer the object is to an edge, the higher chance it has of
			// turning towards the center.
			if (xPercent > 0.6f || yPercent > 0.6f)
			{
				float rand = Random.value;
				
				// The chance of the object turning towards the center
				// increases as the object gets closer to an edge.
				if (rand <= xPercent || rand <= yPercent)
				{
					// Get the direction going from this object to the center
					Vector2 look = Vector2.zero - (Vector2)location.position;
					// Face the center
					location.LookAt(location.position + new Vector3(0, 0, 1), look);
				}
			}

			// Calculate a new random changeTime. changeTime is randomized
			// so all of the targets do not change direction at the same time.
			changeTime = Random.Range(_minChangeTime, _maxChangeTime);

			timer.start();
		}
	}

	// Executes whatever is in FixedUpdate once per frame
	void FixedUpdate () 
	{
		// Apply force in the object's up direction. Force is applied here
		// to make sure that it is applied evenly. The amount of times Update()
		// is called can vary while FixedUpdate() cannot.
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
