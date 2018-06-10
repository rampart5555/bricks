using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour {

	// Use this for initialization
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

	}
	
	// Update is called once per frame
	void Update () 
    {        
        if (Input.GetMouseButton(0)) 
        {                        
            //Debug.Log ("Mouse click");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast (ray, out hit)) 
            {                   
                m_targetJoint.target = new Vector2 (hit.point.x, hit.point.y);
            } 
        }	
		if (m_cannonAttached == true) 
		{
			m_deltaTime += Time.deltaTime;
			if (m_deltaTime > 1.0)
			{
				m_gameController.AddBullet();
				//Debug.Log("Fire bullet");
				m_deltaTime = 0.0f;
			}
		}
	}
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Powerup") 
        {    
			Powerup pup_obj = col.gameObject.GetComponent<Powerup> ();
			if (pup_obj.m_powerupType == Powerup.PowerupType.powerup_cannon) 
			{
				m_cannonAttached = true;
			}
            m_gameController.RemovePowerup (col.gameObject);
        }
    }
}

