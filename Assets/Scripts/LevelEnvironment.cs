using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelEnvironment: MonoBehaviour
{
    public GameObject m_portalCloseEffectGO;
    public GameObject m_levelNumberGO;
    ParticleSystem m_portalCloseEffect;
    TextMesh m_levelNumber;
    GameObject m_gameControllerGO;
    GameController m_gameController;

    GameObject m_ballMesh;
    GameObject m_paddleMesh;
    GameObject [] m_paddleMeshSlot;



    void Awake()
    {
        
        m_ballMesh = transform.Find("ball_mesh").gameObject;
        m_paddleMesh = transform.Find("paddle_mesh_0").gameObject;
        m_paddleMeshSlot = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            m_paddleMeshSlot[i] = transform.Find(string.Format("paddle_mesh_{0}", i + 1)).gameObject;
        }
        GameObject go = Instantiate(m_portalCloseEffectGO, new Vector3(0.0f, 0.0f, -2.0f), Quaternion.identity);
        go.transform.parent = transform;
        m_portalCloseEffect = go.GetComponent<ParticleSystem>();
        m_levelNumber = m_levelNumberGO.GetComponent<TextMesh>();
    }

    void Start()
    {     
        GameObject m_gameControllerGO = GameObject.FindGameObjectWithTag("GameController");
        m_gameController=m_gameControllerGO.GetComponent<GameController>();
        StartCoroutine(LoadImage("game_portal_top","game_portal_top.jpeg"));
        StartCoroutine(LoadImage("game_portal_bottom","game_portal_bottom.jpeg"));
    }
        
    IEnumerator  LoadImage(string go_name,string img_name)
    {        
        string imgpath = Application.dataPath +"/../images/" + img_name;


        GameObject portal_top = GameObject.Find(go_name);
        Renderer rend = portal_top.GetComponent<Renderer>();

        Debug.Log(imgpath);
        WWW www = new WWW("file:///" + imgpath);

        yield return www;
        Debug.Log(www.progress);
        rend.material.mainTexture = www.texture; 
    }

    public void SetLevelNumber(int levelnr)
    {
        m_levelNumber.text = "Level " + levelnr;
    }

    public void DisablePaddleMesh(int slot)
    {
        m_paddleMeshSlot[slot].SetActive(false);
    }

    public void GamePortalCloseEvent()
    {    
        m_gameController.GamePortalCloseEvent();   
        m_portalCloseEffect.Play();
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
