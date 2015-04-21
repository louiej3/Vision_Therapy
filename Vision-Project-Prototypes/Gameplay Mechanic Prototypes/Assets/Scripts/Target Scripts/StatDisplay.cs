using UnityEngine;
using System.Collections;

public class StatDisplay : MonoBehaviour 
{
	private TextMesh textMesh;
	private TargetManager tm;
	
	// Use this for initialization
	void Start () 
	{
		textMesh = GetComponent<TextMesh>();

		if (tm == null)
		{
			tm = GameObject.Find("TargetManager").GetComponent<TargetManager>();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		textMesh.text = "Avg Time = " + tm.getAverage() 
			+ "\nTargetManager Size = " + tm.getNumberOfTargets();
	}
}
