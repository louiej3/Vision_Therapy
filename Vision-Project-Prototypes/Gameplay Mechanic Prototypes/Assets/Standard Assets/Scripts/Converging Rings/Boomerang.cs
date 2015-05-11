using UnityEngine;
using System.Collections;

public class Boomerang : MonoBehaviour 
{

	public float speed = 0f;
	public float distance = 0f;
	public float opacity;
	public float scale;

	void Start()
	{
		opacity = ConvergingSettings.boomerangOpacity;
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, opacity);
		
		scale = ConvergingSettings.boomerangScale;
		transform.localScale = new Vector2(scale, scale);
	}

}
