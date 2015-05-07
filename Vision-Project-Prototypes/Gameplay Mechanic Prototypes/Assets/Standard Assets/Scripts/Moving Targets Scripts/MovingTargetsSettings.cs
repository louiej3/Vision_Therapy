using UnityEngine;
using System.Collections;
using SQLite4Unity3d;

/// <summary>
/// Difficulty settings for moving targets game
/// </summary>
public class MovingTargetsSettings
{
	// The maximum number of targets that can be on the
	// screen at once.
	public static int maxTargetsOnScreen = 5;
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
	public static float targetSpawnInterval = 3f;

	// The transperancy of the background
	public static float backgroundOpacity = 1f;
	// The speed that the background spins
	public static float backgroundSpeed = -80f;

	// The number of targets needed to win
	public static int targetsToWin = 20;

}
