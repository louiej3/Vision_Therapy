using UnityEngine;
using System.Collections;

/// <summary>
/// The Target class is a single object who's main purpose
/// is to be tapped quickly. The base class is meant to be tapped once and dissapear
/// The target does not move unless the Movement class is changed
/// </summary>
public class Target : MonoBehaviour 
{
    //protected float startTime = 0f;
    //protected float currentTime = 0f;
    protected StopWatch timer;
    public float lapTime;
    protected bool isTapped;
    public float tapPrecision = 0f;

    void Awake()
    {
        
    }

    // Use this for initialization
	void Start () 
    { 
        timer = new StopWatch();
        timer.start();
        lapTime = 0f;
        isTapped = false;
    }
	
	// Update is called once per frame
	public virtual void Update () 
    {
        // if there is at least one touch object
	    if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Touch[] taps = Input.touches;
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);
            
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos)
                && touch.phase == TouchPhase.Began)
            {
                lapTime = timer.lap();
                isTapped = true;
                tapBehavior();


            }
        }
	}
    /// <summary>
    /// Specifies the way a target reacts upon being tapped. The target 
    /// automatically records the time it took to tap it.
    /// </summary>
	protected virtual void tapBehavior() { }

    public float getLapTime()
    {
        return lapTime;
    }
    public bool isTouching()
    {
        // check to see that there is actually tapping going on
        if (Input.touchCount == 0)
        {
            return false;
        }

        // check each touch to see if it overlaps
        Touch[] taps = Input.touches;
        foreach (Touch tap in taps)
        {

        }
    }
}
