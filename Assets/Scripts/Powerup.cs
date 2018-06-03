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
	public float m_speed;
	private Vector2 m_direction;
    void Start () 
    {    
		Rigidbody2D rb = GetComponent<Rigidbody2D> ();
		m_direction.Set (0.0f, -1.0f);
		rb.velocity = m_direction * m_speed;
    }
}


