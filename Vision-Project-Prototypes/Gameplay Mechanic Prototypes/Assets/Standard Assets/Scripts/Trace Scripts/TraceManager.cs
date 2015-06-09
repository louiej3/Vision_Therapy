using UnityEngine;
using System.Collections;

/// <summary>
/// TraceManager controls a group of prefabricated GameObjects that act
/// as the path for the user to trace.
/// This class relies on a GameManager type class to provide it with GameObjects
/// in order to keep coupling low.
/// </summary>
public class TraceManager : MonoBehaviour
{

	// Keeps track of every segment in the trace route
	private ArrayList traceRoute;

	// Use this for initialization
	void Start()
	{
		traceRoute = new ArrayList();
	}

	// Update is called once per frame
	void Update()
	{
	}

	/// <summary>
	/// Checks every segment in the trace route to see if any of them collide
	/// with the user's touch.
	/// </summary>
	/// <returns>True on collision, false if not colliding.</returns>
	public bool onRoute()
	{
		if (Input.touchCount > 0)
		{
			// Get the user's touch position
			Touch touch = Input.GetTouch(0);
			Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);

			// Check each segment to see if it collided with the user's touch
			foreach (GameObject g in traceRoute)
			{
				if (g.GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
				{
					Debug.Log("On route");
					return true;
				}
			}
			Debug.Log("Off route - touch not colliding");
			return false;
		}
		Debug.Log("Off route - no touch present");
		return false;
	}

	/// <summary>
	/// Positions a GameObject between two points and adds it to the trace route.
	/// </summary>
	/// <param name="segment">The segment being added to the trace route.</param>
	/// <param name="point1">The point at the start of this segment.</param>
	/// <param name="point2">The point at the end of thie segment.</param>
	public void addSegment(GameObject segment, Vector2 point1, Vector2 point2)
	{
		// Position the segment halfway between point1 and point2
		Vector2 newPoint = (point1 + point2) / 2;
		segment.transform.position = newPoint;

		// Stretch the cube to fit between point1 and point2
		float stretch = Vector2.Distance(point1, point2);
		segment.transform.localScale = new Vector3(1f, stretch, 1f);

		// Turn the cube to face point2
		Vector2 faceDirection = (Vector2)segment.transform.position - point2;
		segment.transform.LookAt(segment.transform.position + new Vector3(0, 0, 1), faceDirection);

		traceRoute.Add(segment);
	}
}
