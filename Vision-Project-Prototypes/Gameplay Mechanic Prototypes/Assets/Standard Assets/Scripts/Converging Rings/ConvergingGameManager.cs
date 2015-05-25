using UnityEngine;
using System.Collections;

public class ConvergingGameManager : Mechanic 
{
	// The current state of the game
	private ConvergeState currentState;

    private ConObjectManager conMan;

	private TextMesh score;
	private GameObject winText;

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
		base.Start();
		
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
		
		conMan = GetComponent<ConObjectManager>();
        targetMan = conMan;
		
		conMan.MarginOfError = marginOfError;

		score = GameObject.Find("Score").GetComponent<TextMesh>();
		score.transform.position = new Vector2(0f, Camera.main.orthographicSize
			- score.transform.localScale.y);

		winText = GameObject.Find("WinText");

		currentState = ConvergeState.PLAY;

        gameSession = GameObject.Find("GameSession").GetComponent<GameSession>();
        dbConnection = GameObject.Find("Database").GetComponent<Database>();

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
