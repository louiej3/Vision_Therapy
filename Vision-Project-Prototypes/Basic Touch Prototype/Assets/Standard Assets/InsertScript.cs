using UnityEngine;
using System.Collections;
using System;

public class InsertScript : MonoBehaviour {

    Database db;
    int streetNum;

	// Use this for initialization
    void Start()
    {
        db = GameObject.Find("Data").GetComponent<Database>();
        streetNum = 1;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector2 touchPos = new Vector2(wp.x, wp.y);
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos) &&Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Address addr = new Address();
                addr.street = streetNum + " First Ave";
                ++streetNum;
                addr.city = "Marysville";
                addr.state = "WA";
                addr.zip = 98270;
                db.insertAddress(addr);
            }
        }
    }
}
