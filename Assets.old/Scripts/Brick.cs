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
    public Powerup.PowerupType m_powerupType;


	void Awake()
	{
		//string txt = string.Format ("entity type {0}", m_brickType);
		//Debug.Log (txt);
        //m_powerupType = Powerup.PowerupType.powerup_none;
    
	}  

    public void SetPowerup(Powerup.PowerupType powerup)
    {
        m_powerupType = powerup;
    }

    public Powerup.PowerupType GetPowerup()
    {
        return m_powerupType;
    }           
}


