using UnityEngine;
using System.Collections;

public class ConvergingGameManager : Mechanic 
{
	// The current state of the game
	private ConvergeState currentState;

    private ConObjectManager conMan;

	private TextMesh score;
	private GameObject winText;

	private Background background;

	// A converging object prefab. This object is set in the Unity
	// scene by dragging an existing converging object prefab into
	// the convergePrefab field in the object with this script
	// attached to it.
	public ConvergingObjects convergePrefab;
	
	// The opacity of the boomerangs
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

	// Game states for this mechanic
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
		backgroundOpacity = ConvergingSettings.backgroundOpacity;
		
		conMan = GetComponent<ConObjectManager>();
        targetMan = conMan;
		
		conMan.MarginOfError = marginOfError;

		// Put the score at the top center of the screen
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
		// Displays the user's current score while they are playing
		score.text = conMan.SuccessfulHits + " / " + targetsToWin + " targets hit";
		
		// Reached the number of required targets, go to WIN state
		if (conMan.SuccessfulHits >= targetsToWin)
		{
			currentState = ConvergeState.WIN;
		}
		
		// Spawn a new converging object if the spawn interval is finished
		// and the number of converging objects on the screen does not
		// exceed the maximum amount of converging objects allowed.
		if (gameTime.lap() >= targetSpawnInterval 
			&& conMan.NumberOfActiveObjects < maxTargetsOnScreen)
		{
			spawnConverge();
		}
	}

	protected override void winBehavior()
	{
		// Clear the screen of all targets so extra data is not collected
		conMan.disableAllTargets();
		// Move the win message into place
		winText.transform.position = Vector2.zero;
		
		base.winBehavior();
	}

	private void spawnConverge()
	{
		// Instantiate the converging object prefab of choice and set its
		// values
		ConvergingObjects co = Instantiate(convergePrefab) as ConvergingObjects;
		co.TimeOut = targetTimeout;
		co.Scale = targetScale;
		co.Opacity = targetOpacity;

		// How long it should take the boomerangs to reach the center
		// starting from their farthest point
		float convergeTime = Random.Range(minTargetSpeed, maxTargetSpeed);

		// Make sure the converging object's x position does not leave the world
		float worldHeight = Camera.main.orthographicSize - co.Scale / 2;
		float y = Random.Range(-worldHeight, worldHeight);

		// Make sure the converging object's y position does not leave the world
		float worldWidth = (Camera.main.orthographicSize * Camera.main.aspect) - co.Scale / 2;
		float x = Random.Range(-worldWidth, worldWidth);

		// Set the center of the converging object and its boomerangs
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
