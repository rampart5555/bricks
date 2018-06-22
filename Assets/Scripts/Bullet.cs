using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
	// Use this for initialization
	private GameController m_gameController;
    public float m_speed;
    private Vector2 m_direction;
	private Rigidbody2D m_rigidBody;
	void Start () 
    {

		GameObject gc_obj = GameObject.FindWithTag("GameController");
		m_gameController = gc_obj.GetComponent<GameController> ();	
		m_rigidBody = GetComponent<Rigidbody2D> ();
		m_direction = new Vector2 (0.0f, 1.0f);
		ResetSpeed ();
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Brick") 
		{
			m_gameController.RemoveBrick (col.gameObject);
			m_gameController.RemoveBullet (gameObject);
		}
	}

	public void ResetSpeed()
	{		
		if (m_rigidBody != null)			
			m_rigidBody.velocity = m_speed * m_direction;
	}
	
}
