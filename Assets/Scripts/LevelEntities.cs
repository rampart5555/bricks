using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEntities : MonoBehaviour 
{   
    GameObject m_brickGO;
    Paddle m_paddle;
    Ball m_ball;
    Brick m_brick;
    GameController m_gameController;
    int m_brickNumber;

    void Start()
    {
        m_gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        m_paddle = transform.Find("paddle").gameObject.GetComponent<Paddle>();
        m_ball = transform.Find("ball").gameObject.GetComponent<Ball>();
        m_paddle.gameObject.SetActive(false);
        m_ball.gameObject.SetActive(false);
        m_brickGO = transform.Find("brick").gameObject;

    }
        
    public void MouseRelease()
    {        
        if (m_ball.m_status == Ball.BallStatus.BallAttached)
        {
            Debug.LogFormat("LevelEntities.MouseRelease {0}", m_ball.m_status);
            m_ball.BallRelease();
        }
    }

    public void MouseDrag(Vector2 pos)
    {        
        m_paddle.PaddleMove(pos);
    }

    public void LevelStart()
    {
        Debug.Log("LevelEntities.LevelStart");
        Debug.Log("LevelEntities.EnableEntities");
        m_paddle.gameObject.SetActive(true);
        m_ball.gameObject.SetActive(true);

    }

    void LevelReset()
    {
        m_brickNumber = 0;
    }

    public void LevelLoad(string level)
    {
        LevelReset();
        TextAsset m_textAsset = Resources.Load("Levels/"+level) as TextAsset;     
        if (m_textAsset == null) {            
            Debug.LogErrorFormat("File not found {0}",level);
            return;
        } 
        else 
        {
            Debug.Log ("Loading level " + level);
        }
        TmxParser tmx = new TmxParser ();
        tmx.ParseFile (m_textAsset.text);

        for (int j = 0; j < tmx.m_height; j++) 
        {
            for (int i = 0; i < tmx.m_width; i++) 
            {
                int brick_id = tmx.m_brickData [j * tmx.m_width + i];
                int powerup_id = tmx.m_powerupData [j * tmx.m_width + i];
                if (brick_id != 0) 
                {                    
                    AddBrick (i, j, brick_id, powerup_id);
                }
            }        
        }
        m_brickGO.SetActive(false);
        //tmx.Dump();
    }

    void AddBrick(int  i, int j, int brick_id, int powerup_id)
    {
        Debug.Log("LevelEntities.AddBrick");
        float x = 1.2f - i * 0.2f - 0.1f;
        float y = 1.4f - j * 0.1f - 0.1f;
        //Brick.BrickType brick_type = (Brick.BrickType)brick_id;
        //Powerup.PowerupType powerup_type = (Powerup.PowerupType)powerup_id;
        //GameObject brick = (GameObject)m_brickPrefab.GetPrefab (brick_type);

        GameObject brick_obj=Instantiate (m_brickGO, new Vector3 (x, y, 0.0f), Quaternion.identity) as GameObject;
        brick_obj.transform.parent = transform;
        m_brickNumber++;

    }

    public void RemoveBrick(GameObject brick)
    {
        m_brickNumber--;
        Debug.LogFormat("LevelEntities.RemoveBrick {0}", m_brickNumber);
        if (m_brickNumber <= 0)
        {
            m_gameController.LevelClear();
        }
    }
}
