using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	// Use this for initialization

	private GameController m_gameController;

	void Start () 
	{
		GameObject gc_obj = GameObject.FindWithTag("GameController");
		m_gameController = gc_obj.GetComponent<GameController> ();	
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if ((col.gameObject.tag == "Bullet") && (gameObject.name == "wall_top")) {
			m_gameController.RemoveBullet (col.gameObject);
		} 
		else if ((col.gameObject.tag == "Ball") && (gameObject.name == "wall_bottom")) 
		{
			m_gameController.RemoveBall (col.gameObject);
		}
	}

}
