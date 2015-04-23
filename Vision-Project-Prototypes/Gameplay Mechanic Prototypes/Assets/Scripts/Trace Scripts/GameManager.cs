using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{

	TraceRoute tr;

	// Use this for initialization
	void Start () 
	{
		tr = GameObject.Find("GameManager").GetComponent<TraceRoute>();
		
		Vector2 p1 = new Vector2(1, 0);
		Vector2 p2 = new Vector2(4, 4);
		tr.createSegment(p1, p2);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
