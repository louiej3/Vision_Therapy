using UnityEngine;
using System.Collections;

public class BalloonTarget : Target 
{
	private TargetManager tm;

	// Use this for initialization
	void Start () 
    {
        setStartTime();
		
		if (tm == null)
		{
			tm = GameObject.Find("TargetManager").GetComponent<TargetManager>();
		}

		tm.addTarget(this);
	}
	
    protected override void tapBehavior()
    {
		setCurrentTime();
		gameObject.SetActive(false);
    }
}
