using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Paddle : MonoBehaviour {

	

    private TargetJoint2D m_targetJoint;
    private GameController m_gameController;
    private float m_deltaTime;
	private bool m_cannonAttached;


	void Start () 
    {   
        m_deltaTime = 0.0f;
        m_targetJoint = gameObject.GetComponent<TargetJoint2D> ();
        GameObject gc_obj = GameObject.FindWithTag("GameController");
        m_gameController = gc_obj.GetComponent<GameController> ();
		m_cannonAttached = false;
        SetPolyCollider();

	}
    
    void SetPolyCollider()
    {
        float start_angle = 60.0f;
        float end_angle = 120.0f;
        int steps = 8;
        float pas = (end_angle - start_angle) / steps;
        float angle;
        Vector2 []poly = new Vector2[steps+1];
        for (int i = 0; i < steps+1; i++)
        {
            poly[i] = new Vector2();
            angle = Mathf.PI / 180.0f * (start_angle + i * pas);
            poly[i].x = 0.5f* Mathf.Cos(angle);
            poly[i].y = 0.5f * Mathf.Sin(angle)-0.45f;                      
        }
        PolygonCollider2D col = GetComponent<PolygonCollider2D>();
        col.points = poly;
    }

    public void PaddleMove(Vector2 to)
    {
        m_targetJoint.target = to;
    }
        

/*
	void FixedUpdate () 
    {        
       
		if (m_cannonAttached == true) 
		{
			m_deltaTime += Time.deltaTime;
			if (m_deltaTime > 1.0)
			{
				//m_gameController.AddBullet();
				//Debug.Log("Fire bullet");
				m_deltaTime = 0.0f;
			}
		}
	}
*/
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Powerup") 
        {    
            /*
			Powerup pup_obj = col.gameObject.GetComponent<Powerup> ();
			if (pup_obj.m_powerupType == Powerup.PowerupType.powerup_cannon) 
			{
				m_cannonAttached = true;
			}
            //m_gameController.RemovePowerup (col.gameObject);
            col.gameObject.SetActive(false);
            */
        }
    }
}

