using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Base class for game manager scripts. This class has variables 
/// and functions that most managers will likely need.
/// </summary>
public abstract class Mechanic : MonoBehaviour
{
    protected bool hasSynced;

    protected string mechanicType;
    protected string mechanicID;

    // The maximum number of targets that can be on the
    // screen at once.
    protected int maxTargetsOnScreen;
    // The scale of the targets. The targets are squares.
    protected float targetScale;
    // The transperancy of the targets
    protected float targetOpacity;
    // The range that the targets' speed can be
    protected float minTargetSpeed;
    protected float maxTargetSpeed;
    // The time before a target disappears
    protected float targetTimeout;
    // The time between each new target being spawned
    protected float targetSpawnInterval;
    // The transperancy of the background
    protected int targetsToWin;
	// The transparency of the background
    protected float backgroundOpacity;
    // game time
    protected StopWatch gameTime;

    // don't destroy on load stuff
    protected GameSession gameSession;
    protected Database dbConnection;

    protected Manager targetMan;

    public virtual void Start()
    {
        hasSynced = false;
 
        gameTime = new StopWatch();

        gameSession = GameObject.Find("GameSession").GetComponent<GameSession>();
        dbConnection = GameObject.Find("Database").GetComponent<Database>();

        gameSession.startTime = System.DateTime.Now;
        mechanicID = System.Guid.NewGuid().ToString();

    }

    protected abstract void playBehavior();

    protected virtual void winBehavior()
    {
        // Sync the data if it has not been synced
        if (!hasSynced)
        {
            // Sync the Game Instance data
            GameInstance inst = gameSession.packData();
            if (!dbConnection.insert(inst))
            {
                Debug.Log("game instance insert failed");
            }

            // Sync the Mechanic Data
            MechanicData man = packData();
            if (!dbConnection.insert(man))
            {
                Debug.Log("game manager insert failed");
            }

            // Sync the Manager Data
            ManagerData targetManData = targetMan.packData(mechanicID);
            if (!dbConnection.insert(targetManData))
            {
                Debug.Log("target Manager insert failed");
            }

            // Sync all the target data
            IEnumerable targets = targetMan.packTargetData();
            if (!dbConnection.insertAll(targets))
            {
                Debug.Log("targets insert failed");
            }
            dbConnection.syncData();
            hasSynced = true;
        }
    }

    /// <summary>
    /// Gathers all the data from the mechanic together.
    /// </summary>
    /// <returns>A MechanicData object containing the data to be added to a database</returns>
    public virtual MechanicData packData()
    {
        MechanicData data = new MechanicData();

        data.mechanicID = mechanicID;
        data.gameInstanceID = gameSession.getID();

        // load current difficulty settings
        data.maxOnScreen = maxTargetsOnScreen;
        data.targetScale = targetScale;
        data.targetOpacity = targetOpacity;
        data.minTargetSpeed = minTargetSpeed;
        data.maxTargetSpeed = maxTargetsOnScreen;
        data.targetTimeout = targetTimeout;
        data.targetSpawnInterval = targetSpawnInterval;
        data.targetsToWin = targetsToWin;
        data.mechanicType = mechanicType;
        data.backgroundOpacity = backgroundOpacity;
        return data;
    }
}
