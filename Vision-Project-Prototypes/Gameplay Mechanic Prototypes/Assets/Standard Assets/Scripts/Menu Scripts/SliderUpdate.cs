using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderUpdate : MonoBehaviour {
	
	private UnityEngine.UI.Slider my_slider;		// For the sliders being manipulated
	private UnityEngine.UI.InputField my_field;		// For the sliders input field number // (For Buttons)
	public int changeby = 2;						// For the sliders increase by number // (For Buttons)
	public UnityEngine.UI.Text textMesh;			// For the sliders textmesh print to screen
	public string sliderName;						// For the sliders name to print
	

	void Start()
	{
		// Assignment for each slider, their fields, and their text display name
		my_slider = gameObject.GetComponent<UnityEngine.UI.Slider>();
		my_field = gameObject.GetComponent<UnityEngine.UI.InputField>(); // Was used for input field // (For Buttons)
		textMesh.text = (sliderName + my_slider.value.ToString ());
	}

	void Update()
	{
		string units = "";

		// Add the end units for some sliders
		if (sliderName == "Time-Out: " || sliderName == "Spawn Rate: " || sliderName == "Spawn Interval: "  || sliderName == "Shuffle Time: " )
		{ units = " sec(s)";}
		if (sliderName == "Target Opacity: " || sliderName == "Background Opacity: " || 
		    sliderName == "Center Opacity: " || sliderName == "Ring Opacity: " )
		{ units = "0%";}

		// Prints the finalized updated string
		textMesh.text = (sliderName + my_slider.value.ToString () + units);
	}
	
	public void UpdateValueFromFloat(float value)
	{
		//Debug.Log("float value changed: " + value);
		if (my_slider) { my_slider.value = value; }
		if (my_field) { my_field.text = value.ToString(); } //(For Buttons)
	}

	public void UpdateValueFromString(string value)
	{
		//Debug.Log("string value changed: " + value);
		if (my_slider) { my_slider.value = float.Parse(value); }
		if (my_field) { my_field.text = value; } 			// (For Buttons)
	}

	public void UpdateValueINCREASE() // (For Buttons)
	{
		if (my_slider) { my_slider.value = my_slider.value + changeby; }
		// Convert to a string and add to log
		string newvalue = (my_slider.value.ToString());
		//Debug.Log("float value changed UP to: " + newvalue);
	}

	public void UpdateValueDECREASE() // (For Buttons)
	{
		if (my_slider) { my_slider.value = my_slider.value - changeby; }
		// Convert to a string and add to log
		string newvalue = (my_slider.value.ToString());
		//Debug.Log("float value changed DOWN to: " + newvalue);
	}

}