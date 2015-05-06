using UnityEngine;
using System.Collections;

/// <summary>
/// Difficulty settings for Poke the Owls
/// </summary>
public class DifficultySettings : MonoBehaviour 
{

	// The maximum number of owls that can be on the
	// screen at once.
	public int maxOwlsOnScreen = 5;
	// The scale of the owls. The owls are squares.
	public float owlScale = 1f;
	// The transperancy of the owls
	public float owlOpacity = 1f;
	// The range that the owls' speed can be
	public float minOwlSpeed = 0.5f;
	public float maxOwlSpeed = 1.5f;
	// The time before an owl disappears
	public float owlTimeout = 5f;
	// The time between each new owl being spawned
	public float owlSpawnInterval = 3f;

	// The transperancy of the spiral
	public float spiralOpacity = 1f;
	// The speed that the spiral spins
	public float spiralSpeed = -80f;

	// The number of owls needed to win
	public int owlsToWin = 20;

}
