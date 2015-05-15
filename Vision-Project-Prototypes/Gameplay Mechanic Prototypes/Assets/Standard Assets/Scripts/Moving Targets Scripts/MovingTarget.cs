using UnityEngine;
using System.Collections;

public class MovingTarget : Target 
{

	// Update is called once per frame
	public override void Update () 
	{
		if (timedOut())
		{
			gameObject.SetActive(false);
		}
	}

	protected override void tapBehavior()
	{
		gameObject.SetActive(false);
	}
}
