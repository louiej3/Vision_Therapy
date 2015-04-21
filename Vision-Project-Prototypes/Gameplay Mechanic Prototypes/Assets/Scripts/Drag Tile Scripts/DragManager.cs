using UnityEngine;
using System.Collections;

public class DragManager : MonoBehaviour 
{

	private DragTile dt;

	// Use this for initialization
	void Start () 
	{
		dt = null;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Touch touch = Input.GetTouch(0);

		if (touch.phase == TouchPhase.Ended)
		{
			dt = null;
		}

		Debug.Log("Drag Manager");
	}

	public void addDragTile(DragTile drag)
	{
		dt = drag;
	}

	public bool isDragging()
	{
		return dt != null;
	}
}
