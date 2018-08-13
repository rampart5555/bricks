using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartState : StateMachineBehaviour 
{
    GameObject m_gameControllerGO;
    GameController m_gameController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("*** LevelStartState.OnStateEnter ***");
        m_gameControllerGO = GameObject.FindGameObjectWithTag("GameController");
        m_gameController = m_gameControllerGO.GetComponent<GameController>();
        m_gameController.SetState(GameController.GCState.LEVEL_START_STATE_ENTER);

    }

    override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_gameController.SetState(GameController.GCState.LEVEL_START_STATE_EXIT);
    }
}
