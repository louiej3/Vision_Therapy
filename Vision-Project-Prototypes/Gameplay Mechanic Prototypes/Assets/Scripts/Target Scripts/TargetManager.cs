using UnityEngine;
using System.Collections;

public class TargetManager : MonoBehaviour 
{
    public Movement moveType;
	private ArrayList targets;
	
	// Use this for initialization
	void Start () 
	{
		targets = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () { 
    }

	public void addTarget(Target t)
	{
        if (t != null)
        {
            // create a copy of the prefab
            // set the movement type
            // add it to the arraylist
            targets.Add(t);
        }
	}

	public float getAverage()
	{
		float average = 0f;
		
		foreach (Target t in targets)
		{
			average += t.lapTime;
		}

		return average / targets.Count;
	}

	public ArrayList getTargets()
	{
		return targets;
	}

	public int getNumberOfTargets()
	{
		return targets.Count;
	}
}
