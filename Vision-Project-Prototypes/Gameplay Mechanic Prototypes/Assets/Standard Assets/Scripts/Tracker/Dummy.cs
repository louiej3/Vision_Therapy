using UnityEngine;
using System.Collections;

public class Dummy : Target 
{
	protected override void tapBehavior()
	{
		// Turn this object red on touch
		GetComponent<SpriteRenderer>().color = new Color(255f, 0f, 0f,
			GetComponent<SpriteRenderer>().color.a);
	}
}
