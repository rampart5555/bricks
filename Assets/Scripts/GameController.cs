﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public enum GCState{
        NONE,
        GAME_PORTAL_OPEN,
        GAME_PORTAL_CLOSE,
        LEVEL_START,
        LEVEL_CLEAR,
        LEVEL_COMPLETED,
        PADDLE_LOST,
        PADDLE_RESTORE
    };
        
    public GameObject m_levelEnvironmentGO;
    public GameObject m_levelEntitiesGO;
    public GameObject m_gamePortalGO;

    GamePortal m_gamePortal;
    LevelEnvironment m_levelEnvironment;
    LevelEntities  m_levelEntities;

    public bool m_levelClear;

    int m_paddleSpare;

    void Awake()
    {
        m_paddleSpare = 3;
    }

	void Start () 
    {
        Debug.Log("GameController.Start");
        m_gamePortal = m_gamePortalGO.GetComponent<GamePortal>();
        m_levelEnvironment = m_levelEnvironmentGO.GetComponent<LevelEnvironment>();
        m_levelEntities = m_levelEntitiesGO.GetComponent<LevelEntities>();

	}

    public void OnStateEnter(GCState state)
    {
        Debug.LogFormat("GameController.OnStateEnter {0}", state);
        switch (state)
        {
            case GCState.GAME_PORTAL_OPEN:
                {
                    m_gamePortal.PlayAnimation("game_portal_open", state);
                    m_levelClear = false;
                    m_levelEntities.LevelLoad("level_01");
                }
                break;
            case GCState.LEVEL_START:
                {
                    
                }
                break;
            case GCState.PADDLE_RESTORE:
                {
                    if (m_paddleSpare == 0)
                    {
                        Debug.Log("GAME OVER");
                    }
                    else
                    {
                        m_levelEntities.PaddleLost();
                        m_levelEnvironment.EnableEntities();
                        m_levelEnvironment.PlayAnimation(string.Format("paddle_restore_{0}", 
                            m_paddleSpare),GCState.PADDLE_RESTORE);
                        m_paddleSpare--;
                    }
                }             
                break;
            case GCState.LEVEL_CLEAR:
                {
                    m_levelEnvironment.PlayAnimation("level_clear",GCState.LEVEL_CLEAR);
                }
                break;
            case GCState.LEVEL_COMPLETED:
                {
                    m_gamePortal.PlayAnimation("game_portal_close", GCState.GAME_PORTAL_CLOSE);
                    m_levelClear = false;
                }
                break;
            default:
                break;    
        }
    }

    public void OnStateExit(GCState state)
    {
        Debug.LogFormat("GameController.OnStateExit: {0}", state);
        switch (state)
        {
            case GCState.GAME_PORTAL_OPEN:
                {
                    m_levelEnvironment.PlayAnimation("level_start", GCState.LEVEL_START);                 
                    //m_gamePortal.PlayAnimation("game_portal_close",GCState.NONE);
                }
                break;
            case GCState.LEVEL_START:
                {
                    m_levelClear = false;
                    m_levelEnvironment.DisableEntities();
                    m_levelEntities.LevelStart();
                }
                break;
            case GCState.PADDLE_RESTORE:
                {
                    m_levelEnvironment.DisableEntities();
                    m_levelEntities.LevelStart();
                }
                break;
            case GCState.LEVEL_CLEAR:
                {                    
                    m_levelEntities.LevelClear();
                    m_levelClear = true;
                }
                break;
            default:
                break;    
        }
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Mouse Release");
                if (hit.collider.gameObject.name == "button_continue")
                    OnStateEnter(GCState.GAME_PORTAL_OPEN);
                else
                    m_levelEntities.MouseRelease();
            }
        }
    }
}
