using UnityEngine;
using System.Collections;
using SQLite4Unity3d;

/// <summary>
/// Difficulty settings for tracker game
/// </summary>
/// 
/// 
public class TrackerSettings
{

	// The scale of the targets. The targets are squares.
	public static float targetScale = 1f;
	// The transperancy of the targets
	public static float targetOpacity = 1f;
	// The range that the targets' speed can be
	public static float minChangeTime = 0.5f;
	public static float maxChangeTime = 1.5f;
	// Number of targets that the user needs to track
	public static int numberOfTrackTargets = 3;
	// Number of dummy targets in the scene
	public static int numberOfDummyTargets = 10;
	// The amount of time that targets are allowed to move around
	public static float shuffleTime = 20f;
	// The time between tracks spawning, dummies spawning, and the
	// game starting
	public static float startUpTime = 3f;
	// The max speed of the targets
	public static float targetSpeed = 1f;
	// The transperancy of the background
	public static float backgroundOpacity = 1f;

}
