using UnityEngine;
using System.Collections;

public class Trace : MonoBehaviour 
{

	GameObject trail;
	
	// Use this for initialization
	void Start () 
	{
		trail = new GameObject();
		trail.transform.position = Vector2.zero;
		trail.AddComponent<TrailRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			trail.transform.position = Vector2.MoveTowards(transform.position, touchPos, maxFollowSpeed);
		}
	}
}
