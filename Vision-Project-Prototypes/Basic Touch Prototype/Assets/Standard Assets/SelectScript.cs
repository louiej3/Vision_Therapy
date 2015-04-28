using UnityEngine;
using System.Collections;
using System.Data;

public class SelectScript : MonoBehaviour {

    Database db;
    IEnumerable reader;
    IEnumerator addresses;
    string sql;

	// Use this for initialization
	void Start () 
    {
        db = GameObject.Find("Data").GetComponent<Database>();
        reader = null;
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
                    reader = db.getAddress();
                    addresses = reader.GetEnumerator();
                }
                
                if (addresses.MoveNext())
                {
                    Address addr = (Address)addresses.Current;
                    transform.Find("Zip").GetComponent<TextMesh>().text = addr.zip.ToString();
                    transform.Find("Street").GetComponent<TextMesh>().text = addr.street;
                    transform.Find("State").GetComponent<TextMesh>().text = addr.state;
                    transform.Find("City").GetComponent<TextMesh>().text = addr.city;
                }
                else
                {
                    reader = null;
                }
                
            }

        }
	}
}
