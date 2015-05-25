using UnityEngine;
using System.Collections;

public class ConObjectManager : Manager 
{
	// Multiplies converge time by this value to determine how
	// far off the user can be when they tap the object
	private float _marginOfError;

    public int SuccessfulHits { get; protected set; }

	// Use this for initialization
	void Start () 
	{
        base.Start();
        SuccessfulHits = 0;
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
	/// Adds a coverging object to the list of managed targets.
	/// </summary>
	/// <param name="co">The fully prepared converging object to be added</param>
	public void addConverge(ConvergingObjects co)
	{
		if (co != null)
		{
			Targets.Add(co);
		}
	}

	/// <summary>
	/// Retrieve the average time between converging object creation and tap
	/// </summary>
	public float AverageLifeTime
	{
		get
		{
			float average = 0f;

            foreach (ConvergingObjects co in Targets)
			{
				average += co.LapTime;
			}

            return average / Targets.Count;
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
