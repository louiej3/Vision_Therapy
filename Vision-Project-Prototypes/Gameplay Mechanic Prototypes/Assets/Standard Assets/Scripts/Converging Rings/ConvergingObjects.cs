using UnityEngine;
using System.Collections;

public class ConvergingObjects : MonoBehaviour 
{

	GameObject point;
	GameObject ring1;
	GameObject ring2;
	Vector2 newVec1;
	Vector2 newVec2;
	
	// Use this for initialization
	void Start () 
	{
		point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		point.transform.position = new Vector2(3f, 3f);

		ring1 = Instantiate(Resources.Load("Converging Rings Prefabs/Ring", typeof(GameObject))) as GameObject;
		ring1.transform.position = new Vector2(0f, -3f);

		ring2 = Instantiate(Resources.Load("Converging Rings Prefabs/Ring", typeof(GameObject))) as GameObject;
		ring2.transform.position = new Vector2(-3f, 1f);

		newVec1 = Vector2.zero;
		newVec2 = Vector2.zero;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{	
		ring1.transform.position = Vector2.SmoothDamp(ring1.transform.position,
			point.transform.position, ref newVec1, 5f);

		ring2.transform.position = Vector2.SmoothDamp(ring2.transform.position,
			point.transform.position, ref newVec2, 5f);
	}
}
