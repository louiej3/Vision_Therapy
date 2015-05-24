using UnityEngine;
using System.Collections;

/// <summary>
/// The Track manager controls a group of Targets, most likely all targets in a specific level
/// The class relies on a Game manager type class to provide it with tagets in order to keep coupling low
/// </summary>
public class TrackManager : Manager 
{
	public int SuccessfulHits { get; private set; }
	public int UnsuccessfulHits { get; private set; }
	public float NearMissThreshold { get; private set; }
	
	// Use this for initialization
	public override void Start()
	{
		base.Start();
		NearMissThreshold = 5f;
	}

	// Update is called once per frame
	void Update()
	{
		Touch[] taps = Input.touches;
		if (TrackerGameManager.CurrentState == TrackerGameManager.TrackerState.PLAY 
			&& Input.touchCount > 0)
		{
			foreach (Touch tap in taps)
			{
				if (tap.phase == TouchPhase.Began)
				{
					bool hit = false;

					foreach (Target t in Targets)
					{
						if (t.checkTouch(tap))
						{
							hit = true;
							Hits++;

							if (t.tag == "Track")
							{
								SuccessfulHits++;
								Debug.Log("SuccessfulHits = " + SuccessfulHits);
							}
							else
							{
								UnsuccessfulHits++;
								Debug.Log("UnsuccessfulHits = " + UnsuccessfulHits);
							}

							break;
						}
						else
						{
							if (t.checkNearMiss(tap, NearMissThreshold))
							{
								++NearMisses;
							}
						}
					}
					if (!hit)
					{
						Misses++;
						Debug.Log("Misses = " + Misses);
					}
				}
			}
		}
	}

	/// <summary>
	/// Retrieve the average time between target creation and tap
	/// </summary>
	/// <returns>The average hit time for all track targets</returns>
	public override float AverageLifeTime
	{
		get
		{
			float average = 0f;

			foreach (Target t in Targets)
			{
				if (t.tag == "Track")
				{
					average += t.LapTime;
				}
			}

			return average / Targets.Count;
		}
	}

	public void freezeTargets()
	{
		foreach (Target t in Targets)
		{
			t.gameObject.GetComponent<RandomStraightMove>().enabled = false;
			t.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			t.GetComponent<Rigidbody2D>().fixedAngle = true;	
		}
	}

	public void unfreezeTargets()
	{
		foreach (Target t in Targets)
		{
			t.gameObject.GetComponent<RandomStraightMove>().enabled = true;
			t.GetComponent<Rigidbody2D>().fixedAngle = false;
		}
	}
}
