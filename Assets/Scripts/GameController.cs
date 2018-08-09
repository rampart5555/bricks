using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public enum GCState{
        NONE,
        GAME_PORTAL_OPEN,
        LEVEL_START,
        LEVEL_CLEAR,
        PADDLE_LOST,
        PADDLE_RESTORE
    };
        
    public GameObject m_levelEnvironmentGO;
    public GameObject m_levelEntitiesGO;
    public GameObject m_gamePortalGO;

    GamePortal m_gamePortal;
    LevelEnvironment m_levelEnvironment;
    LevelEntities  m_levelEntities;

    bool m_levelClear;
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
        OnStateEnter(GCState.GAME_PORTAL_OPEN);

	}

    public void OnStateEnter(GCState state)
    {
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
                        m_levelEnvironment.PlayAnimation(string.Format("paddle_restore_{0}", 
                            m_paddleSpare),GCState.PADDLE_RESTORE);
                        m_paddleSpare--;
                    }
                }
                break;

            default:
                break;    
        }
    }

    public void OnStateExit(GCState state)
    {
        switch (state)
        {
            case GCState.GAME_PORTAL_OPEN:
                {
                    m_levelEnvironment.PlayAnimation("level_start", GCState.LEVEL_START);
                 
                }
                break;
            case GCState.LEVEL_START:
                {
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
            default:
                break;    
        }
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
            m_levelEnvironment.PlayAnimation("level_start",GCState.LEVEL_START);
        }
        else if (anim_name == "level_start")
        {            
            m_levelEnvironment.DisableEntities();
            m_levelEntities.LevelStart();
        }
        else if (anim_name == "level_clear")
        {
            m_levelClear = true; 
        }
    }

    public void LevelClear()
    {
        m_levelEnvironment.PlayAnimation("level_clear",GCState.LEVEL_CLEAR);
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
