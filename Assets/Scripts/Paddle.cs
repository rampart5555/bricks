﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour {

	// Use this for initialization
    private TargetJoint2D m_targetJoint;
    private GameController m_gameController;

	void Start () 
    {         
        m_targetJoint = gameObject.GetComponent<TargetJoint2D> ();
        GameObject gc_obj = GameObject.FindWithTag("GameController");
        m_gameController = gc_obj.GetComponent<GameController> ();
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
	}
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Powerup") 
        {                         
            m_gameController.RemovePowerup (col.gameObject);
        }
    }
}

