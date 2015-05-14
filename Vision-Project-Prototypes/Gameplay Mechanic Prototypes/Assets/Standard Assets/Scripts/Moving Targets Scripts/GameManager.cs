using UnityEngine;
using System.Collections;

/// <summary>
/// Game manager for moving targets game
/// </summary>
public class GameManager : MonoBehaviour 
{
    private string gameManID;
	// The maximum number of targets that can be on the
	// screen at once.
	private int maxTargetsOnScreen;
	// The range that the targets' speed can be
	private float minTargetSpeed;
	private float maxTargetSpeed;
	// The time between each new target being spawned
	private float targetSpawnInterval;

	// The number of targets needed to win
	private int targetsToWin;

	private StopWatch timer;

	private TargetManager targetMan;

    private GameSession gameSession;
    private Database dbConnection;

	private Background background;

	// The current state of the game
	private state currentState;

	public enum state
	{
		PLAY,
		PAUSE,
		WIN,
		LOSE
	}
	
	// Use this for initialization
	void Start () 
	{
		maxTargetsOnScreen = MovingTargetsSettings.maxTargetsOnScreen;
		minTargetSpeed = MovingTargetsSettings.minTargetSpeed;
		maxTargetSpeed = MovingTargetsSettings.maxTargetSpeed;
		targetSpawnInterval = MovingTargetsSettings.targetSpawnInterval;
		targetsToWin = MovingTargetsSettings.targetsToWin;

		timer = new StopWatch();
		targetMan = GetComponent<TargetManager>();
        gameSession = GameObject.Find("GameSession").GetComponent<GameSession>();
        dbConnection = GameObject.Find("Database").GetComponent<Database>();
		background = GameObject.Find("Background").GetComponent<Background>();

		currentState = state.PLAY;
        gameManID = System.Guid.NewGuid().ToString();
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (currentState)
		{ 
			case state.PLAY:
				playBehavior();
				background.spin();
				break;

			case state.WIN:
				winBehavior();
				break;
		}
	}

	private void playBehavior()
	{
		ArrayList targets = targetMan.getTargets();
		int activeTargets = 0;

		if (targetMan.getHits() >= targetsToWin)
		{
			currentState = state.WIN;
		}

		foreach (Target t in targets)
		{
			if (t.isActiveAndEnabled)
			{
				activeTargets++;
			}
		}

		if (timer.lap() >= targetSpawnInterval && activeTargets < maxTargetsOnScreen)
		{
			spawnTarget();
		}
	}

	private void winBehavior()
	{
        GameInstance inst = gameSession.packData();
        if (!dbConnection.insert(inst))
        {
            Debug.Log("game instance insert failed");
        }

        MovingTargetsManData man = packData();
        if (!dbConnection.insert(man))
        {
            Debug.Log("game manager insert failed");
        }

        TargetManData targetManData = targetMan.packData(gameManID);
        if (!dbConnection.insert(targetManData))
        {
            Debug.Log("target Manager insert failed");
        }

        IEnumerable targets = targetMan.packTargetData();
        if (!dbConnection.insertAll(targets))
        {
            Debug.Log("targets insert failed");
        }
        Application.Quit();
	}

	public void spawnTarget()
	{
		// Instantiate the target prefab
		Target target = Instantiate(Resources.Load("Moving Targets Prefabs/Owl", typeof(Target))) as Target;
		
		// Generate random x position
		float worldHeight = Camera.main.orthographicSize - target.transform.lossyScale.y / 2;
		float x = Random.Range(-worldHeight, worldHeight);
		
		// Generate random y position
		float worldWidth = Mathf.Sqrt(Mathf.Pow(worldHeight, 2) - Mathf.Pow(x, 2));
		float y = Random.Range(-worldWidth, worldWidth);

		// Generate random speed
		float speed = Random.Range(minTargetSpeed, maxTargetSpeed);

		// Position and set target speed
		target.transform.position = new Vector2(x, y);
		target.GetComponent<OrbitMove>().SPEEDFACTOR = speed;

		// Add target to target manager
		targetMan.addTarget(target);
		
		// Restart the spawn timer
		timer.start();
	}

	public state getState()
	{
		return currentState;
	}
    public MovingTargetsManData packData()
    {
        MovingTargetsManData data = new MovingTargetsManData();

        data.gameManID = gameManID;
        data.gameInstanceID = gameSession.getID();

        // load current difficulty settings
        data.maxOnScreen = MovingTargetsSettings.maxTargetsOnScreen;
        data.targetScale = MovingTargetsSettings.targetScale;
        data.targetOpacity = MovingTargetsSettings.targetOpacity;
        data.minTargetSpeed = MovingTargetsSettings.minTargetSpeed;
        data.maxTargetSpeed = MovingTargetsSettings.maxTargetsOnScreen;
        data.targetTimeout = MovingTargetsSettings.targetTimeout;
        data.targetSpawnInterval = MovingTargetsSettings.targetSpawnInterval;
        data.backgroundOpacity = MovingTargetsSettings.backgroundOpacity;
        data.backgroundSpeed = MovingTargetsSettings.backgroundSpeed;
        data.targetsToWin = MovingTargetsSettings.targetsToWin;
        return data;
    }
}
