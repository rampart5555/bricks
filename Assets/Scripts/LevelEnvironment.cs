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
    GameObject m_paddleEntryMesh;
    GameObject m_paddleEntrySlot;
    GameObject m_paddleSpareMesh;
    GameObject m_paddleSpareSlot;



    void Awake()
    {
        
        m_ballMesh = transform.Find("ball_mesh").gameObject;
        m_paddleEntryMesh = transform.Find("paddle_entry_mesh").gameObject;
        m_paddleSpareMesh = transform.Find("paddle_spare_mesh").gameObject;
        m_paddleSpareSlot = transform.Find("paddle_spare_slot").gameObject;
        GameObject go = Instantiate(m_portalCloseEffectGO, new Vector3(0.0f, 0.0f, -2.0f), Quaternion.identity);
        go.transform.parent = transform;
        m_portalCloseEffect = go.GetComponent<ParticleSystem>();
        m_levelNumber = m_levelNumberGO.GetComponent<TextMesh>();
    }

    void Start()
    {     
        GameObject m_gameControllerGO = GameObject.FindGameObjectWithTag("GameController");
        m_gameController=m_gameControllerGO.GetComponent<GameController>();
        StartCoroutine(LoadImage("game_portal_top","game_portal_top.png"));
        StartCoroutine(LoadImage("game_portal_bottom","game_portal_bottom.png"));
        StartCoroutine(LoadImage("background","background.png"));
        StartCoroutine(LoadImage("trackpad","trackpad.png"));
        StartCoroutine(LoadImage("hud","hud.png"));
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

    public void DisableSparePaddleMesh()
    {
        
        m_paddleSpareMesh.SetActive(false);
    }

    public void EnableSparePaddleMesh()
    {     
        m_paddleSpareMesh.SetActive(true);
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
