using UnityEngine;
using System.Collections;

public class ConObjectManager : Manager 
{
	// Multiplies converge time by this value to determine how
	// far off the user can be when they tap the object
	private float _marginOfError;

	// Records the number of targets that were tapped within the
	// margin of error. Starts at 0.
    public int SuccessfulHits { get; protected set; }

	// Use this for initialization
	void Start () 
	{
        base.Start();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Touch[] taps = Input.touches;

		foreach (ConvergingObjects co in Targets)
		{
			co.converge();
		}

		if (Input.touchCount > 0)
		{
			foreach (Touch tap in taps)
			{
				if (tap.phase == TouchPhase.Began)
				{
					bool hit = false;
					
					foreach (ConvergingObjects co in Targets)
					{
						if (co.checkTouch(tap))
						{
							if (co.TapPrecision <= co.ConvergeTime * _marginOfError)
							{
								SuccessfulHits++;
								co.Success = true;
							}

							Hits++;
							hit = true;
							break;
						}
					}

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
	/// tap a converging object.
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
