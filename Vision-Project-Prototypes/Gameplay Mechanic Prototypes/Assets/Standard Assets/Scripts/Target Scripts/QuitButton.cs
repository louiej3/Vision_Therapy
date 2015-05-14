using UnityEngine;
using System.Collections;

public class QuitButton : Target 
{

	// Use this for initialization
	public override void Start () 
    {
	
	}
	
	protected override void tapBehavior()
    {
        Application.Quit();
    }
}
