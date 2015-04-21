using UnityEngine;
using System.Collections;

public class TraceRestart : Target 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	protected override void tapBehavior()
	{
		Application.LoadLevel("Trace Prototype");
	}
}
