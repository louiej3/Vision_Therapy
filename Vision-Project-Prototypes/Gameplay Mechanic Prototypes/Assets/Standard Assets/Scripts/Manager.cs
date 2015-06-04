using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// The manager base class standardises the managing of data from 
/// the various manager objects. It relies on the subclasses to actually
/// adjust the variables
/// </summary>
 
public class Manager : MonoBehaviour 
{
    private string manID;
    public int Hits { get; protected set; }
    public int Misses { get; protected set; }
    public int UnsuccessfulHits { get; protected set; }
    public int NearMisses { get; protected set; }
    public ArrayList Targets { get; protected set; }

    public Movement moveType;
    public float nearMissThreshold = 1f;

	void Awake()
	{
		Targets = new ArrayList();
	}

    public virtual void Start()
    {
        manID = System.Guid.NewGuid().ToString();
        Hits = 0;
        Misses = 0;
        NearMisses = 0;
        UnsuccessfulHits = 0;
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
	/// Disables all of the targets in the manager.
	/// </summary>
	public void disableAllTargets()
	{
		foreach (Target t in Targets)
		{
			t.gameObject.SetActive(false);
		}
	}

    /// <summary>
    /// Retrieve the average time between target creation and tap
    /// </summary>
    /// <returns>The average hit time for all targets</returns>
	public virtual float AverageLifeTime
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
	public virtual int NumberOfTargets
    {
        get
        {
            return Targets.Count;
        }
    }

    /// <summary>
    /// Retrieve the average accuracy of the user
    /// </summary>
	public virtual float AverageAccuracy
    {
        get
        {
            float average = 0f;

            foreach (Target t in Targets)
            {
                average += t.TapPrecision;
            }

            return average / Targets.Count;
        }
    }

	/// <summary>
	/// Returns the number of currently active objects in the target manager.
	/// </summary>
	public int NumberOfActiveObjects
	{
		get
		{
			int activeTargets = 0;
			foreach (Target t in Targets)
			{
				if (t.isActiveAndEnabled)
				{
					activeTargets++;
				}
			}
			return activeTargets;
		}
	}
	
    public IEnumerable packTargetData()
    {
        if (Targets.Count == 0)
        {
            return null;
        }
        ArrayList data = new ArrayList();
        foreach (Target t in Targets)
        {
            data.Add(t.packData(manID));
        }

        return data;
    }

    public ManagerData packData(string gameManID)
    {
        ManagerData data = new ManagerData();

        data.managerID = manID;
        data.mechanicID = gameManID;
        data.totalTargets = Targets.Count;
        data.hits = Hits;
        data.unsuccessfulHits = UnsuccessfulHits;
        data.misses = Misses;
        data.nearMisses = NearMisses;

        return data;
    }
}