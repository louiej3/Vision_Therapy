using UnityEngine;
using System.Collections;

public class RestartButton : Target 
{

	// Use this for initialization
	void Start () 
    {
	
	}
	
	protected override void tapBehavior()
    {
        Application.LoadLevel("Target Prototype");
    }
}
