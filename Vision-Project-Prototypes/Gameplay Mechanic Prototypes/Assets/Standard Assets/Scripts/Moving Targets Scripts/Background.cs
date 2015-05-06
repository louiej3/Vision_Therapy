using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour 
{

	private float speed;
	private float opacity;

	GameManager gameMan;

	// Use this for initialization
	void Start () 
	{
		gameMan = GameObject.Find("GameManager").GetComponent<GameManager>();

		speed = gameMan.GetComponent<DifficultySettings>().backgroundSpeed;

		opacity = gameMan.GetComponent<DifficultySettings>().backgroundOpacity;
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, opacity);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void spin()
	{
		transform.Rotate(Vector3.forward, speed * Time.deltaTime);
	}
}
