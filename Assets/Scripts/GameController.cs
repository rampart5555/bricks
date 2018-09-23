using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class GameData
{
    public int m_totalScore;
    public int m_currentLevel;
    public int m_lives;

    public GameData()
    {
        m_totalScore = 0;
        m_currentLevel = 1;
        m_lives = 3;
    }
}

public class GameController : MonoBehaviour {

    public enum GCState{
        NONE,
        LEVEL_ENTRY_STATE_ENTER,
        LEVEL_ENTRY_STATE_EXIT,
        LEVEL_START_STATE_ENTER,
        LEVEL_START_STATE_EXIT,
        LEVEL_CLEARED_STATE_ENTER,
        LEVEL_CLEARED_STATE_EXIT,
        LEVEL_PADDLE_LOST_STATE_ENTER,
        LEVEL_PADDLE_LOST_STATE_EXIT
    };

    public Text m_fpsGUI;
    public GameObject m_scoreGO;
    public GameObject m_levelEnvironmentGO;
    public GameObject m_levelEntitiesGO;


    LevelEnvironment m_levelEnvironment;
    LevelEntities  m_levelEntities;

    TextMesh m_score;

    public bool m_doorRightIsOpen;
    private bool m_levelComplete;

    int m_paddleSpare;
    int m_levelNumber;
    int m_currentScore;
    float m_deltaTime;

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
        m_score = m_scoreGO.GetComponent<TextMesh>();
        m_levelEnvironment.SetLevelNumber(m_levelNumber);
        LoadConfig();
	}

    void LoadConfig()
    {
        GameData data = new GameData();
        LoadFile(ref data);
        m_currentScore = data.m_totalScore;
        m_paddleSpare = data.m_lives;
        m_levelNumber = data.m_currentLevel;
        Debug.LogFormat("score {0}, lives {1}, levelNumber {2}", m_score, m_paddleSpare, m_levelNumber);
        UpdateScore(0);//display score stored in config
        m_levelEnvironment.SetLevelNumber(m_levelNumber);//update level number
    }

    void SaveConfig()
    {
        GameData data = new GameData();
        data.m_totalScore = m_currentScore;
        data.m_lives=m_paddleSpare;
        data.m_currentLevel = m_levelNumber;
        SaveFile(data);
    }

    void ResetConfig()
    {
        GameData data = new GameData();
        data.m_totalScore = 0;
        data.m_lives=3;
        data.m_currentLevel = 1;
        SaveFile(data);
        LoadConfig();
    }

    public void SaveFile(GameData data)
    {
        string destination = Application.persistentDataPath + "/config.dat";
        FileStream file;
        Debug.Log(destination);
        if (File.Exists (destination))
            file = File.OpenWrite (destination);
        else
            file = File.Create (destination);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadFile(ref GameData data)
    {
        string destination = Application.persistentDataPath + "/config.dat";
        FileStream file;

        if(File.Exists(destination))
            file = File.OpenRead(destination);
        else
        {
            Debug.LogErrorFormat("File not found {0}", destination);
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        data= (GameData) bf.Deserialize(file);
        file.Close();
    }

    void OnGUI()
    {
        
        float msec = m_deltaTime * 1000.0f;
        float fps = 1.0f / m_deltaTime;
        //string text = string.Format("{0:0.0} ms ({1:0.} fps)\n Entities:{2:0.0}", msec, fps,m_cacheList.Count);  
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);  
        m_fpsGUI.text = text;


    }

    public void UpdateScore(int score)
    {
        m_currentScore += score;
        string text=string.Format(" {0} ", m_currentScore);  
        m_score.text = text;
    }

    public void SetState(GCState state)
    {        
        switch (state)
        {
            case GCState.LEVEL_ENTRY_STATE_ENTER:
                {
                    m_doorRightIsOpen = false;
                    m_levelComplete = false;
                    string level_str=string.Format("level_{0:00}",m_levelNumber);
                    m_levelEntities.LevelStop();
                    m_levelEntities.LevelLoad(level_str);
                    string[] entities_1= {"ball_mesh", "paddle_entry_mesh" };
                    m_levelEnvironment.EnableEntities(entities_1);
                    m_levelNumber++;

                }
                break;
            case GCState.LEVEL_ENTRY_STATE_EXIT:
                {
                    string[] entities = { "ball_mesh", "paddle_entry_mesh" };
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
            case GCState.LEVEL_PADDLE_LOST_STATE_ENTER:
                {
                    SaveConfig();
                    m_levelEnvironment.EnableSparePaddleMesh();
                    break;
                }
            case GCState.LEVEL_PADDLE_LOST_STATE_EXIT:
                {                    
                    m_levelEnvironment.DisableSparePaddleMesh();

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
        SaveConfig();

    }

    void GameOver()
    {
        Debug.Log("GameController.GameOver");
        m_levelEnvAnimator.SetTrigger("game_over");
    }

    public void LevelPaddleLost()
    {
        if (m_doorRightIsOpen == true)
        {
            LevelComplete();
            return;
        }
        if (m_paddleSpare == 0)
        {            
            GameOver();
            return;
        }

        m_levelEntities.LevelStop();
        //string trigger=string.Format("paddle_lost_{0}",m_paddleSpare);
        m_levelEnvAnimator.SetTrigger("paddle_lost");
        m_paddleSpare--;

    }

    public void GamePortalCloseEvent()
    {
        Debug.Log("GameController.GamePortalCloseEvent");
        m_levelEntities.LevelStop();
    }

    public void LevelComplete()
    {
        if (m_levelComplete == false)
        {
            Debug.LogFormat("*** GameController.LEVEL_COMPLETE ***");
            m_levelEnvAnimator.SetTrigger("level_complete");
            m_levelComplete = true;
            m_levelEnvironment.SetLevelNumber(m_levelNumber);
            m_levelEntities.LevelComplete();
        }
    }

    public void LevelLoad()
    {
        //m_levelEntities.LevelLoad("level_01");
    }


   
    void FixedUpdate ()
    {
        m_deltaTime += (Time.unscaledDeltaTime - m_deltaTime) * 0.1f;   
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
                    if (m_paddleSpare == 0)
                    {
                        Debug.Log("no mode paddles");
                        return;
                    }
                    m_levelEnvAnimator.SetTrigger("level_entry");
                    //m_levelEnvironment.GamePortalCloseEvent();
                }
                else if (hit.collider.gameObject.name == "reset_config")
                {
                    Debug.Log("Button ResetConfig pressed");
                    ResetConfig();
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
        if(Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("Force to next level");
            LevelCleared();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            GameOver();
        }
    }
}
