using UnityEngine;
using System.Collections;

public class RestartButton : Target 
{
	protected override void tapBehavior()
    {
        Application.LoadLevel("Target Prototype");
    }
}
