using UnityEngine;
using System.Collections;

public class SpinMove : Movement 
{

	private float _speed;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		location.Rotate(Vector3.forward, _speed * Time.deltaTime);
	}

	public float Speed
	{
		get
		{
			return _speed;
		}
		set
		{
			if (_speed >= 0)
			{
				_speed = value;
			}
			else
			{
				throw new System.Exception("Value cannot be negative");
			}
		}
	}
}
