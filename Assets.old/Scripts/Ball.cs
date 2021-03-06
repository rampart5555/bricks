﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public Vector2 m_direction;
    public float m_speed;
	public AudioClip m_brickHit;
    private GameController m_gameController;
    private float PI_2,PI_3;
    private bool m_directionChanged;
    private float m_reflectionAngle;
    private ContactPoint2D[] m_contacts;
    void Awake()
    {
        GameObject gc_obj = GameObject.FindWithTag("GameController");
        m_gameController = gc_obj.GetComponent<GameController> ();

        PI_2 = Mathf.PI / 2.0f;
        PI_3 = Mathf.PI / 3.0f;
        m_directionChanged = false;
        m_contacts = new ContactPoint2D[5];
    }

	void Start () 
    {    
        
	}
		
    public void SetSpeed(float x, float y)
    {       
        m_direction.Set (x, y);
        m_direction.Normalize ();
        Rigidbody2D rb = GetComponent<Rigidbody2D> ();
        rb.velocity=m_direction*m_speed;
    }

	void OnCollisionEnter2D(Collision2D col)
	{
		//Debug.Log (col.gameObject.name);
		//AudioSource.PlayClipAtPoint (m_brickHit, transform.position);
        if (col.gameObject.tag == "Brick") 
		{                       
            //m_gameController.RemoveBrick (col.gameObject);
            col.gameObject.SetActive(false);
        } 
		else if (col.gameObject.tag == "Paddle") 
        {               
            GameObject paddle = col.gameObject;
            Rigidbody2D rb = paddle.GetComponent<Rigidbody2D> ();

            rb.GetContacts (m_contacts);
            float dist_x = m_contacts[0].point.x - paddle.transform.position.x;
            m_reflectionAngle = PI_2 + PI_3 * dist_x/0.25f;
            //Debug.LogFormat ("{0},{1}", m_reflectionAngle * 180 / Math.PI,dist_x);
            m_directionChanged = true;

        }
	}

    void OnCollisionExit2D(Collision2D col)
    {
        if (m_directionChanged == true) 
        {
            m_directionChanged = false;
            SetSpeed (-Mathf.Cos (m_reflectionAngle), Mathf.Sin (m_reflectionAngle));
            //Debug.LogFormat ("Collision Exit {0}, {1}", -Mathf.Cos (m_reflectionAngle), Mathf.Sin (m_reflectionAngle));
        }
    }
}
