using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SQLite4Unity3d;

/// <summary>
/// The class relies on a Game manager type class to provide it with tagets in order to keep coupling low
/// </summary>
public class TargetManager : Manager
{
	// Update is called once per frame
	void Update () 
    {
        // An array containing all of the user's touches that are on the screen
		Touch[] taps = Input.touches;
        
		if (Input.touchCount > 0)
        {
            foreach (Touch tap in taps)
            {
                if (tap.phase == TouchPhase.Began)
                {
                    // If this is true, then something was hit, but if
					// this remains false, nothing was hit
					bool hit = false;

					// Check if touch collided with any objects in Target
                    foreach (Target target in Targets)
                    {
                        // Something was hit
						if (target.checkTouch(tap))
                        {
                            hit = true;
							Hits++;
                            break;
                        } else {
                            // Check if the touch resulted in a near miss
							if (target.checkNearMiss(tap, nearMissThreshold))
                            {
                                ++NearMisses;
                            }
                        }
                    }
					// Nothing was hit, increment Misses
                    if (!hit)
                    {
                        Misses++;
                    }
                }
            }
        }
    }

}
