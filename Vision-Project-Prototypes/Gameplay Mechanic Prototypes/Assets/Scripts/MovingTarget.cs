using UnityEngine;
using System.Collections;

public class MovingTarget : Target 
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

    protected override void movement()
    {
        transform.position += new Vector3(0f, 0.05f, 0f);
    }
}
