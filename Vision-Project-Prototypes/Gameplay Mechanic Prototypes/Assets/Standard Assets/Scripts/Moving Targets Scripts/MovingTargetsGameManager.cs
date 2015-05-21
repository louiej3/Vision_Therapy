using UnityEngine;
using System.Collections;

/// <summary>
/// Game manager for moving targets game
/// </summary>
public class MovingTargetsGameManager : MonoBehaviour 
{
    private string gameManID;
	// The maximum number of targets that can be on the
	// screen at once.
	private int maxTargetsOnScreen;
	// The scale of the targets. The targets are squares.
	private float targetScale;
	// The transperancy of the targets
	private float targetOpacity;
	// The range that the targets' speed can be
	private float minTargetSpeed;
	private float maxTargetSpeed;
	// The time before a target disappears
	private float targetTimeout;
	// The time between each new target being spawned
	private float targetSpawnInterval;
	// The transperancy of the background
	private float backgroundOpacity;
	// The speed that the background spins
	private float backgroundSpeed;
	// The number of targets needed to win
	private int targetsToWin;

    private const string mechanicType = "Moving Targets";

	private StopWatch timer;

	private TargetManager targetMan;

    private GameSession gameSession;
    private Database dbConnection;

	private Background background;

	public Target targetPrefab;

	// The current state of the game
	public MovingTargetsState CurrentState { get; private set; }

	public enum MovingTargetsState
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
		targetScale = MovingTargetsSettings.targetScale;
		targetOpacity = MovingTargetsSettings.targetOpacity;
		minTargetSpeed = MovingTargetsSettings.minTargetSpeed;
		maxTargetSpeed = MovingTargetsSettings.maxTargetSpeed;
		targetTimeout = MovingTargetsSettings.targetTimeout;
		targetSpawnInterval = MovingTargetsSettings.targetSpawnInterval;
		backgroundOpacity = MovingTargetsSettings.backgroundOpacity;
		backgroundSpeed = MovingTargetsSettings.backgroundSpeed;
		targetsToWin = MovingTargetsSettings.targetsToWin;

		timer = new StopWatch();
		
		targetMan = GetComponent<TargetManager>();
        gameSession = GameObject.Find("GameSession").GetComponent<GameSession>();
        dbConnection = GameObject.Find("Database").GetComponent<Database>();
		background = GameObject.Find("Background").GetComponent<Background>();
		background.Speed = backgroundSpeed;
		background.Opacity = backgroundOpacity;

        gameManID = System.Guid.NewGuid().ToString();
		CurrentState = MovingTargetsState.PLAY;
    }
	
	// Update is called once per frame
	void Update () 
	{
		switch (CurrentState)
		{ 
			case MovingTargetsState.PLAY:
				playBehavior();
				background.spin();
				break;

			case MovingTargetsState.WIN:
				winBehavior();
				break;
		}
	}

	private void playBehavior()
	{
		ArrayList targets = targetMan.Targets;
		int activeTargets = 0;

		if (targetMan.Hits >= targetsToWin)
		{
			CurrentState = MovingTargetsState.WIN;
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

        ManagerData targetManData = targetMan.packData(gameManID);
        if (!dbConnection.insert(targetManData))
        {
            Debug.Log("target Manager insert failed");
        }

        IEnumerable targets = targetMan.packTargetData();
        if (!dbConnection.insertAll(targets))
        {
            Debug.Log("targets insert failed");
        }
        dbConnection.syncData();
        Application.Quit();
	}

    public void spawnTarget()
    {
        // Instantiate the target prefab
        Target target = Instantiate(targetPrefab) as Target;

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
    public MechanicData packData()
    {
        MechanicData data = new MechanicData();

        data.gameManID = gameManID;
        data.gameInstanceID = gameSession.getID();

        // load current difficulty settings
        data.maxOnScreen = maxTargetsOnScreen;
        data.targetScale = targetScale;
        data.targetOpacity = targetOpacity;
        data.minTargetSpeed = minTargetSpeed;
        data.maxTargetSpeed = maxTargetsOnScreen;
        data.targetTimeout = targetTimeout;
        data.targetSpawnInterval = targetSpawnInterval;
        data.backgroundOpacity = backgroundOpacity;
        data.backgroundSpeed = backgroundSpeed;
        data.targetsToWin = targetsToWin;
        data.mechanicType = mechanicType;
        return data;
    }
}
