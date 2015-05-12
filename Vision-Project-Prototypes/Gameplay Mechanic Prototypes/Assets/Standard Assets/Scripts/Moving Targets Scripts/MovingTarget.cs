using UnityEngine;
using System.Collections;

public class MovingTarget : Target 
{

	MovingTargetsGameManager gameMan;

	protected override void Start()
	{
		base.Start();

		gameMan = GameObject.Find("GameManager").GetComponent<MovingTargetsGameManager>();

		timeOut = MovingTargetsSettings.targetTimeout;

		scale = MovingTargetsSettings.targetScale;
		transform.localScale = new Vector2(scale, scale);

		opacity = MovingTargetsSettings.targetOpacity;
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, opacity);
	}

	// Update is called once per frame
	void Update () 
	{
		if (timedOut() && gameMan.getState() == MovingTargetsGameManager.state.PLAY)
		{
			gameObject.SetActive(false);
		}
	}

	protected override void tapBehavior()
	{
		gameObject.SetActive(false);
	}
}
