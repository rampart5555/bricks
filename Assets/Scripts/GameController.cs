using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public enum GCState{
        NONE,
        LEVEL_ENTRY_STATE_ENTER,
        LEVEL_ENTRY_STATE_EXIT,
        LEVEL_START_STATE_ENTER,
        LEVEL_START_STATE_EXIT,
        LEVEL_CLEARED_STATE_ENTER,
        LEVEL_CLEARED_STATE_EXIT,
    };
        
    public GameObject m_levelEnvironmentGO;
    public GameObject m_levelEntitiesGO;

    LevelEnvironment m_levelEnvironment;
    LevelEntities  m_levelEntities;

    public bool m_doorRightIsOpen;
    private bool m_levelComplete;

    int m_paddleSpare;

    Animator m_levelEnvAnimator;

    void Awake()
    {
        m_paddleSpare = 3;
    }

	void Start () 
    {
        Debug.Log("GameController.Start");

        m_levelEnvironment = m_levelEnvironmentGO.GetComponent<LevelEnvironment>();
        m_levelEntities = m_levelEntitiesGO.GetComponent<LevelEntities>();
        m_levelEnvAnimator = m_levelEnvironmentGO.GetComponent<Animator>();

	}
    public void SetState(GCState state)
    {
        switch (state)
        {
            case GCState.LEVEL_ENTRY_STATE_ENTER:
                {
                    m_doorRightIsOpen = false;
                    m_levelComplete = false;
                    m_levelEntities.LevelLoad("level_01");
                    string[] entities_1= {"ball_mesh", "paddle_mesh_0" };
                    m_levelEnvironment.EnableEntities(entities_1);
                    m_levelEntities.LevelStop();
                }
                break;
            case GCState.LEVEL_ENTRY_STATE_EXIT:
                {
                    string[] entities = { "game_portal_top", "game_portal_bottom", "ball_mesh", "paddle_mesh_0" };
                    m_levelEnvironment.DisableEntities(entities);
                }
                break;
            case GCState.LEVEL_START_STATE_ENTER:
                {                    
                    m_levelEntities.LevelStart();
                }
                break;

            case GCState.LEVEL_CLEARED_STATE_ENTER:                
                break;

            case GCState.LEVEL_CLEARED_STATE_EXIT:
                {                    
                }
                break;
            default:
                break;
        }
    }

    public void DoorRightOpenComplete()
    {
        m_doorRightIsOpen = true;
    }

    public void LevelCleared()
    {
        m_levelEnvAnimator.SetTrigger("level_cleared");
    }

    public void LevelPaddleLost()
    {
        if (m_paddleSpare == 0)
        {
            Debug.Log("*** GAME OVER ***");
            return;
        }

        m_levelEntities.LevelStop();
        string trigger=string.Format("paddle_lost_{0}",m_paddleSpare);
        m_levelEnvAnimator.SetTrigger(trigger);
        m_paddleSpare--;

    }

    public void LevelComplete()
    {
        if (m_levelComplete == false)
        {
            Debug.LogFormat("*** GameController.LEVEL_COMPLETE ***");
            m_levelEnvAnimator.SetTrigger("level_complete");
            m_levelComplete = true;
            string[] entities = { "game_portal_top", "game_portal_bottom"};
            m_levelEnvironment.EnableEntities(entities);
            m_levelEntities.LevelComplete();
        }
    }

    public void LevelLoad()
    {
        //m_levelEntities.LevelLoad("level_01");
    }


   
    void FixedUpdate ()
    {

        if (Input.GetMouseButtonUp(0))
        {    
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {       
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject.name == "button_continue")
                {
                    Debug.Log("Button Continue pressed");
                    m_levelEnvAnimator.SetTrigger("level_entry");
                }
                else
                    m_levelEntities.MouseRelease();
            }
        }

        if (Input.GetMouseButton(0))
        {                                    
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {  
                m_levelEntities.MouseDrag(hit.point);
            } 
        }

    }
}
