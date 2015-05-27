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

    private float NearMissThreshold;

	// Use this for initialization
    public override void Start()
    {
        base.Start();
		NearMissThreshold = 5f;
    }
	
	// Update is called once per frame
	void Update () 
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
                            break;
                        } else {
                            if (target.checkNearMiss(tap, nearMissThreshold))
                            {
                                ++NearMisses;
                            }
                        }
                    }
                    if (hit)
                    {
                        Hits++;
                    }
                    else
                    {
                        Misses++;
                    }
                }
            }
        }
    }

}
