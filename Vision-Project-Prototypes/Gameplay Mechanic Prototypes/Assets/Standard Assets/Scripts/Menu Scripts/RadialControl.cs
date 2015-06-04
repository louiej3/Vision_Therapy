using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RadialControl : MonoBehaviour {

	private UnityEngine.UI.Toggle my_toggle;		// For the sliders being manipulated
	private GameObject master_toggle;				// For the Master difficulty slider

	// Use this for initialization
	void Start () {
		my_toggle = gameObject.GetComponent<UnityEngine.UI.Toggle>();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Uncheck() {
		bool master_toggle = GameObject.Find ("MasterToggle").GetComponent<Toggle>().isOn;
		if (my_toggle && master_toggle == false) { my_toggle.isOn = false; }
	}
}
