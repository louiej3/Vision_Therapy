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
		p1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		p1.transform.position = point1;
		p2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		p2.transform.position = point2;
		
		segment = GameObject.CreatePrimitive(PrimitiveType.Cube);
		Vector2 newPoint = (point1 + point2) / 2;
		segment.transform.position = newPoint;
		float stretch = Vector2.Distance(point1, point2);
		segment.transform.localScale = new Vector3(1f, 1f, stretch);
		segment.transform.LookAt(point2);
	}
}
