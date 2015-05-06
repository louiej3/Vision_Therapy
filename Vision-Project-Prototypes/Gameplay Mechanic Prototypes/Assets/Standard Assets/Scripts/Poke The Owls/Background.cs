using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour 
{

	public float speed;

	// Use this for initialization
	void Start () 
	{
		speed = GameObject.Find("GameManager").GetComponent<DifficultySettings>().spiralSpeed;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate(Vector3.forward, speed * Time.deltaTime);
	}
}
