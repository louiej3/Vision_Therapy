using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Quick and dirty restart button.
/// </summary>

public class RestartButton : MonoBehaviour
{
	public String level = "";
	
	void Update()
	{
		if (Input.touchCount > 0)
		{
			Touch tap = Input.GetTouch(0);
			
			// Gets the location of the user's touch
			Vector3 worldPoint = Camera.main.ScreenToWorldPoint(tap.position);
			// Converts the location to a Vector2
			Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);

			// Checks to see if the user's touch collided with this target's collider
			if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos)
					&& tap.phase == TouchPhase.Began)
			{
				Application.LoadLevel(level);
			}
		}
	}
}
