using UnityEngine;
using System.Collections;

public class Form : MonoBehaviour {

    public bool isNumber = false;
    private TouchScreenKeyboard keyboard;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {

        if (Input.touchCount == 1)
        {

            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector2 touchPos = new Vector2(wp.x, wp.y);
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos) && keyboard == null)
            {
                if (isNumber)
                {
                    keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.NumberPad);
                }
                else
                {
                    keyboard = TouchScreenKeyboard.Open("");
                }
                
            }
            if (keyboard != null)
            {
                TextMesh tm = GetComponent<TextMesh>();
                tm.text = keyboard.text;
            }
            if (keyboard != null && keyboard.done)
            {
                keyboard = null;
            }

        }
    }
}
