using UnityEngine;
using System.Collections;

public class ConvergingManager : MonoBehaviour 
{

	private int hits = 0;
	private int misses = 0;
	private ArrayList converging;

	// Use this for initialization
	void Start () 
	{
		converging = new ArrayList();
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

					foreach (ConvergingObjects co in converging)
					{
						co.converge();
						if (co.checkTouch(tap))
						{
							hit = true;
							break;
						}
					}
					if (hit)
					{
						hits++;
					}
				}
			}
		}
	}

	/// <summary>
	/// Adds a coverging object to the list of managed targets.
	/// </summary>
	/// <param name="t">The fully prepared converging object to be added</param>
	public void addTarget(ConvergingObjects co)
	{
		if (co != null)
		{
			converging.Add(co);
		}
	}

	/// <summary>
	/// Retrieve the average time between converging object creation and tap
	/// </summary>
	/// <returns>The average hit time for all converging objects</returns>
	public float getAverage()
	{
		float average = 0f;

		foreach (ConvergingObjects co in converging)
		{
			average += co.lapTime;
		}

		return average / converging.Count;
	}

	/// <summary>
	/// Retrieve an ArrayList of all converging objects
	/// </summary>
	/// <returns></returns>
	public ArrayList getConverging()
	{
		return converging;
	}

	/// <summary>
	/// The total number of converging objects managed by the ConvergingManager
	/// </summary>
	/// <returns></returns>
	public int getNumberOfTargets()
	{
		return converging.Count;
	}
}
