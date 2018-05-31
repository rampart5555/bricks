using System;
using UnityEngine;

public class Powerup : MonoBehaviour
{    
    public enum PowerupType : uint  // your custom enumeration
    {
        powerup_none=0,
        powerup_balls=5, 
        powerup_fast, 
        powerup_slow, 
        powerup_canon
    };
    public PowerupType m_powerupType;
    void Start () 
    {    

    }
}


