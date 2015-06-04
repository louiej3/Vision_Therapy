using System;
using UnityEngine;

/// <summary>
/// Can be attached to any of the vision object types to control the way they move
/// The base class does not have any sort of movement
/// </summary>
public class Movement : MonoBehaviour
{
    /// <summary>
    /// The location of the object being moved.
    /// </summary>
    protected Transform location;
    
	void Awake()
	{
		// Get the transform of the object so we can manipulate its
		// position
		location = GetComponent<Transform>();
	}

	void Start()
    {
        
    }
    
	public virtual void Update()
    {

    }
    
	public virtual float Velocity
    {
		get
		{
			return 0;
		}
    }
}
