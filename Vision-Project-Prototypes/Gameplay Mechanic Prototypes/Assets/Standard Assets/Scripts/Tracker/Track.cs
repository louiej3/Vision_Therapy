using UnityEngine;
using System.Collections;

/// <summary>
/// A subclass of Target that defines tap behavior. This is the script
/// for the objects that the user must follow in the Tracker game.
/// </summary>

public class Track : Target 
{
	protected override void tapBehavior()
	{
		// Turn this object green on touch
		GetComponent<SpriteRenderer>().color = new Color(0f, 255f, 0f, 
			GetComponent<SpriteRenderer>().color.a);
	}
}
