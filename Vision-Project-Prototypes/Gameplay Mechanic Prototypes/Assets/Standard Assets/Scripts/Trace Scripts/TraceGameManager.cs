using UnityEngine;
using System.Collections;

public class TraceGameManager : MonoBehaviour 
{

	TraceManager traceMan;

	// Use this for initialization
	void Start () 
	{
		traceMan = GetComponent<TraceManager>();

		GameObject segment1 = Instantiate(Resources.Load("Trace Prefabs/Segment", typeof(GameObject))) as GameObject;
		traceMan.addSegment(segment1, new Vector2(-2, -2), new Vector2(3, 3));
		GameObject segment2 = Instantiate(Resources.Load("Trace Prefabs/Segment", typeof(GameObject))) as GameObject;
		traceMan.addSegment(segment2, new Vector2(3, 3), new Vector2(3, -3));
	}
	
	// Update is called once per frame
	void Update () 
	{
		traceMan.onRoute();
	}
}
