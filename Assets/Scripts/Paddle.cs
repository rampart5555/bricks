using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Paddle : MonoBehaviour {

	

    private TargetJoint2D m_targetJoint;
    private GameController m_gameController;
    private LevelEntities m_levelEntities;
    public Vector3 m_bulletPos;
    private bool m_cannonAttached;

	void Start () 
    {   
        
        m_targetJoint = gameObject.GetComponent<TargetJoint2D> ();
        GameObject gc_obj = GameObject.FindWithTag("GameController");
        m_gameController = gc_obj.GetComponent<GameController> ();
        GameObject le=GameObject.FindWithTag("LevelEntities");
        m_levelEntities=le.GetComponent<LevelEntities>();		
        SetPolyCollider();
        m_cannonAttached = false;
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
        if(m_targetJoint!=null)
            m_targetJoint.target = to;
    }

    public void BallRelease()
    {
        FixedJoint2D pfj = GetComponent<FixedJoint2D>();
        pfj.enabled = false;
    }

    public void BallAttach(Rigidbody2D ball)
    {        
        FixedJoint2D pfj = GetComponent<FixedJoint2D>();
        pfj.enabled = true;
        pfj.connectedBody = ball;
    }
        
    public void CannonAttach()
    {
        if (m_cannonAttached == false)
        {
            m_cannonAttached = true;
            InvokeRepeating("LaunchProjectile", 1.0f, 2.0f);

        }
    }

    public void Reset()
    {
        if (m_cannonAttached == true)
            CancelInvoke("LaunchProjectile");
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        
        if (col.gameObject.tag == "Powerup")
        {    
            Powerup pup_obj = col.gameObject.GetComponent<Powerup> ();
            			
			if (pup_obj.m_powerupType == LevelEntities.PowerupType.POWERUP_CANNON) 
			{
                CannonAttach();
			}
            m_levelEntities.RemovePowerup(col.gameObject);
        }
        else if (col.gameObject.name == "wall_right")
        {
            if (m_gameController.m_doorRightIsOpen == true)
            {
                m_gameController.LevelComplete();
            }
        }
    }

    void LaunchProjectile()
    {
        m_levelEntities.AddBullet(transform.localPosition+m_bulletPos);
    }
}

