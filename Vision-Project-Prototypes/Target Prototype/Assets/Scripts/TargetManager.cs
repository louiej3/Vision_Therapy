using UnityEngine;
using System.Collections;

public class TargetManager : MonoBehaviour 
{
	private ArrayList targets;
	
	// Use this for initialization
	void Start () 
	{
		targets = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () { }

	public void addTarget(Target t)
	{
		targets.Add(t);
	}

	public float getAverage()
	{
		float average = 0f;
		
		foreach(Target t in targets)
		{
			average += t.getEndTime();
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
