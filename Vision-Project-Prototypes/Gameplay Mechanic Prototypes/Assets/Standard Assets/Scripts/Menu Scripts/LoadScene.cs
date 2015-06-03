using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {

	static int thisPlayer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Loads the game/therapy application
	public void LoadMovingTargets ()
	{
		PlayerPrefs.SetInt("PlayerNumber", PlayerInfo.playerNumber);
		Application.LoadLevel("Moving Targets");
	}

	// Loads the game/therapy application
	public void LoadTracker ()
	{
		PlayerPrefs.SetInt("PlayerNumber", PlayerInfo.playerNumber);
		Application.LoadLevel("Tracker");
	}

	// Loads the game/therapy application
	public void LoadRings ()
	{
		PlayerPrefs.SetInt("PlayerNumber", PlayerInfo.playerNumber);
		Application.LoadLevel("Converging Rings");
	}

}
