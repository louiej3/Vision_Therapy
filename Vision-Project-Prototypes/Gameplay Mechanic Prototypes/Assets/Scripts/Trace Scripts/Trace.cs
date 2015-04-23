using UnityEngine;
using System.Collections;

public class Trace : MonoBehaviour 
{

	private GameObject trail;
	private float maxFollowSpeed = 400f;
	private int previousFingerId = -1;
	private int currentFingerId = -1;
	private bool firstTouch = false;
	
	// Use this for initialization
	void Start () 
	{
		trail = new GameObject();
		trail.transform.position = Vector2.zero;
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
			Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);

			if (Input.touchCount == 1)
			{
				currentFingerId = touch.fingerId;
			}

			if (touch.phase == TouchPhase.Began && currentFingerId == touch.fingerId)
			{
				trail.GetComponent<TrailRenderer>().time = 0f;
				trail.transform.position = touchPos;
				trail.GetComponent<TrailRenderer>().time = 5f;
				Debug.Log("Began touch");
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				trail.GetComponent<TrailRenderer>().time = 0f;
				Debug.Log("Ended touch");
			}
			else if (currentFingerId == touch.fingerId)
			{
				trail.transform.position = Vector2.MoveTowards(transform.position, touchPos, maxFollowSpeed);
				Debug.Log("Moved touch");
			}
		}
		else
		{
			currentFingerId = -1;
		}
	}
}
