using UnityEngine;
using System.Collections;

/// <summary>
/// Difficulty settings for moving targets game
/// </summary>
public class DifficultySettings : MonoBehaviour 
{

	// The maximum number of targets that can be on the
	// screen at once.
	public int maxTargetsOnScreen = 5;
	// The scale of the targets. The targets are squares.
	public float targetScale = 1f;
	// The transperancy of the targets
	public float targetOpacity = 1f;
	// The range that the targets' speed can be
	public float minTargetSpeed = 0.5f;
	public float maxTargetSpeed = 1.5f;
	// The time before a target disappears
	public float targetTimeout = 20f;
	// The time between each new target being spawned
	public float targetSpawnInterval = 3f;

	// The transperancy of the background
	public float backgroundOpacity = 1f;
	// The speed that the background spins
	public float backgroundSpeed = -80f;

	// The number of targets needed to win
	public int targetsToWin = 20;

}
