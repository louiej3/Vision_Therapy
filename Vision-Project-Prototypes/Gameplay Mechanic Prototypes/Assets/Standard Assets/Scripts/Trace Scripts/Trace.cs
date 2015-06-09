using UnityEngine;
using System.Collections;

/// <summary>
/// Trace leaves a trail behind the user's touch as it moves and lets
/// the user see the path that they have traced.
/// This class is attached to a GameManager.
/// </summary>
public class Trace : MonoBehaviour
{

	// An empty game object with a TrailRenderer attached to it
	public GameObject trail;
	// The maximum speed that the trail object that follow the user's touch
	public float maxFollowSpeed = 400f;
	// The min/max life of the trail left behind the TrailRenderer
	public float minLifeTime = 0f;
	public float maxLifeTime = 5f;
	// The unique finger ID of the touch that trail is following
	private int traceFingerId = -1;

	// Use this for initialization
	void Start()
	{
		trail = new GameObject();
		// Z axis is -1 so the trail is drawn on top of objects
		trail.transform.position = new Vector3(0f, 0f, -1f);
		trail.AddComponent<TrailRenderer>();
		trail.GetComponent<TrailRenderer>().time = 0f;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.touchCount > 0)
		{
			// The the user's touch position
			Touch touch = Input.GetTouch(0);
			Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			Vector3 touchPos = new Vector3(worldPoint.x, worldPoint.y, -1f);

			// Record the ID of the touch being used to trace
			if (Input.touchCount == 1)
			{
				traceFingerId = touch.fingerId;
			}

			if (touch.phase == TouchPhase.Began && traceFingerId == touch.fingerId)
			{
				// Prevents the trail from jumping across the screen if the
				// touch is picked up and placed in a different spot
				trail.GetComponent<TrailRenderer>().time = minLifeTime;
				trail.transform.position = touchPos;
				trail.GetComponent<TrailRenderer>().time = maxLifeTime;
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				// Clean up the trail
				trail.GetComponent<TrailRenderer>().time = minLifeTime;
			}
			else if (traceFingerId == touch.fingerId)
			{
				// Make the trail object follow the user's touch
				trail.transform.position = Vector3.MoveTowards(trail.transform.position, touchPos, maxFollowSpeed);
			}
		}
	}
}
