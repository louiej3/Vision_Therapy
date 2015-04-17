using UnityEngine;
using System.Collections;

public class MovingTarget : Target 
{

	// Use this for initialization
	void Start () 
    {
	
	}

    protected override void tapBehavior()
    {
        Destroy(gameObject);
    }

    protected override void movement()
    {
        transform.position += new Vector3(0f, 0.05f, 0f);
    }
}
