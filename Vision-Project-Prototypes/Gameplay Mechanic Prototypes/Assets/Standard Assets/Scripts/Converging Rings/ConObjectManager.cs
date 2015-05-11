using UnityEngine;
using System.Collections;

public class ConObjectManager : MonoBehaviour 
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

		foreach (ConvergingObjects co in converging)
		{
			co.converge();
		}

		if (Input.touchCount > 0)
		{
			foreach (Touch tap in taps)
			{
				if (tap.phase == TouchPhase.Began)
				{
					bool hit = false;

					foreach (ConvergingObjects co in converging)
					{
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
	/// <param name="co">The fully prepared converging object to be added</param>
	public void addConverge(ConvergingObjects co)
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
			average += co.getLapTime();
		}

		return average / converging.Count;
	}

	/// <summary>
	/// Retrieve the average accuracy of the user
	/// </summary>
	/// <returns>The average margin of error for the converging objects</returns>
	public float getAverageAccuracy()
	{
		float average = 0f;

		foreach (ConvergingObjects co in converging)
		{
			average += co.getAccuracy();
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

	/// <summary>
	/// Returns the total number of converging objects that were hit
	/// </summary>
	/// <returns></returns>
	public int getHits()
	{
		return hits;
	}

	/// <summary>
	/// Returns the total number of taps that did not hit a converging
	/// object
	/// </summary>
	/// <returns></returns>
	public int getMisses()
	{
		return misses;
	}
}
