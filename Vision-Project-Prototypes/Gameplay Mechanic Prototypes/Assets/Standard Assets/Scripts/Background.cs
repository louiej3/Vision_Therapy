using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour
{

	protected float _opacity;

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
