using UnityEngine;
using System.Collections;

public class Owl : Target 
{

	private float timeout;
	private float scale;

	protected override void Start()
	{
		base.Start();
		timeout = GameObject.Find("GameManager").GetComponent<DifficultySettings>().owlTimeout;
		scale = GameObject.Find("GameManager").GetComponent<DifficultySettings>().owlScale;
		transform.localScale = new Vector2(scale, scale);
	}

	// Update is called once per frame
	void Update () 
	{
		if (timer.lap() >= timeout)
		{
			gameObject.SetActive(false);
		}
	}

	protected override void tapBehavior()
	{
		gameObject.SetActive(false);
	}
}
