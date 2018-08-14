﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelEnvironment: MonoBehaviour
{
    GameObject m_gameControllerGO;
    GameController m_gameController;


    GameObject m_ballMesh;
    GameObject m_paddleMesh;
    GameObject [] m_paddleMeshSlots;

    void Awake()
    {
        
        m_ballMesh = transform.Find("ball_mesh").gameObject;
        m_paddleMesh = transform.Find("paddle_mesh_0").gameObject;
        m_paddleMeshSlots = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            m_paddleMeshSlots[i] = transform.Find(string.Format("paddle_mesh_{0}", i + 1)).gameObject;
        }
    }

    void Start()
    {     
        GameObject m_gameControllerGO = GameObject.FindGameObjectWithTag("GameController");
        m_gameController=m_gameControllerGO.GetComponent<GameController>();
    }

    public void DoorRightOpenComplete()
    {
        m_gameController.DoorRightOpenComplete();
    }

    public void SetGamePortalPosOpen()
    {
        Transform tr;  
        tr = transform.Find("game_portal_top");
        tr.localPosition = new Vector3(tr.localPosition.x, 2.6f, tr.localPosition.z);
        tr = transform.Find("game_portal_bottom");
        tr.localPosition = new Vector3(tr.position.x, -2.6f, tr.localPosition.z);
    }

    public void DisableEntities(string [] entities)
    {
        Transform tr;
        foreach (string ent_str in entities)
        {
            tr=transform.Find(ent_str);
            tr.gameObject.SetActive(false);
        }            
    }
        
    public void EnableEntities(string [] entities)
    {
        Transform tr;
        foreach (string ent_str in entities)
        {
            tr=transform.Find(ent_str);
            tr.gameObject.SetActive(true);
        }            
    }           
}
