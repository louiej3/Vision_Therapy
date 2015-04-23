using UnityEngine;
using System.Collections;

public class Trace : MonoBehaviour 
{

	protected GameObject trail;
	protected float maxFollowSpeed = 400f;
	private int traceFingerId = -1;
	
	// Use this for initialization
	void Start () 
	{
		trail = new GameObject();
		// Z axis is -1 so the trail is drawn on top of objects
		trail.transform.position = new Vector3(0f, 0f, -1f);
		trail.AddComponent<TrailRenderer>();
		trail.GetComponent<TrailRenderer>().time = 0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			Vector3 touchPos = new Vector3(worldPoint.x, worldPoint.y, -1f);

			if (Input.touchCount == 1)
			{
				traceFingerId = touch.fingerId;
			}

			if (touch.phase == TouchPhase.Began && traceFingerId == touch.fingerId)
			{
				trail.GetComponent<TrailRenderer>().time = 0f;
				trail.transform.position = touchPos;
				trail.GetComponent<TrailRenderer>().time = 5f;
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				trail.GetComponent<TrailRenderer>().time = 0f;
			}
			else if (traceFingerId == touch.fingerId)
			{
				trail.transform.position = Vector3.MoveTowards(trail.transform.position, touchPos, maxFollowSpeed);
			}
		}
	}
}
