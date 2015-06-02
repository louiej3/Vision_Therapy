using UnityEngine;
using System.Collections;

/// <summary>
/// A subclass of ConvergingObjects that defines timeout and
/// tap behavior.
/// </summary>

public class Ring : ConvergingObjects
{
	
	// Update is called once per frame
	void Update ()
	{
		if (timedOut())
		{
			// Deactivate each boomerang
			foreach (Boomerang b in boomerangs)
			{
				b.gameObject.SetActive(false);
			}

			// Deactivate the center
			gameObject.SetActive(false);
		}
	}

	protected override void tapBehavior()
	{
		// Deactivate each boomerang
		foreach (Boomerang b in boomerangs)
		{
			b.gameObject.SetActive(false);
		}

		// Deactivate the center
		gameObject.SetActive(false);
	}
}
