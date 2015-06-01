using UnityEngine;
using System.Collections;

/// <summary>
/// This script controls the various attributes of the objects that
/// move back and forth through the center of a Converging Object
/// </summary>

public class Boomerang : MonoBehaviour 
{
	// How fast this boomerang needs to travel to reach the center on time 
	private float _speed;
	// The farthest distance this boomerang can be from the center
	private float _distance;
	private float _opacity;
	private float _scale;

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

	public float Distance
	{
		get
		{
			return _distance;
		}
		set
		{
			if (_distance >= 0)
			{
				_distance = value;
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
}
