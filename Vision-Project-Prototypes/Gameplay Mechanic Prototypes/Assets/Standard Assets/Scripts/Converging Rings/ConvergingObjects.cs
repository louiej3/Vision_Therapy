using UnityEngine;
using System.Collections;

public class ConvergingObjects : MonoBehaviour 
{

	// List of boomerangs associated with this convergence point
	protected ArrayList boomerangs;
	// The time it takes for all of the boomerangs to converge
	protected float convergeTime = 0f;
	// Tells whether or not this converging object has been successfully tapped
	protected bool isTapped = false;
	// The total time it took for this converging object to the tapped
	protected float lapTime = 0f;
	// How accurately the rings intersected when the user touched this converging object
	protected float accuracy = 0f;
	// Tells whether accuracy was in the margin of error
	public bool success = false;
	
	// The time before this object initiates timeout behavior
	protected float timeOut;
	// The scale of the center object
	protected float scale;
	// The transparency of the center of the converging object
	protected float opacity;

	protected StopWatch timer;

	public Boomerang boomerangPrefab;

	void Awake()
	{
		boomerangs = new ArrayList();
	}

	// Use this for initialization
	protected virtual void Start () 
	{
		timeOut = ConvergingSettings.convergeTimeOut;
		
		scale = ConvergingSettings.centerScale;
		transform.localScale = new Vector2(scale, scale);

		opacity = ConvergingSettings.centerOpacity;
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, opacity);

		timer = new StopWatch();
		timer.start();
	}
	
	// For subclasses that would like to implement behavior upon tap
	protected virtual void tapBehavior() { }

	public bool checkTouch(Touch tap)
	{
		Vector3 worldPoint = Camera.main.ScreenToWorldPoint(tap.position);
		Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);

		if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
		{
			// because the unit has been tapped, set the variables
			lapTime = timer.lap();

			// Mod the user's lapTime by the time it takes for a boomerang
			// to complete one cycle (convergeTime * 2), then subtract the
			// convergeTime and get the absolute value
			accuracy = Mathf.Abs((lapTime % (convergeTime * 2)) - convergeTime);

			isTapped = true;

			// If it is an inherited class, we can call the specific tap behavior
			tapBehavior();

			return true;
		}
		return false;   
	}

	public void converge()
	{
		if (!isTapped)
		{
			// If the first boomerang reaches its max distance, turn
			// all of them boomerangs around and go the other way
			if (Vector2.Distance(((Boomerang)boomerangs[0]).transform.position, transform.position)
				>= ((Boomerang)boomerangs[0]).distance)
			{
				turnAround();
			}

			// Move the boomerangs towards the center point
			foreach (Boomerang b in boomerangs)
			{
				b.transform.position += b.transform.up * b.speed * Time.smoothDeltaTime;
			}
		}
	}

	private void turnAround()
	{
		// Turn all of the boomerangs around to face the center point
		foreach (Boomerang b in boomerangs)
		{
			Vector2 convergeDirection = (Vector2)transform.position - (Vector2)b.transform.position;
			b.transform.LookAt(b.transform.position + new Vector3(0, 0, 1), convergeDirection);
		}
	}

	public void set(int numberOfObjects, Vector2 centerPoint, float time)
	{
		// Position the center point
		transform.position = centerPoint;

		// Create the amount of boomerangs specified by numberOfObjects
		for (int i = 0; i < numberOfObjects; i++)
		{
			// Instantiate boomerang
			Boomerang b = Instantiate(boomerangPrefab) as Boomerang;
			
			convergeTime = time;

			// Find the x coordinate of the boomerang relative to the center point
			float height = Camera.main.orthographicSize - Mathf.Abs(centerPoint.y - scale / 2);
			float y = Random.Range(centerPoint.y - height, centerPoint.y + height);

			// Find the y coordinate of the boomerang relative to the center point
			float width = Mathf.Sqrt(Mathf.Abs(Mathf.Pow(height, 2) - Mathf.Pow(y, 2)));
			float x = Random.Range(-width, width);

			// Position the boomerang
			b.transform.position = new Vector2(x, y);

			// Turn the point to face the center point
			Vector2 convergeDirection = centerPoint - (Vector2)b.transform.position;
			b.transform.LookAt(b.transform.position + new Vector3(0, 0, 1), convergeDirection);

			// The distance between this boomerang and the center point
			float distance = Vector2.Distance(b.transform.position, centerPoint);

			// The speed that this boomerang needs to travel to reach the center point
			// in time
			float speed = distance / time;

			// Set variables in this boomerang
			b.distance = distance;
			b.speed = speed;

			boomerangs.Add(b);
		}
	}

	public float getLapTime()
	{
		return lapTime;
	}

	public float getScale()
	{
		return scale;
	}

	public float getAccuracy()
	{
		return accuracy;
	}

	public bool timedOut()
	{
		if (timer.lap() >= timeOut)
		{
			return true;
		}
		return false;
	}

	public float getConvergeTime()
	{
		return convergeTime;
	}
}
