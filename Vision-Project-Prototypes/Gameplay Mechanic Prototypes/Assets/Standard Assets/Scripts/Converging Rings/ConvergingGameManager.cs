using UnityEngine;
using System.Collections;

public class ConvergingGameManager : MonoBehaviour 
{

	ConvergingObjects co;

	// Use this for initialization
	void Start () 
	{
		co = GetComponent<ConvergingObjects>();
		co.set(5, new Vector2(0f, 0f), 1);
	}
	
	// Update is called once per frame
	void Update () 
	{
		co.converge();
	}
}
