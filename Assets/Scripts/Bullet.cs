using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
	// Use this for initialization
    public float m_speed;
    private Vector2 m_direction;

	void Start () 
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D> ();
        m_direction = new Vector2 (0.0f, 1.0f);
        rb.velocity = m_speed * m_direction;
	}
	
}
