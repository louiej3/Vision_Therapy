using UnityEngine;
using System.Collections;

public class Touch_Stats : MonoBehaviour {

	TextMesh tm;

	// Use this for initialization
	void Start () {
	
		tm = GetComponent<TextMesh> ();

	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.touchSupported) {
			tm.text = "supported " + Input.touchCount;
		} else {
			tm.text = "not supported " + Input.touchCount;
		}

	}
}
