using UnityEngine;
using System.Collections;

public class OrbitMove : Movement {

    public float SPEEDFACTOR = 1f;

    protected float curTime;

    /// <summary>
    /// The point that is being orbited around
    /// </summary>
    public Vector2 center = Vector2.zero;
    public bool isClockwise = false;
    float radius;
    float angle;

	// Use this for initialization
	void Start () {
        location = GetComponent<Transform>();
        angle = Vector2.Angle(center, location.position);
        radius = Vector2.Distance(center, location.position);
	}
	
	// Update is called once per frame
	public override void Update () {
        angle += Time.smoothDeltaTime * SPEEDFACTOR * (isClockwise ? -1 : 1);
        float x, y;
        x = center.x + (float)System.Math.Cos(angle) * radius;
        y = center.x + (float)System.Math.Sin(angle) * radius;
        //y = center.y + (float)System.Math.Sin(angle) * radius;
        //if (isClockwise)
        //{
        //    y *= -1;
        //}
        location.position = new Vector2(x, y);
	}

    public override float getVelocity()
    {
        return radius * SPEEDFACTOR;
    }
}
