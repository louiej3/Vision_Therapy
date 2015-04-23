using UnityEngine;
using System.Collections;
using System.Data;

public class SelectScript : MonoBehaviour {

    Database db;
    IDataReader reader;
    string sql;

	// Use this for initialization
	void Start () 
    {
        db = GameObject.Find("Data").GetComponent<Database>();
        reader = null;
        sql = string.Format(Database.SELECT_ALL, "Address");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount == 1)
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector2 touchPos = new Vector2(wp.x, wp.y);
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos) && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (reader == null)
                {
                    Debug.Log("Creating Reader");
                    reader = db.select(sql);
                }
                if (reader.Read())
                {
                    transform.Find("Zip").GetComponent<TextMesh>().text = reader.GetValue(1).ToString();
                    transform.Find("Street").GetComponent<TextMesh>().text = reader.GetString(2);
                    transform.Find("State").GetComponent<TextMesh>().text = reader.GetString(3);
                    transform.Find("City").GetComponent<TextMesh>().text = reader.GetString(4);
                }
                else
                {
                    Debug.Log("Destroying Reader");
                    reader.Dispose();
                    reader = null;
                }
                
            }

        }
	}
}
