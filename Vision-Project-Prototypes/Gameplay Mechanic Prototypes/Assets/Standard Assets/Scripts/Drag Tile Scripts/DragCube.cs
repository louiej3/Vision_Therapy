using UnityEngine;
using System.Collections;

public class DragCube : DragTile 
{

	protected override void pickUpBehavior()
	{
		Debug.Log("Cube Picked Up");
	}

	protected override void releaseBehavior()
	{
		Debug.Log("Cube Released");
	}
}
