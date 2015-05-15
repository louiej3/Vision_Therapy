using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour 
{

	private float _speed;
	private float _opacity;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void spin()
	{
		transform.Rotate(Vector3.forward, _speed * Time.deltaTime);
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
}
