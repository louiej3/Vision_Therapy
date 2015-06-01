﻿using UnityEngine;
using System.Collections;

public class ConvergingGameManager : Mechanic 
{
	// The current state of the game
	private ConvergeState currentState;

    private ConObjectManager conMan;

	private TextMesh score;
	private GameObject winText;

	private Background background;

	public ConvergingObjects convergePrefab;
	
	private float boomerangOpacity;
	// The scale of the boomerangs, boomerangs are square
	private float boomerangScale;
	// The number of boomerangs for each converging object
	private int numberOfBoomerangs;
	// Multiplies converge time by this value to determine how
	// far off the user can be when they tap the object
	private float marginOfError;
	// The transperancy of the background
	private float backgroundOpacity;

	public enum ConvergeState
	{
		PLAY,
		PAUSE,
		WIN,
		LOSE
	}

	// Use this for initialization
	void Start () 
	{
		base.Start();

		// Load the players setting preference string (all sliders and their difficulty level)
		string playernum = PlayerPrefs.GetInt("PlayerNumber").ToString();
		string playerdiff = PlayerPrefs.GetString("P" + playernum + "DIFF");
		Debug.Log("PDIFF is: " + playernum );
		
		// Unpack string into 2D array for settings
		char[] delimiterChars = { ',' };							// Delimiter characters to look for and removes them
		string[] wordsDiff = playerdiff.Split(delimiterChars);		// Splits the playerspref string into the individial bits (One big array)
		
		int [,] my2DArray;											// Create 2D array to hold the setting for this players game
		my2DArray = new int[int.Parse (wordsDiff [1]),int.Parse (wordsDiff [2])]; // Length of the sizes in the 2nd and 3rd element of the array
		
		// Starts after the first 3 elements check (First 3 are game#, Slider#, and Diff#) & first 2 games' data
		Debug.Log("Game number is: " + wordsDiff [0] );
		int z = 3 + (2 * int.Parse (wordsDiff [1]) * int.Parse (wordsDiff [2]) ); // Times 2 to get to the 3rd part of the string
		int stringLength = wordsDiff.Length;						// Length of the given string of user saved data
		for (int x = 0; x < int.Parse (wordsDiff[1]); x++)			// For each Slider
		{
			for (int y = 0; y < int.Parse (wordsDiff[2]); y++)		// For each difficulty of the slider
			{
				if (z < stringLength)
				{
					// Converts each string element to an int in the 4D array
					my2DArray [x,y] = int.Parse (wordsDiff [z]); ///
					z++;
				}
			}
		}
		
		// Associate each difficulty for each setting
		int CurretDiff = 2;

		// Difficulty settings assignment
		maxTargetsOnScreen = my2DArray[1,CurretDiff];				// Maximum number of rings sets
		minTargetSpeed = (float)my2DArray[2,CurretDiff];			// Minimum ring speed
		maxTargetSpeed = (float)my2DArray[3,CurretDiff];			// Maximum ring speed
		targetScale = ((float)my2DArray[4,CurretDiff])/4;			// Size of targets (scaled)
		boomerangScale = ((float)my2DArray[4,CurretDiff])/4;		// Size of distractors
		targetTimeout = (float)my2DArray[5,CurretDiff];				// Ring time out in seconds
		boomerangOpacity = ((float)my2DArray[6,CurretDiff])/10;		// Clarity of distractors (scaled)
		numberOfBoomerangs = my2DArray[7,CurretDiff];				// Number of distractors
		targetSpawnInterval = (float)my2DArray[8,CurretDiff];		// Ring set spawn interval
		targetsToWin = my2DArray[9,CurretDiff];						// Number of successful hits to win
		targetOpacity = ((float)my2DArray[10,CurretDiff])/10;		// Clarity of the center point (scaled)
		backgroundOpacity = ((float)my2DArray[11,CurretDiff])/10;	// Clarity of the background (scaled)

		marginOfError = ConvergingSettings.marginOfError;  		// Not manually set

		// To check for min max speed, if larger, set min speed to max speed (which is lower)
		if(minTargetSpeed > maxTargetSpeed)
		{ minTargetSpeed = maxTargetSpeed; }

		// Original settings from ConvergingSettings
//		maxTargetsOnScreen = ConvergingSettings.maxConvergeOnScreen;
//		targetSpawnInterval = ConvergingSettings.convergeSpawnInterval;
//		minTargetSpeed = ConvergingSettings.minConvergeTime;
//		maxTargetSpeed = ConvergingSettings.maxConvergeTime;
//		targetOpacity = ConvergingSettings.centerOpacity;
//		targetTimeout = ConvergingSettings.convergeTimeOut;
//		targetScale = ConvergingSettings.centerScale;
//		boomerangOpacity = ConvergingSettings.boomerangOpacity;
//		boomerangScale = ConvergingSettings.boomerangScale;
//		numberOfBoomerangs = ConvergingSettings.numberOfBoomerangs;
//		targetsToWin = ConvergingSettings.convergesToWin;
//		marginOfError = ConvergingSettings.marginOfError;
//		backgroundOpacity = ConvergingSettings.backgroundOpacity;
		
		conMan = GetComponent<ConObjectManager>();
        targetMan = conMan;
		
		conMan.MarginOfError = marginOfError;

		score = GameObject.Find("Score").GetComponent<TextMesh>();
		score.transform.position = new Vector3(0f, Camera.main.orthographicSize
			- score.transform.localScale.y, score.transform.position.z);

		winText = GameObject.Find("WinText");

		currentState = ConvergeState.PLAY;

        gameSession = GameObject.Find("GameSession").GetComponent<GameSession>();
        dbConnection = GameObject.Find("Database").GetComponent<Database>();

		background = GameObject.Find("Background").GetComponent<Background>();
		background.Opacity = backgroundOpacity;

        mechanicType = "Converging Rings";
		
		spawnConverge();

		gameTime.start();
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (currentState)
		{
			case ConvergeState.PLAY:
				playBehavior();
				break;

			case ConvergeState.WIN:
				winBehavior();
				break;
		}
	}

	protected override void playBehavior()
	{
		score.text = conMan.SuccessfulHits + " / " + targetsToWin + " targets hit";
		
		if (conMan.SuccessfulHits >= targetsToWin)
		{
			currentState = ConvergeState.WIN;
		}
		
		if (gameTime.lap() >= targetSpawnInterval 
			&& conMan.NumberOfActiveObjects < maxTargetsOnScreen)
		{
			spawnConverge();
		}
	}

	protected override void winBehavior()
	{
		conMan.disableAllTargets();
		winText.transform.position = Vector2.zero;

		base.winBehavior();
	}

	private void spawnConverge()
	{
		ConvergingObjects co = Instantiate(convergePrefab) as ConvergingObjects;
		co.TimeOut = targetTimeout;
		co.Scale = targetScale;
		co.Opacity = targetOpacity;

		float convergeTime = Random.Range(minTargetSpeed, maxTargetSpeed);

		float worldHeight = Camera.main.orthographicSize - co.Scale / 2;
		float x = Random.Range(-worldHeight, worldHeight);

		float worldWidth = (Camera.main.orthographicSize / Camera.main.aspect) - co.Scale / 2;
		float y = Random.Range(-worldWidth, worldWidth);

		co.set(numberOfBoomerangs, new Vector2(x, y), convergeTime, boomerangScale, boomerangOpacity);

		conMan.addTarget(co);

		gameTime.start();
	}

    public override MechanicData packData()
    {
        MechanicData data = base.packData();

        data.secondaryOpacity = boomerangOpacity;
        data.secondaryScale = boomerangScale;

        return data;
    }
}
