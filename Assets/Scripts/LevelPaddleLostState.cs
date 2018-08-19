using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPaddleLostState : StateMachineBehaviour 
{    
    GameObject m_gameControllerGO;
    GameController m_gameController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("*** LevelPaddleLostState.OnStateEnter ***");
        m_gameControllerGO = GameObject.FindGameObjectWithTag("GameController");
        m_gameController = m_gameControllerGO.GetComponent<GameController>();
        m_gameController.SetState(GameController.GCState.LEVEL_PADDLE_LOST_STATE_ENTER);

    }

    override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("*** LevelPaddleLostState.OnStateExit ***");
        m_gameController.SetState(GameController.GCState.LEVEL_PADDLE_LOST_STATE_EXIT);
    }
}
