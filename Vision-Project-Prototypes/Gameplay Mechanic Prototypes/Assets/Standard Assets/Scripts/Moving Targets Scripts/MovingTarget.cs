﻿using UnityEngine;
using System.Collections;

public class MovingTarget : Target 
{

	private float timeout;
	private float scale;
	private float opacity;

	GameManager gameMan;

	public override void Start()
	{
		base.Start();

		gameMan = GameObject.Find("GameManager").GetComponent<GameManager>();
		
		timeout = MovingTargetsSettings.targetTimeout;

		scale = MovingTargetsSettings.targetScale;
		transform.localScale = new Vector2(scale, scale);

		opacity = MovingTargetsSettings.targetOpacity;
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, opacity);
	}

	// Update is called once per frame
	public override void Update () 
	{
		if (timer.lap() >= timeout && gameMan.getState() == GameManager.state.PLAY)
		{
			gameObject.SetActive(false);
		}
	}

	protected override void tapBehavior()
	{
		gameObject.SetActive(false);
	}
}
