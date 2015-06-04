using UnityEngine;
using System.Collections;

/// <summary>
/// A subclass of Target that defines timeout and tap behavior.
/// </summary>

public class MovingTarget : Target 
{
    public override void Start()
	{
		base.Start();
	}

	// Update is called once per frame
	public override void Update () 
	{
		if (timedOut())
		{
			gameObject.SetActive(false);
		}
	}

	protected override void tapBehavior()
	{
        gameObject.SetActive(false);
	}

    /// <summary>
    /// Checks if the touch is overlapping with the target. Sets data if 
    /// </summary>
    /// <param name="tap">The touch object to be checked against the target</param>
    /// <returns>returns true if the target was tapped/touched</returns>
    public override bool checkTouch(Touch tap)
    {
        return base.checkTouch(tap);
    }
}
