using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// This class allows the users to interact with game entities that use
/// this script. Targets can be tapped and behavior can be defined for
/// when they are tapped.
/// </summary>

public class Target : MonoBehaviour
{
    // Timer for keeping track of the time alive
    protected StopWatch timeSinceSpawn;
    protected float _timeOut;

    // DATABASE OBJECTS
    // The uid for this object
    protected string objectID;
    protected float _scale;
    protected float _opacity;
    public float LapTime { get; protected set; }
    public bool IsTapped { get; protected set; }
    public float TapPrecision { get; protected set; }

    public virtual void Start()
    {
        // setup the timer
        timeSinceSpawn = new StopWatch();
        timeSinceSpawn.start();

        // setup the ID
        objectID = Guid.NewGuid().ToString();

        IsTapped = false;
        LapTime = -1.0f;
        TapPrecision = -1.0f;
    }

    public virtual void Update()
    {

    }

    /// <summary>
    /// Specifies the way a target reacts upon being tapped. The target 
    /// automatically records the time it took to tap it.
    /// </summary>
    protected virtual void tapBehavior() { }

    /// <summary>
    /// Checks if the touch is overlapping with the target. Sets data if 
    /// </summary>
    /// <param name="tap">The touch object to be checked against the target</param>
    /// <returns>returns true if the target was tapped/touched</returns>
    public virtual bool checkTouch(Touch tap)
    {
        // Gets the location of the user's touch
		Vector3 worldPoint = Camera.main.ScreenToWorldPoint(tap.position);
		// Converts the location to a Vector2
        Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);

		// Checks to see if the user's touch collided with this target's collider
        if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos)
                && tap.phase == TouchPhase.Began)
        {
            // because the unit has been tapped, set the variables
            LapTime = timeSinceSpawn.lap();
            IsTapped = true;

            // determine how close the tap was to the target.
            TapPrecision = Vector2.Distance(touchPos, transform.position);
            // If it is an inherited class, we can call the specific tap behavior
            tapBehavior();

            return true;
        }
        return false;
    }
    /// <summary>
    /// Checks if the target was almost hit, based on its own size and a given threshold
    /// </summary>
    /// <param name="tap">The touch to test against</param>
    /// <param name="threshold">The threshold of what a near miss is</param>
    /// <returns>Returns true if the hit counts as a near miss.</returns>
    public bool checkNearMiss(Touch tap, float threshold)
    {
        // take the given threshold
        float myThreshold = threshold;

        // add the distance from the middle of the object to the edge of the object
        Transform t = GetComponent<Transform>();
        myThreshold += t.localScale.x / 2;

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(tap.position);
        Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);

        float dist = Vector2.Distance(GetComponent<Transform>().position, touchPos);
        return dist < threshold;
    }


    /// <summary>
    /// Returns a true value if this object has timed out, false otherwise
    /// </summary>
    /// <returns></returns>
    public bool timedOut()
    {
        if (timeSinceSpawn.lap() >= TimeOut)
        {
            return true;
        }
        return false;
    }

    public float TimeOut
    {
        get
        {
            return _timeOut;
        }
        set
        {
            if (value >= 0)
            {
                _timeOut = value;
            }
            else
            {
                throw new System.Exception("Value cannot be negative");
            }
        }
    }

    public float Scale
    {
        get
        {
            return _scale;
        }
        set
        {
            if (value >= 0)
            {
                _scale = value;
                transform.localScale = new Vector2(_scale, _scale);
            }
            else
            {
                throw new System.Exception("Value cannot be negative");
            }
        }
    }

    public float Opacity
    {
        get
        {
            return _opacity;
        }
        set
        {
            if (value >= 0)
            {
                _opacity = value;
                GetComponent<SpriteRenderer>().color = new Color(
                    GetComponent<SpriteRenderer>().color.r,
                    GetComponent<SpriteRenderer>().color.g,
                    GetComponent<SpriteRenderer>().color.b,
                    _opacity);
            }
            else
            {
                throw new System.Exception("Value cannot be negative");
            }
        }
    }

    /// <summary>
    /// Packs the data up into a Object data object
    /// </summary>
    /// <param name="manID"></param>
    /// <returns></returns>
    public ObjectData packData(string manID)
    {
        var data = new ObjectData();
        Movement m = GetComponent<Movement>();
        SpriteRenderer r = GetComponent<SpriteRenderer>();

        data.objectID = objectID;
        data.managerID = manID;
        data.timeAlive = LapTime;
        data.wasHit = IsTapped;
        // Check that there is a movement
        if (m != null)
        {
            data.velocity = m.Velocity;
        }
        else
        {
            data.velocity = 0f;
        }

        // Check that there is a sprite renderer
        if (r != null)
        {
            data.opacity = r.color.a;
            data.blue = r.color.b;
            data.red = r.color.r;
            data.green = r.color.g;
        }
        else
        {
            data.opacity = 0f;
            data.blue = 0f;
            data.red = 0f;
            data.green = 0f;
        }
        data.scale = GetComponent<Transform>().localScale.x;

        return data;
    }
}
