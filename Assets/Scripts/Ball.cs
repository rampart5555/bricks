using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public Vector2 m_direction;
    public float m_speed;
	public AudioClip m_brickHit;
    private GameController m_gameController;
	void Start () 
    {    
        GameObject gc_obj = GameObject.FindWithTag("GameController");
        m_gameController = gc_obj.GetComponent<GameController> ();
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void SetSpeed(float x, float y)
    {
        m_direction.Set (x, y);
        m_direction.Normalize ();
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = m_direction*m_speed;      
    }

	void OnCollisionEnter2D(Collision2D col)
	{
		//Debug.Log (col.gameObject.name);
		//AudioSource.PlayClipAtPoint (m_brickHit, transform.position);
        if (col.gameObject.tag == "Brick") {
            //collision.gameObject.SendMessage ("ApplyDamage", 10);
            //Debug.Log ("Brick hit");
            Brick br = (Brick)col.gameObject.GetComponent<Brick> ();
            switch (br.m_brickType) {
            case Brick.BrickType.brick_brown:
                    //Debug.Log ("brick_brown");
                break;
            case Brick.BrickType.brick_red:
                    //Debug.Log ("brick_red");
                break;
            case Brick.BrickType.brick_turquoise:
                    //Debug.Log ("brick_turquoise");
                break;
            case Brick.BrickType.brick_white:
                    //Debug.Log ("brick_white");
                break;
            }
            m_gameController.UpdateScore (br.m_brickValue);
            m_gameController.RemoveBrick (col.gameObject);
        } else if (col.gameObject.tag == "Paddle") 
        {    
            GameObject paddle = col.gameObject;
            ContactPoint2D [] contacts=new ContactPoint2D[5];
            Rigidbody2D rb = paddle.GetComponent<Rigidbody2D> ();
            int csz = rb.GetContacts(contacts);
            float dist_x = contacts [0].point.x - paddle.transform.position.x;
            float angle = 90.0f - 60.0f * dist_x;
            Debug.Log ("Contact points: " + angle + " " + dist_x);
        }
	}
}
