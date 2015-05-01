using UnityEngine;
using System.Collections;

public class Owl : Target 
{

	// Update is called once per frame
	void Update () 
	{
	
	}

	protected override void tapBehavior()
	{
		gameObject.SetActive(false);
	}
}
