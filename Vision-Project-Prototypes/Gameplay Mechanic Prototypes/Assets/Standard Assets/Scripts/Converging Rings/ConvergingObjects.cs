using UnityEngine;
using System.Collections;

public class ConvergingObjects : MonoBehaviour 
{

	private GameObject point;
	public GameObject ring1;
	public GameObject ring2;
	public float time = 5f;
	private float startTime = 0f;
	public float ring1Speed = 0f;
	public float ring2Speed = 0f;
	private TextMesh timeDisplay;
	
	// Use this for initialization
	void Start () 
	{
		point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		point.transform.position = new Vector2(3f, 3f);

		ring1 = Instantiate(Resources.Load("Converging Rings Prefabs/Ring", typeof(GameObject))) as GameObject;
		ring1.transform.position = new Vector2(0f, -3f);

		ring2 = Instantiate(Resources.Load("Converging Rings Prefabs/Ring", typeof(GameObject))) as GameObject;
		ring2.transform.position = new Vector2(-3f, 1f);

		float ring1Distance = Vector2.Distance(ring1.transform.position, point.transform.position);
		float ring2Distance = Vector2.Distance(ring2.transform.position, point.transform.position);

		ring1Speed = ring1Distance / time;
		ring2Speed = ring2Distance / time;

		Vector2 ring1Direction = (Vector2)point.transform.position - (Vector2)ring1.transform.position;
		ring1.transform.LookAt(ring1.transform.position + new Vector3(0, 0, 1), ring1Direction);

		Vector2 ring2Direction = (Vector2)point.transform.position - (Vector2)ring2.transform.position;
		ring2.transform.LookAt(ring2.transform.position + new Vector3(0, 0, 1), ring2Direction);

		timeDisplay = GameObject.Find("TimeDisplay").GetComponent<TextMesh>();

		startTime = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update () 
	{
		ring1.transform.position += ring1.transform.up * ring1Speed * Time.smoothDeltaTime;
		ring2.transform.position += ring2.transform.up * ring2Speed * Time.smoothDeltaTime;

		timeDisplay.text = "Converge in = " + (Time.realtimeSinceStartup - startTime);
	}
}
