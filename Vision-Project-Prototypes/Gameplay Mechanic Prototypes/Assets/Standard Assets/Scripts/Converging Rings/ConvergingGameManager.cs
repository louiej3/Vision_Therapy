using UnityEngine;
using System.Collections;

public class ConvergingGameManager : MonoBehaviour 
{
    // Database Variables
    private const string mechanicType = "Converging Rings";
    private GameSession gameSession;
    private Database dbConnection;
    private string gameManID;
	
	private StopWatch timer;

	private ConObjectManager conMan;
	
	// The current state of the game
	private ConvergeState currentState;

	public ConvergingObjects convergePrefab;

	// The maximum amount of converging objects on the screen
	// at one time
	private int maxConvergeOnScreen;
	// The time between each converging object beign spawned
	private float convergeSpawnInterval;
	// The minimum time it takes for the boomerangs to intersect
	private float minConvergeTime;
	// The maximum time it takes for the boomerangs to intersect
	private float maxConvergeTime;
	// The transparency of the center of the converging object
	private float centerOpacity;
	// The time before a converging object times out
	private float convergeTimeOut;
	// The scale of the center object, center is a square
	private float centerScale;
	// The transparency of the boomerangs
	private float boomerangOpacity;
	// The scale of the boomerangs, boomerangs are square
	private float boomerangScale;
	// The number of boomerangs for each converging object
	private int numberOfBoomerangs;
	// The amount of converging objects that need to be tapped
	// before the user wins
	private int convergesToWin;
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
		maxConvergeOnScreen = ConvergingSettings.maxConvergeOnScreen;
		convergeSpawnInterval = ConvergingSettings.convergeSpawnInterval;
		minConvergeTime = ConvergingSettings.minConvergeTime;
		maxConvergeTime = ConvergingSettings.maxConvergeTime;
		centerOpacity = ConvergingSettings.centerOpacity;
		convergeTimeOut = ConvergingSettings.convergeTimeOut;
		centerScale = ConvergingSettings.centerScale;
		boomerangOpacity = ConvergingSettings.boomerangOpacity;
		boomerangScale = ConvergingSettings.boomerangScale;
		numberOfBoomerangs = ConvergingSettings.numberOfBoomerangs;
		convergesToWin = ConvergingSettings.convergesToWin;
		marginOfError = ConvergingSettings.marginOfError;

		timer = new StopWatch();
		
		conMan = GetComponent<ConObjectManager>();
		conMan.MarginOfError = marginOfError;

		currentState = ConvergeState.PLAY;

        gameSession = GameObject.Find("GameSession").GetComponent<GameSession>();
        dbConnection = GameObject.Find("Database").GetComponent<Database>();
        gameManID = System.Guid.NewGuid().ToString();
		
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

	private void playBehavior()
	{
		ArrayList con = conMan.Converging;
		
		int activeConverges = 0;

		if (conMan.SuccessfulHits >= convergesToWin)
		{
			currentState = ConvergeState.WIN;
		}

		foreach (ConvergingObjects co in con)
		{
			if (co.isActiveAndEnabled)
			{
				activeConverges++;
			}
		}
		
		if (timer.lap() >= convergeSpawnInterval 
			&& activeConverges < maxConvergeOnScreen)
		{
			spawnConverge();
		}
	}

	private void winBehavior()
	{
        GameInstance inst = gameSession.packData();
        Debug.Log(inst.generateInsert());
        if (!dbConnection.insert(inst))
        {
            Debug.Log("game instance insert failed");
        }

        MechanicData man = packData();
        if (!dbConnection.insert(man))
        {
            Debug.Log("game manager insert failed");
        }

        ManagerData targetManData = conMan.packData(gameManID);
        if (!dbConnection.insert(targetManData))
        {
            Debug.Log("target Manager insert failed");
        }

        IEnumerable targets = conMan.packTargetData();
        if (!dbConnection.insertAll(targets))
        {
            Debug.Log("targets insert failed");
        }
        dbConnection.syncData();
        Application.Quit();
	}

	private void spawnConverge()
	{
		ConvergingObjects co = Instantiate(convergePrefab) as ConvergingObjects;
		co.TimeOut = convergeTimeOut;
		co.Scale = centerScale;
		co.Opacity = centerOpacity;

		float convergeTime = Random.Range(minConvergeTime, maxConvergeTime);

		float worldHeight = Camera.main.orthographicSize - co.Scale / 2;
		float x = Random.Range(-worldHeight, worldHeight);

		float worldWidth = (Camera.main.orthographicSize / Camera.main.aspect) - co.Scale / 2;
		float y = Random.Range(-worldWidth, worldWidth);

		co.set(numberOfBoomerangs, new Vector2(x, y), convergeTime, boomerangScale, boomerangOpacity);

		conMan.addConverge(co);

		timer.start();
	}

    public MechanicData packData()
    {
        MechanicData data = new MechanicData();

        data.gameManID = gameManID;
        data.gameInstanceID = gameSession.getID();

        // load current difficulty settings
        data.maxOnScreen = maxConvergeOnScreen;
        data.targetScale = centerScale;
        data.targetOpacity = centerOpacity;
        data.minTargetSpeed = minConvergeTime;
        data.maxTargetSpeed = maxConvergeTime;
        data.targetTimeout = convergeTimeOut;
        data.targetSpawnInterval = convergeSpawnInterval;
        data.targetsToWin = convergesToWin;
        data.mechanicType = mechanicType;
        data.secondaryOpacity = boomerangOpacity;
        data.secondaryScale = boomerangScale;
        return data;
    }
}
