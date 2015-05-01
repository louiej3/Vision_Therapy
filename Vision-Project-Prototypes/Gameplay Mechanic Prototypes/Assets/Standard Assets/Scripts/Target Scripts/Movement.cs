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
    void Start()
    {
        location = GetComponent<Transform>();
    }
    public virtual void Update()
    {

    }
    public virtual float getVelocity()
    {
        float v = 0f;

        return v;
    }
}//