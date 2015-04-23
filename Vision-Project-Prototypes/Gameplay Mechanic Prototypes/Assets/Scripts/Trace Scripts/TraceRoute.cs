using UnityEngine;
using System.Collections;

public class TraceRoute : MonoBehaviour 
{

	protected GameObject segment;
	protected GameObject p1;
	protected GameObject p2;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void createSegment(Vector2 point1, Vector2 point2)
	{
		// Create the trace segment
		segment = GameObject.CreatePrimitive(PrimitiveType.Cube);

		// Position the segment halfway between point1 and point2
		Vector2 newPoint = (point1 + point2) / 2;
		segment.transform.position = newPoint;

		// Stretch the cube to fit between point1 and point2
		float stretch = Vector2.Distance(point1, point2);
		segment.transform.localScale = new Vector3(1f, 1f, stretch);
		
		// Turn the cube to face point2
		segment.transform.LookAt(point2);
	}
}
