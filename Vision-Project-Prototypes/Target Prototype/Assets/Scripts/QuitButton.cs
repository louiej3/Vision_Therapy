using UnityEngine;
using System.Collections;

public class QuitButton : Target 
{

	// Use this for initialization
	void Start () 
    {
	
	}
	
	protected override void tapBehavior()
    {
        Application.Quit();
    }
}
