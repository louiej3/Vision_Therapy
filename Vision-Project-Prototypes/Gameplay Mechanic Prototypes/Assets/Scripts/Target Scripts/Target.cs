using UnityEngine;
using System.Collections;

public abstract class Target : MonoBehaviour 
{
    protected float startTime = 0f;
    protected float currentTime = 0f;
	
    // Use this for initialization
	void Start () { }
	
	// Update is called once per frame
	public virtual void Update () 
    {
	    if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);
            
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos)
                && touch.phase == TouchPhase.Ended)
            {
                tapBehavior();
            }
        }

        movement();
	}

	protected virtual void tapBehavior() { }

	protected virtual void movement() { }

    protected void setStartTime()
    {
        startTime = Time.realtimeSinceStartup;
    }

	protected void setCurrentTime()
	{
		currentTime = Time.realtimeSinceStartup;
	}

    public float getEndTime()
    {
		if (currentTime == 0f)
		{
			return 0f;
		} 
		else 
		{
			return currentTime - startTime;
		}	
    }
}
