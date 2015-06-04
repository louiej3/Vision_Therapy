using UnityEngine;
using System.Collections;

/// <summary>
/// This is an object that the user can pick up and drag around the screen.
/// </summary>

public class DragTile : MonoBehaviour 
{
	// Keeps track of whether or not this object is currently picked up
	private bool pickedUp = false;
	// The speed that the drag tile follows the user's touch
	protected float maxFollowSpeed = 200f;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	public virtual void Update () 
	{
		if (Input.touchCount > 0)
		{
			// Get touch position and convert it to a vector2
			Touch touch = Input.GetTouch(0);
			Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);

			// If the user's touch collides with this object's collider
			if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos)
				&& touch.phase == TouchPhase.Began)
			{
				pickedUp = true;
				pickUpBehavior();
			}
			else if (pickedUp && touch.phase == TouchPhase.Ended)
			{
				pickedUp = false;
				releaseBehavior();
			}	

			// Make the drag tile follow the user's touch if it is picked up
			if (pickedUp)
			{
				transform.position = Vector2.MoveTowards(transform.position, touchPos, maxFollowSpeed);
			}
		}
	}

	// What the drag tile does when picked up, if anything
	protected virtual void pickUpBehavior() { }

	// What the drag tile does when released, if anything
	protected virtual void releaseBehavior() { }
}
