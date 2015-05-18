using UnityEngine;
using System.Collections;

public class OrbitMove : Movement {

    private float _speedFactor = 1f;

    private float curTime;

    /// <summary>
    /// The point that is being orbited around
    /// </summary>
	public Vector2 Center { get; set; }
	public bool IsClockwise { get; set; }
	public float Radius { get; private set; }
	public float Angle { get; private set; }

	// Use this for initialization
	void Start () {
        location = GetComponent<Transform>();
        Angle = Vector2.Angle(Center, location.position);
        Radius = Vector2.Distance(Center, location.position);
		Center = Vector2.zero;
	}
	
	// Update is called once per frame
	public override void Update () {
        Angle += Time.smoothDeltaTime * _speedFactor * (IsClockwise ? -1 : 1);
        float x, y;
        x = Center.x + (float)System.Math.Cos(Angle) * Radius;
        y = Center.x + (float)System.Math.Sin(Angle) * Radius;
        location.position = new Vector2(x, y);
	}

    public override float Velocity
    {
		get
		{
			return Radius * _speedFactor;
		}
    }

	public float SpeedFactor
	{
		get
		{
			return _speedFactor;
		}
		set
		{
			if (value >= 0)
			{
				_speedFactor = value;
			}
			else
			{
				throw new System.Exception("Value cannot be negative");
			}
		}
	}
}
