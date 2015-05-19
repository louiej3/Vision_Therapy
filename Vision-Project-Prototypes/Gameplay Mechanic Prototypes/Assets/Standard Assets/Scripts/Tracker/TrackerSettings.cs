using UnityEngine;
using System.Collections;
using SQLite4Unity3d;

/// <summary>
/// Difficulty settings for tracker game
/// </summary>
public class TrackerSettings
{

	// The scale of the targets. The targets are squares.
	public static float targetScale = 1f;
	// The transperancy of the targets
	public static float targetOpacity = 1f;
	// The range that the targets' speed can be
	public static float minTargetSpeed = 0.5f;
	public static float maxTargetSpeed = 1.5f;
	// The time before a target disappears
	public static float targetTimeout = 20f;
	// The time between each new target being spawned
	public static float targetSpawnInterval = 0.5f;

	public static int numberOfTrackTargets = 3;
	public static int numberOfDummyTargets = 10;

	// The number of targets needed to win
	public static int targetsToWin = 10;

}
