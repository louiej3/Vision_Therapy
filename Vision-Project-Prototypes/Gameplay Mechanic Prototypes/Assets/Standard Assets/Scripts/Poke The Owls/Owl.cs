using UnityEngine;
using System.Collections;

public class Owl : Target 
{

	private float timeout;
	private float scale;
	private float opacity;

	protected override void Start()
	{
		base.Start();
		
		timeout = GameObject.Find("GameManager").GetComponent<DifficultySettings>().owlTimeout;
		
		scale = GameObject.Find("GameManager").GetComponent<DifficultySettings>().owlScale;
		transform.localScale = new Vector2(scale, scale);

		opacity = GameObject.Find("GameManager").GetComponent<DifficultySettings>().owlOpacity;
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, opacity);
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
