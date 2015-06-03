using UnityEngine;
using System.Collections;

public class CameraPosition : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// The start splash screen
	public void SplashScreen() 	{ transform.position = new Vector3(0f, 0f, -10f); }

	// User select screen (pick player)
	public void UserSelect() 	{ transform.position = new Vector3(0f, 200f, -10f); }

	// User name entry screen
	public void Admin() 		{ transform.position = new Vector3(400f, 200f, -10f); }

	// User name entry screen
	public void EnterName() 	{ transform.position = new Vector3(800f, 200f, -10f); }

	// Select a game screen
	public void GameSelect() 	{ transform.position = new Vector3(0f, 400f, -10f); }

	//Must be public
	public void PlayGame() 		{ transform.position = new Vector3(-400f, 400f, -10f); }

	//Must be public
	public void PokeTheOwls() 	{ transform.position = new Vector3(400f, 400f, -10f); }

	//Must be public
	public void EDB() 			{ transform.position = new Vector3(800f, 400f, -10f); }

	//Must be public
	public void Rings()			{ transform.position = new Vector3(1200f, 400f, -10f); }

	//Must be public
	public void PTOedit() 		{ transform.position = new Vector3(400f, 600f, -10f); }
	
	//Must be public
	public void EDBedit() 		{ transform.position = new Vector3(800f, 600f, -10f); }
	
	//Must be public
	public void Ringsedit()		{ transform.position = new Vector3(1200f, 600f, -10f); }
}