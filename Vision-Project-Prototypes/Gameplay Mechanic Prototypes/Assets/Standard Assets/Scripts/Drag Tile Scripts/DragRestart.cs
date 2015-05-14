using UnityEngine;
using System.Collections;

public class DragRestart : Target 
{

	// Use this for initialization

	protected override void tapBehavior()
	{
		Application.LoadLevel("Drag Tile Prototype");
	}
}
