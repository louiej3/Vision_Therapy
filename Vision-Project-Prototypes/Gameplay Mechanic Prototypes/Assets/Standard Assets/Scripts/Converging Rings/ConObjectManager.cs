using UnityEngine;
using System.Collections;

public class ConObjectManager : MonoBehaviour 
{

	public int Hits { get; private set; }
	public int Misses { get; private set; }
	public int SuccessfulHits { get; private set; }
	public ArrayList Converging { get; private set; }

	// Multiplies converge time by this value to determine how
	// far off the user can be when they tap the object
	private float _marginOfError;

	// Use this for initialization
	void Start () 
	{
		Converging = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Touch[] taps = Input.touches;

		foreach (ConvergingObjects co in Converging)
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
					
					foreach (ConvergingObjects co in Converging)
					{
						if (co.checkTouch(tap))
						{
							if (co.Accuracy <= co.ConvergeTime * _marginOfError)
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
			Converging.Add(co);
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

			foreach (ConvergingObjects co in Converging)
			{
				average += co.LapTime;
			}

			return average / Converging.Count;
		}
	}

	/// <summary>
	/// Retrieve the average accuracy of the user
	/// </summary>
	public float AverageAccuracy
	{
		get
		{
			float average = 0f;

			foreach (ConvergingObjects co in Converging)
			{
				average += co.Accuracy;
			}

			return average / Converging.Count;
		}
	}
	
	/// <summary>
	/// The total number of converging objects managed by the ConvergingManager
	/// </summary>
	public int NumberOfTargets
	{
		get
		{
			return Converging.Count;
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
