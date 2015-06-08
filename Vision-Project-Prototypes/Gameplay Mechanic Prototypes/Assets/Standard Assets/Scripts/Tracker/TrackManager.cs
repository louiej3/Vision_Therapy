using UnityEngine;
using System.Collections;

/// <summary>
/// The Track manager controls a group of Targets, most likely all targets in a specific level
/// The class relies on a Game manager type class to provide it with tagets in order to keep coupling low
/// </summary>

public class TrackManager : Manager 
{	
	// Update is called once per frame
	void Update()
	{
		Touch[] taps = Input.touches;
		
		// Touches are only evaluated if the current state of the game is
		// PLAY and there is at least 1 touch
		if (TrackerGameManager.CurrentState == TrackerGameManager.TrackerState.PLAY 
			&& Input.touchCount > 0)
		{
			foreach (Touch tap in taps)
			{
				if (tap.phase == TouchPhase.Began)
				{
					// If this is changed to true, something was hit
					// If this stays false then nothing was hit
					bool hit = false;

					// Check each target to see if it was touched
					foreach (Target t in Targets)
					{
						// This target was touched
						if (!t.IsTapped && t.checkTouch(tap))
						{
							// The user touched the correct target
							if (t.tag == "Track")
							{
								Hits++;
							}
							// The user touched the incorrect target
							else
							{
								UnsuccessfulHits++;
							}

							break;
						}
						else
						{
							// The user missed a target slightly
							if (t.checkNearMiss(tap, nearMissThreshold))
							{
								++NearMisses;
							}
						}
					}
					// Nothing was hit, increment misses
					if (!hit)
					{
						Misses++;
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

	/// <summary>
	/// Stops all targets from moving.
	/// </summary>
	public void freezeTargets()
	{
		foreach (Target t in Targets)
		{
			t.gameObject.GetComponent<RandomStraightMove>().enabled = false;
			t.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			t.GetComponent<Rigidbody2D>().fixedAngle = true;	
		}
	}

	/// <summary>
	/// Allows previously frozen targets to begin moving again.
	/// </summary>
	public void unfreezeTargets()
	{
		foreach (Target t in Targets)
		{
			t.gameObject.GetComponent<RandomStraightMove>().enabled = true;
			t.GetComponent<Rigidbody2D>().fixedAngle = false;
		}
	}
}
