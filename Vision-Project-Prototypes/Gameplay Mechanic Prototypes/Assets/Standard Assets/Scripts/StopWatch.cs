using System;
using UnityEngine;

/// <summary>
/// A tool to keep track of time
/// Can be used to wait for a certain amount of time to pass
/// or to measure the amount of time between two events
/// </summary>

public class StopWatch
{
    private float startTime;
    private float endTime;

    /// <summary>
    /// Constructor for the StopWatch
    /// Initializes the variables but does not "start" the stopwatch
    /// </summary>
	public StopWatch()
	{
        startTime = 0f;
        endTime = 0f;
	}
    /// <summary>
    /// Returns the time (in seconds since the game started) that the stopwatch was last started.
    /// </summary>
    /// <returns>Time in seconds since the last start</returns>
    public float getTime()
    {
        return startTime;
    }
    /// <summary>
    /// starts the stopWatch by setting startTime to the current time
    /// </summary>
    public float start()
    {
        startTime = Time.time;
        return startTime;
    }

    /// <summary>
    /// Shows the amount of time that has passed between the current moment (endtime) and the last time start was
    /// called (startTime) in seconds
    /// </summary>
    /// <returns>Difference in seconds between now(endTime) and startTime</returns>
    public float lap()
    {
        endTime = Time.time;
        return endTime - startTime;
    }

    /// <summary>
    /// Shows the amount of time that has passed between the current moment (endtime) and the passed in variable
    /// endTime - oldTime in seconds
    /// </summary>
    /// <param name="oldTime"></param>
    /// Any time point in time before now, measured in seconds since the game started running
    /// <returns>Difference in seconds between now(endTime) and oldTime </returns>
    public float lap(float oldTime)
    {
        endTime = Time.time;
        return endTime - oldTime;
    }
}
