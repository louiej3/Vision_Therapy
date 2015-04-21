using UnityEngine;
using System.Collections;

public class DragManager : MonoBehaviour 
{

	public bool draggingTile;

	// Use this for initialization
	void Start () 
	{
		draggingTile = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public bool isDragging()
	{
		return draggingTile;
	}
}
