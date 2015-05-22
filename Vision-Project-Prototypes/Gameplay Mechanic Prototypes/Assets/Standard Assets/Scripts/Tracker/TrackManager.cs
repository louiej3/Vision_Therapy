using UnityEngine;
using System.Collections;

public class TrackManager : MonoBehaviour 
{
	public int SuccessfulHits { get; private set; }
	public int UnsuccessfulHits { get; private set; }
	public int Misses { get; private set; }
	public int NearMisses { get; private set; }
	public ArrayList Targets { get; private set; }
	public float NearMissThreshold { get; private set; }

	void Awake()
	{
		Targets = new ArrayList();
	}

	// Use this for initialization
	void Start()
	{
		NearMissThreshold = 5f;
	}

	// Update is called once per frame
	void Update()
	{
		Touch[] taps = Input.touches;
		if (Input.touchCount > 0)
		{
			foreach (Touch tap in taps)
			{
				if (tap.phase == TouchPhase.Began)
				{
					bool hit = false;

					foreach (Target target in Targets)
					{
						if (target.checkTouch(tap))
						{
							hit = true;
							
							if (target.tag == "Track")
							{
								SuccessfulHits++;
							}
							else
							{
								UnsuccessfulHits++;
							}

							break;
						}
						else
						{
							if (target.checkNearMiss(tap, NearMissThreshold))
							{
								++NearMisses;
							}
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
	/// Adds a target to the list of managed targets.
	/// </summary>
	/// <param name="t">The fully prepared target to be added</param>
	public void addTarget(Target t)
	{
		if (t != null)
		{
			Targets.Add(t);
		}
	}

	/// <summary>
	/// Retrieve the average time between target creation and tap
	/// </summary>
	/// <returns>The average hit time for all targets</returns>
	public float AverageLifeTime
	{
		get
		{
			float average = 0f;

			foreach (Target t in Targets)
			{
				average += t.LapTime;
			}

			return average / Targets.Count;
		}
	}

	/// <summary>
	/// The total number of targets managed by the TargetManager
	/// </summary>
	public int NumberOfTargets
	{
		get
		{
			return Targets.Count;
		}
	}
}
