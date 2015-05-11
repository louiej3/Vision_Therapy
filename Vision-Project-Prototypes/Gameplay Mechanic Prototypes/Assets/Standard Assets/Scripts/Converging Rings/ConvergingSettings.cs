using UnityEngine;
using System.Collections;

/// <summary>
/// Settings for converging objects game
/// </summary>
public class ConvergingSettings : MonoBehaviour 
{

	// The maximum amount of converging objects on the screen
	// at one time
	public static int maxConvergeOnScreen = 4;
	// The time between each converging object beign spawned
	public static float convergeSpawnInterval = 5f;
	// The minimum time it takes for the boomerangs to intersect
	public static float minConvergeTime = 1f;
	// The maximum time it takes for the boomerangs to intersect
	public static float maxConvergeTime = 4f;

	// The transparency of the center of the converging object
	public static float centerOpacity = 1f;
	// The time before a converging object times out
	public static float convergeTimeOut = 20f;
	// The scale of the center object, center is a square
	public static float centerScale = 1f;

	// The transparency of the boomerangs
	public static float boomerangOpacity = 1f;
	// The scale of the boomerangs, boomerangs are square
	public static float boomerangScale = 1f;
	// The number of boomerangs for each converging object
	public static int numberOfBoomerangs = 4;

	// The amount of converging objects that need to be tapped
	// before the user wins
	public static int convergesToWin = 10;

}
