using UnityEngine;
using System.Collections;

public class ConvergingGameManager : Mechanic 
{
	// The current state of the game
	private ConvergeState currentState;

    private ConObjectManager conMan;

	public ConvergingObjects convergePrefab;
	private float boomerangOpacity;
	// The scale of the boomerangs, boomerangs are square
	private float boomerangScale;
	// The number of boomerangs for each converging object
	private int numberOfBoomerangs;
	// Multiplies converge time by this value to determine how
	// far off the user can be when they tap the object
	private float marginOfError;

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
		maxTargetsOnScreen = ConvergingSettings.maxConvergeOnScreen;
		targetSpawnInterval = ConvergingSettings.convergeSpawnInterval;
		minTargetSpeed = ConvergingSettings.minConvergeTime;
		maxTargetSpeed = ConvergingSettings.maxConvergeTime;
		targetOpacity = ConvergingSettings.centerOpacity;
		targetTimeout = ConvergingSettings.convergeTimeOut;
		targetScale = ConvergingSettings.centerScale;
		boomerangOpacity = ConvergingSettings.boomerangOpacity;
		boomerangScale = ConvergingSettings.boomerangScale;
		numberOfBoomerangs = ConvergingSettings.numberOfBoomerangs;
		targetsToWin = ConvergingSettings.convergesToWin;
		marginOfError = ConvergingSettings.marginOfError;

		gameTime = new StopWatch();
		
		conMan = GetComponent<ConObjectManager>();
        targetMan = conMan;
		conMan.MarginOfError = marginOfError;

		currentState = ConvergeState.PLAY;

        gameSession = GameObject.Find("GameSession").GetComponent<GameSession>();
        dbConnection = GameObject.Find("Database").GetComponent<Database>();

        mechanicType = "Converging Rings";
		
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
		
		int activeConverges = conMan.ActiveObjects;

		if (conMan.SuccessfulHits >= targetsToWin)
		{
			currentState = ConvergeState.WIN;
		}
		
		if (gameTime.lap() >= targetSpawnInterval 
			&& activeConverges < maxTargetsOnScreen)
		{
			spawnConverge();
		}
	}

	protected override void winBehavior()
	{
        base.winBehavior();
        
        Application.Quit();
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

		conMan.addConverge(co);

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
