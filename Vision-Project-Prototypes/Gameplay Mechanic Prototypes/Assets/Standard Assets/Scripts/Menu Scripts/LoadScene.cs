using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {

	//static int thisPlayer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Loads the MOVING TARGETS/Poke the Owls game/therapy application
	public void LoadMovingTargets ()
	{
		// Sets the player number in PlayerInfo for the GameManager to know which players difficulty settings to load
		PlayerPrefs.SetInt("PlayerNumber", PlayerInfo.playerNumber);
		Application.LoadLevel("Moving Targets");
	}

	// Loads the TRACKER game/therapy application
	public void LoadTracker ()
	{
		// Sets the player number in PlayerInfo for the GameManager to know which players difficulty settings to load
		PlayerPrefs.SetInt("PlayerNumber", PlayerInfo.playerNumber);
		Application.LoadLevel("Tracker");
	}

	// Loads the CONVERGING RINGS game/therapy application
	public void LoadRings ()
	{
		// Sets the player number in PlayerInfo for the GameManager to know which players difficulty settings to load
		PlayerPrefs.SetInt("PlayerNumber", PlayerInfo.playerNumber);
		Application.LoadLevel("Converging Rings");
	}

}
