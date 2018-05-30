using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour 
{    
    public enum BrickType : uint  // your custom enumeration
    {
        brick_none,
        brick_brown, 
        brick_red, 
        brick_turquoise,
        brick_white
    };

    public BrickType m_brickType;
    public int m_brickValue;

	void Start()
	{
		string txt = string.Format ("entity type {0}", m_brickType);
		Debug.Log (txt);
	}    
}


