using UnityEngine;
using System.Collections;
using SQLite4Unity3d;

/// <summary>
/// The Target manager controls a group of Targets, most likely all targets in a specific level
/// The class relies on a Game manager type class to provide it with tagets in order to keep coupling low
/// </summary>
public class TargetManager : MonoBehaviour 
{
	public int Hits { get; private set; }
	public int Misses { get; private set; }
	public int NearMisses { get; private set; }
	public ArrayList Targets { get; private set; }
    public float nearMissThreshold = 5f;
	public Movement moveType;
	
	// Use this for initialization
	void Start () 
	{
		Targets = new ArrayList();
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
                                nearMissAnimation(target);
                                ++NearMisses;
                            }
                        }
                    }
                    if (hit)
                    {
                        Hits++;
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

    /// <summary>
    /// The animation to be played when a near miss happens
    /// can trigger on multiple objects
    /// </summary>
    /// <param name="target">The target that was almost hit.</param>
    public void nearMissAnimation(Target target)
    {

    }
}

// Target Manager Data
// Total Targets
// Total Hits
// Total Misses
// Near Misses
// Target Manager ID
// GameSession ID
public class TargetManData
{
    //[PrimaryKey, AutoIncrement]
    public int targetManID;
    //[NotNull]
    public int GameManID;
    //[NotNull]
    public int totalTargets;
    //[NotNull]
    public int hits;
    //[NotNull]
    public int misses;
    public int nearMisses;

    const string targetTable = "TargetManagers";

    /// <summary>
    /// Generate an SQL insert statement for the Class
    /// This is not needed for inserting into the local database
    /// </summary>
    /// <param name="ManID">The FK id for the Game Manager class</param>
    /// <returns>An SQL statement in the form of a string</returns>
    public string generateInsert(int ManID)
    {
		var insert = "INSERT into ";
        insert += targetTable;
        insert += "(";

        return insert;
    }
}

