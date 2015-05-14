using UnityEngine;
using System.Collections;

public class SyncButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(touch.position);
            Vector2 touchPos = new Vector2(worldPoint.x, worldPoint.y);
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos)
                    && touch.phase == TouchPhase.Began)
            {

                GameObject.Find("Database").GetComponent<Database>().syncData();

            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousepos = new Vector2(wp.x, wp.y);
            if (Input.GetMouseButtonDown(0)
                    && GetComponent<Collider2D>() == Physics2D.OverlapPoint(mousepos))
            {
                GameObject.Find("Database").GetComponent<Database>().syncData();
            }

        }
        
    }
	
}
