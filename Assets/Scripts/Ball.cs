using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    // Use this for initialization
    public enum BallStatus {
        BallAttached=0,
        BallReleased,
        BallRunning
    };

    public  BallStatus m_status;
    public Vector2 m_direction;
    public float m_speed;
	public AudioClip m_brickHit;

    private LevelEntities m_levelEntities;
    private GameController m_gameController;
    private bool m_directionChanged;
    private ContactPoint2D[] m_contacts;

    void Awake()
    {
        GameObject gc_obj = GameObject.FindWithTag("LevelEntities");
        m_levelEntities = gc_obj.GetComponent<LevelEntities>();
        m_gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        m_directionChanged = false;
        m_contacts = new ContactPoint2D[5];
        m_direction = new Vector2();
        m_status = BallStatus.BallAttached;
    }

	void Start () 
    {    
        
	}

    public void BallRelease()
    {
        if (m_status == BallStatus.BallAttached)
        {
            Debug.LogFormat("Ball.BallRelease {0}", m_status);
            FixedJoint2D joint = GetComponent<FixedJoint2D>();
            if (joint != null)
            {
                joint.breakForce = 0;
            }
            m_status = BallStatus.BallReleased;
            Invoke("BallStart", 0.2f);
        }
    }

    public void BallStart()
    {
        Debug.LogFormat("Ball.BallStart {0}",m_status);
        SetSpeed(0.0f, 1.0f);
        m_status = BallStatus.BallRunning;
    }

    public void SetSpeed(float x, float y)
    {       
        m_direction.Set (x, y);
        m_direction.Normalize ();
        Rigidbody2D rb = GetComponent<Rigidbody2D> ();
        rb.velocity=m_direction*m_speed;
    }

    private void SetSpeed()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D> ();
        rb.velocity = m_direction*m_speed;
    }

	void OnCollisionEnter2D(Collision2D col)
	{
		//Debug.Log (col.gameObject.name);
		//AudioSource.PlayClipAtPoint (m_brickHit, transform.position);
        if (col.gameObject.tag == "Brick")
        {                                   
            col.gameObject.SetActive(false);
            m_levelEntities.RemoveBrick(col.gameObject);
        }
        else if (col.gameObject.tag == "Paddle")
        {               
            GameObject paddle = col.gameObject;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            rb.GetContacts(m_contacts);
            m_direction.Set(m_contacts[0].normal.x, m_contacts[0].normal.y);
            m_directionChanged = true;

        }
        else if (col.gameObject.name == "wall_bottom")
        {
            m_gameController.SetState(GameController.GCState.PADDLE_LOST);
        }
	}

    void OnCollisionExit2D(Collision2D col)
    {
        if (m_directionChanged == true) 
        {
            m_directionChanged = false;
            SetSpeed();
        }
    }
}
