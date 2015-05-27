using UnityEngine;
using System.Collections;

public class Track : Target 
{
	protected override void tapBehavior()
	{
		// Turn this object green on touch
		GetComponent<SpriteRenderer>().color = new Color(0f, 255f, 0f, 
			GetComponent<SpriteRenderer>().color.a);
	}
}
