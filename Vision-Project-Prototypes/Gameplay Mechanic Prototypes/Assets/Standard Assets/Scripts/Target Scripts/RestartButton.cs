using UnityEngine;
using System.Collections;

public class RestartButton : Target 
{

	// Use this for initialization
	public override void Start () 
    {
	
	}
	
	protected override void tapBehavior()
    {
        Application.LoadLevel("Target Prototype");
    }
}
