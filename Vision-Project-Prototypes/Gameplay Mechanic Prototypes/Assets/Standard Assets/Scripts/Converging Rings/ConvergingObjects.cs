using UnityEngine;
using System.Collections;

/// <summary>
/// This class controls the center of the converging object as well as
/// the boomerangs that move through its center.
/// </summary>

public class ConvergingObjects : Target 
{
	// List of boomerangs associated with this convergence point
	protected ArrayList boomerangs;
	// The time it takes for all of the boomerangs to converge
	public float ConvergeTime { get; protected set; }
	// Tells whether accuracy was in the margin of error
	public bool Success { get; set; }

	// A boomerang prefab. This object is set in the Unity
	// scene by dragging an existing boomerang prefab into
	// the boomerang field in the prefab with this script
	// attached to it.
	public Boomerang boomerangPrefab;

	void Awake()
	{
		boomerangs = new ArrayList();
	}

	public bool checkTouch(Touch tap)
	{
		// Get position of the user's touch
		Vector3 worldPoint = Camera.main.ScreenToWorldPoint(tap.position);
		// Convert the position of the touch to a Vector2
		Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);

		// Check if the touch collides with this object's collider
		if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
		{
			// because the unit has been tapped, set the variables
			LapTime = timeSinceSpawn.lap();
			IsTapped = true;

            // Mod the user's lapTime by the time it takes for a boomerang
            // to complete one cycle (convergeTime * 2), then subtract the
            // convergeTime and get the absolute value. The calculated value
			// will be a time in seconds that indicates how far off the user
			// was the from perfect time when all of the boomerangs intersect
			// (convergeTime).
            TapPrecision = Mathf.Abs((LapTime % (ConvergeTime * 2)) - ConvergeTime);

			// If it is an inherited class, we can call the specific tap behavior
			tapBehavior();

			return true;
		}
		return false;   
	}

	public void converge()
	{
		if (!IsTapped)
		{
			// If the first boomerang reaches its max distance, turn
			// all of them boomerangs around and go the other way
			if (Vector2.Distance(((Boomerang)boomerangs[0]).transform.position, transform.position)
				>= ((Boomerang)boomerangs[0]).Distance)
			{
				turnAround();
			}

			// Move the boomerangs towards the center point
			foreach (Boomerang b in boomerangs)
			{
				b.transform.position += b.transform.up * b.Speed * Time.smoothDeltaTime;
			}
		}
	}

	private void turnAround()
	{
		// Turn all of the boomerangs around to face the center point
		foreach (Boomerang b in boomerangs)
		{
			// Find the direction of the center point compared to the boomerang
			Vector2 convergeDirection = (Vector2)transform.position - (Vector2)b.transform.position;
			// Add 1 in the z-axis so the boomerang's up position is facing the center point
			b.transform.LookAt(b.transform.position + new Vector3(0, 0, 1), convergeDirection);
		}
	}

	/// <summary>
	/// Sets the attributes of the centerpoint and the boomerangs and positions them
	/// </summary>
	/// <param name="numberOfObjects">The number of boomerangs</param>
	/// <param name="centerPoint">The location of the centerpoint</param>
	/// <param name="time">The time it should take for a boomerang to reach the enter point</param>
	/// <param name="boomerangScale">The scale of the boomerangs</param>
	/// <param name="boomerangOpacity">The opacity of the boomerangs</param>
	public void set(int numberOfObjects, Vector2 centerPoint, float time, 
		float boomerangScale, float boomerangOpacity)
	{
		// Position the center point
		transform.position = centerPoint;

		// Create the amount of boomerangs specified by numberOfObjects
		for (int i = 0; i < numberOfObjects; i++)
		{
			// Instantiate boomerang and set attributes
			Boomerang b = Instantiate(boomerangPrefab) as Boomerang;
			b.Scale = boomerangScale;
			b.Opacity = boomerangOpacity;
			
			// The amount of time it takes for the boomerange to reach the
			// center starting from the boomerangs farthest point
			ConvergeTime = time;
			
			// Determine the how far in the x and y direction that the boomerang can be
			// placed before it goes off screen
			float height = Camera.main.orthographicSize - Mathf.Abs(centerPoint.y + _scale / 2);
			float width = (Camera.main.aspect * Camera.main.orthographicSize) - Mathf.Abs(centerPoint.x + _scale / 2);
			float y = 0f;
			float x = 0f;

			if (height <= width)
			{
				y = Random.Range(-height, height);
				float newWidth = Mathf.Sqrt(Mathf.Abs(Mathf.Pow(height, 2) - Mathf.Pow(y, 2)));
				x = Random.Range(-newWidth, newWidth);
			}
			else
			{
				x = Random.Range(-width, width);
				float newHeight = Mathf.Sqrt(Mathf.Abs(Mathf.Pow(width, 2) - Mathf.Pow(x, 2)));
				y = Random.Range(-newHeight, newHeight);
			}

			// Position the boomerang
			b.transform.position = new Vector2(centerPoint.x + x, centerPoint.y + y);

			// Turn the point to face the center point
			Vector2 convergeDirection = centerPoint - (Vector2)b.transform.position;
			b.transform.LookAt(b.transform.position + new Vector3(0, 0, 1), convergeDirection);

			// The distance between this boomerang and the center point
			float distance = Vector2.Distance(b.transform.position, centerPoint);

			// The speed that this boomerang needs to travel to reach the center point
			// in time
			float speed = distance / time;

			// Set variables in this boomerang
			b.Distance = distance;
			b.Speed = speed;

			boomerangs.Add(b);
		}
	}
}
