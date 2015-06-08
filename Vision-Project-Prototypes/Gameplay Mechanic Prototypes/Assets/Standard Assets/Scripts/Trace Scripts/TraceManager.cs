using UnityEngine;
using System.Collections;

public class TraceManager : MonoBehaviour
{

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

	public bool onRoute()
	{
		if (Input.touchCount > 0)
		{
			Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);

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
