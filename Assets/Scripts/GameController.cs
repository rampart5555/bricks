﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Button m_gameComplete;
    public Button m_paddleLost;
    public GameObject m_levelEnvironmentGO;
    public GameObject m_levelEntitiesGO;
    public GameObject m_gamePortalGO;

    GamePortal m_gamePortal;
    LevelEnvironment m_levelEnvironment;
    LevelEntities  m_levelEntities;

    bool m_levelClear;

	void Start () 
    {
        Debug.Log("GameController.Start");
        m_gamePortal = m_gamePortalGO.GetComponent<GamePortal>();
        m_levelEnvironment = m_levelEnvironmentGO.GetComponent<LevelEnvironment>();
        m_levelEntities = m_levelEntitiesGO.GetComponent<LevelEntities>();
        m_gamePortal.PlayAnimation("game_portal_open");

	}

    public void AnimationStart(string anim_name)
    {
        Debug.LogFormat("GameController.AnimationStart {0}",anim_name);
        if (anim_name == "game_portal_open")
        {
            m_levelClear = false;
            m_levelEntities.LevelLoad("level_01");
        }
    }

    public void AnimationComplete(string anim_name)
    {
        Debug.LogFormat("GameController.AnimationComplete {0}",anim_name);
       
        if (anim_name == "game_portal_open")
        {
            m_levelEnvironment.PlayAnimation("level_start");
        }
        else if (anim_name == "level_start")
        {            
            m_levelEnvironment.DisablePaddleBall();
            m_levelEntities.LevelStart();
        }
        else if (anim_name == "level_clear")
        {
            m_levelClear = true;
        }
    }

    public void LevelClear()
    {
        m_levelEnvironment.PlayAnimation("level_clear");
    }

    void FixedUpdate ()
    {
        
        if (Input.GetMouseButton(0))
        {                                    
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {  
                m_levelEntities.MouseDrag(hit.point);
            } 
        }
        if (Input.GetMouseButtonUp(0))
        {    
            Debug.Log("Mouse Release");
            m_levelEntities.MouseRelease();
        }
    }
}
