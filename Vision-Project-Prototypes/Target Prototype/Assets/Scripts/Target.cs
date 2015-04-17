using UnityEngine;
using System.Collections;

public abstract class Target : MonoBehaviour 
{
    protected float startTime;
    protected float currentTime;
	
    // Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	public virtual void Update () 
    {
	    if (Input.touchCount > 0)
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
            {
                tapBehavior();
                currentTime = Time.realtimeSinceStartup;
            }
        }

        movement();
	}

    protected abstract void tapBehavior();

    protected virtual void movement() { }

    protected void timeStart()
    {
        startTime = Time.realtimeSinceStartup;
    }

    public float getEndTime()
    {
        return currentTime - startTime;
    }
}
