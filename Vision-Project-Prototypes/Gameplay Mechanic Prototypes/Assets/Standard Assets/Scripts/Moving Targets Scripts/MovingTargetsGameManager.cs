using UnityEngine;
using System.Collections;

/// <summary>
/// Game manager for moving targets game
/// </summary>
public class MovingTargetsGameManager : Mechanic 
{
	private Background background;

    protected float backgroundOpacity;
    // The speed that the background spins
    protected float backgroundSpeed;
    // The number of targets needed to win

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
	public override void Start () 
	{
        base.Start();
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
		
		targetMan = GetComponent<TargetManager>();
		background = GameObject.Find("Background").GetComponent<Background>();
		background.Speed = backgroundSpeed;
		background.Opacity = backgroundOpacity;

		CurrentState = MovingTargetsState.PLAY;
        mechanicType = "Moving Targets";
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

	protected override void playBehavior()
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

		if (gameTime.lap() >= targetSpawnInterval && activeTargets < maxTargetsOnScreen)
		{
			spawnTarget();
		}
	}

    protected override void winBehavior()
    {
        base.winBehavior();
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

        Debug.Log(string.Format("X:{0}, Y:{1}", x, y));
        // Generate random speed
        float speed = Random.Range(minTargetSpeed, maxTargetSpeed);

        // Position and set target speed
        target.transform.position = new Vector2(x, y);
        target.GetComponent<OrbitMove>().SPEEDFACTOR = speed;
        target.TimeOut = targetTimeout;

        // Add target to target manager
        targetMan.addTarget(target);

        // Restart the spawn timer
        gameTime.start();
    }

    public override MechanicData packData()
    {
        MechanicData data = base.packData();
        data.backgroundOpacity = backgroundOpacity;
        data.backgroundSpeed = backgroundSpeed;
        return data;
    }
}
