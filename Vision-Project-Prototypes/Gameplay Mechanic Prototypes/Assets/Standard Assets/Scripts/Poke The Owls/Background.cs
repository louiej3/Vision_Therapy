using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour 
{

	private float speed;
	private float opacity;

	// Use this for initialization
	void Start () 
	{
		speed = GameObject.Find("GameManager").GetComponent<DifficultySettings>().spiralSpeed;
		
		opacity = GameObject.Find("GameManager").GetComponent<DifficultySettings>().spiralOpacity;
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, opacity);
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate(Vector3.forward, speed * Time.deltaTime);
	}
}
