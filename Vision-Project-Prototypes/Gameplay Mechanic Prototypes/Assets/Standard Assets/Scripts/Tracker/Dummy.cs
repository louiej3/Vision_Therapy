using UnityEngine;
using System.Collections;

/// <summary>
/// A subclass of Target that defines tap behavior. This is the script for
/// the decoy objects in the Tracker game.
/// </summary>

public class Dummy : Target 
{
	protected override void tapBehavior()
	{
		// Turn this object red on touch
		GetComponent<SpriteRenderer>().color = new Color(255f, 0f, 0f,
			GetComponent<SpriteRenderer>().color.a);
	}
}
