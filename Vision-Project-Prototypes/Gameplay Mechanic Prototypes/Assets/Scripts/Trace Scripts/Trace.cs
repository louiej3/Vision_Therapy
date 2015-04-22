using UnityEngine;
using System.Collections;

public class Trace : MonoBehaviour 
{

	private GameObject trail;
	private float maxFollowSpeed = 400f;
	
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
			Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);

			trail.GetComponent<TrailRenderer>().time = 5f;
			trail.transform.position = Vector2.MoveTowards(transform.position, touchPos, maxFollowSpeed);

			if (touch.phase == TouchPhase.Ended)
			{
				trail.GetComponent<TrailRenderer>().time = 0f;
			}
		}
	}
}
