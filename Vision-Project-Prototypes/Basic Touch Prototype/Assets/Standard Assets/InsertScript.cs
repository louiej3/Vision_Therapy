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
                string str, st, c, z;
                str = streetNum + "Main St.";
                ++streetNum;
                c = "Bothell";
                st = "WA";
                z = "98033";

                if (str != "" && c != "" && st != "" && z != "")    
                {
                    string addrIn = "zip, street, state, city";
                    string addrData = z + ", \"" + str + "\", \"" + st + "\", \"" + c + "\"";
                    Debug.Log(addrData);
                    string sql = string.Format(Database.INSERT_ROW, "Address", addrIn, addrData);
                    bool success = db.insertFail(sql);
                }
                else
                {
                    Debug.Log("Invalid insert");
                }
            }

        }
    }
}
