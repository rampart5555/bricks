using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public Vector2 m_speed;
	public AudioClip m_brickHit;

	void Start () 
    {        
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void SetSpeed(float x,float y)
    {
        m_speed.Set (x, y);
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = m_speed;      
    }

	void OnCollisionEnter2D(Collision2D col)
	{
		//Debug.Log (col.gameObject.name);
		//AudioSource.PlayClipAtPoint (m_brickHit, transform.position);
        if (col.gameObject.tag == "Brick") 
        {
            //collision.gameObject.SendMessage ("ApplyDamage", 10);
            Debug.Log ("Brick hit");
            Brick br=(Brick)col.gameObject.GetComponent<Brick>();
            switch (br.m_brickType) 
            {
                case Brick.BrickType.brick_brown:
                    Debug.Log ("brick_brown");
                    break;
                case Brick.BrickType.brick_red:
                    Debug.Log ("brick_red");
                    break;
                case Brick.BrickType.brick_turquoise:
                    Debug.Log ("brick_turquoise");
                    break;
                case Brick.BrickType.brick_white:
                    Debug.Log ("brick_white");
                    break;
            }
        }		
	}
}
