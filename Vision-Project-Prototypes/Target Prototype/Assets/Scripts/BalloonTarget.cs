using UnityEngine;
using System.Collections;

public class BalloonTarget : Target 
{

	// Use this for initialization
	void Start () 
    {
        timeStart();
	}
	
    protected override void tapBehavior()
    {
        Destroy(gameObject);
    }
}
