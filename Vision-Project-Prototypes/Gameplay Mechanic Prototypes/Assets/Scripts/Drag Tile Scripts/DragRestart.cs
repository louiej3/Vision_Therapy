using UnityEngine;
using System.Collections;

public class DragRestart : Target 
{

	// Use this for initialization
	void Start () 
	{
	
	}

	protected override void tapBehavior()
	{
		Application.LoadLevel("Drag Tile Prototype");
	}
}
