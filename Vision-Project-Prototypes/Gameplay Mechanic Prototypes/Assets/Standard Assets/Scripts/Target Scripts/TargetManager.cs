using UnityEngine;
using System.Collections;

// Target Manager Data
// Total Targets
// Total Hits
// Total Misses
// Near Misses
// Target Manager ID
// GameSession ID

/// <summary>
/// The Target manager controls a group of Targets, most likely all targets in a specific level
/// The class relies on a Game manager type class to provide it with tagets in order to keep coupling low
/// </summary>
public class TargetManager : MonoBehaviour 
{
    private int hits, misses;
    public Movement moveType;
	private ArrayList targets;
	
	// Use this for initialization
	void Start () 
	{
        hits = 0;
        misses = 0;
		targets = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () 
    {
        Touch[] taps = Input.touches;
    }

    /// <summary>
    /// Adds a target to the list of managed targets.
    /// </summary>
    /// <param name="t">The fully prepared target to be added</param>
	public void addTarget(Target t)
	{
        if (t != null)
        {
            targets.Add(t);
        }
	}

    /// <summary>
    /// Retrieve the average time between target creation and tap
    /// </summary>
    /// <returns>The average hit time for all targets</returns>
	public float getAverage()
	{
		float average = 0f;
		
		foreach (Target t in targets)
		{
			average += t.lapTime;
		}

		return average / targets.Count;
	}

    /// <summary>
    /// Retrieve an ArrayList of all targets
    /// </summary>
    /// <returns></returns>
	public ArrayList getTargets()
	{
		return targets;
	}

    /// <summary>
    /// The total number of targets managed by the TargetManager
    /// </summary>
    /// <returns></returns>
	public int getNumberOfTargets()
	{
		return targets.Count;
	}
}
