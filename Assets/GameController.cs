using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Button m_gameComplete;
    public Button m_paddleLost;
    public GameObject m_levelEnvironmentGO;
    public GameObject m_gamePortalGO;

    GamePortal m_gamePortal;
    LevelEnvironment m_levelEnvironment;

	void Start () 
    {
        Debug.Log("GameController.Start");
        m_gamePortal = m_gamePortalGO.GetComponent<GamePortal>();
        m_levelEnvironment = m_levelEnvironmentGO.GetComponent<LevelEnvironment>();
        m_gamePortal.PortalOpen();

	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void AnimationComplete(string anim_name)
    {
        Debug.LogFormat("GameController.AnimationComplete {0}",anim_name);
       
        if (anim_name == "game_portal_open")
        {
            m_levelEnvironment.PlayAnimation("level_start");
        }
    }
}
