using UnityEngine;
using System.Collections;

/// <summary>
/// Manages a set of converging objects. Keeps track of when the converging
/// objects were touched and various user performance stats.
/// </summary>
public class ConObjectManager : Manager 
{
	// Multiplies converge time by this value to determine how
	// far off the user can be when they tap the object
	private float _marginOfError;
	
	// Update is called once per frame
	void Update () 
	{
		// An array of all of the user's touches currently on the screen
		Touch[] taps = Input.touches;

		// Move the boomerangs for each converging object
		foreach (ConvergingObjects co in Targets)
		{
			co.converge();
		}

		// If the user is touching the screen, check for collisions
		if (Input.touchCount > 0)
		{
			foreach (Touch tap in taps)
			{
				// Prevents collisions from happening if the user drags their
				// finger around
				if (tap.phase == TouchPhase.Began)
				{
					bool hit = false;
					
					// Check every converging object to see if they were touched
					foreach (ConvergingObjects co in Targets)
					{
						if (co.checkTouch(tap))
						{
							// Check if the touch was within the margin of error
							if (co.TapPrecision <= co.ConvergeTime * _marginOfError)
							{
                                Hits++;
								co.Success = true;
							}

							// Increment the total number of times the user touched an
							// object. Touch does not have to be successful.
							// The user touched something
							hit = true;
							break;
						}
					}

					// If we looped through the list and hit was never flipped,
					// the user missed completely.
					if (!hit)
					{
						Misses++;
					}
				}
			}
		}
	}

	/// <summary>
	/// A percentage that determines how inaccurate users can be when they 
	/// tap a converging object. Can be between 0 and 1.
	/// </summary>
	public float MarginOfError
	{
		get
		{
			return _marginOfError;
		}
		set
		{
			if (value >= 0 && value <= 1)
			{
				_marginOfError = value;
			}
			else
			{
				throw new System.Exception("Value must be between 0 and 1");
			}
		}
	}
}
