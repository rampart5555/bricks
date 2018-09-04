using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
COLOR_MAP = {
    "252,252,252" => 1, # white
    "252,116,96" => 2, # orange
    "60,188,252" => 3, # light blue
    "128,208,16" => 4, # green
    "216,40,0" => 5, # red
    "0,112,236" => 6, # blue
    "252,116,180" => 7, # pink
    "252,152,56" => 8, # yellow
    "188,188,188" => 9, # silver
    "240,188,60" => 10 # gold
}
*/
public class LevelEntities : MonoBehaviour 
{   
    public enum BrickType{
        BRICK_NONE=0,
        BRICK_WHITE,
        BRICK_ORANGE,
        BRICK_LIGHTBLUE,
        BRICK_GREEN,
        BRICK_RED,
        BRICK_BLUE,
        BRICK_PINK,
        BRICK_YELLOW,
        BRICK_SILVER,
        BRICK_GOLD
    };

    public enum PowerupType{
        POWERUP_NONE=0,
        POWERUP_BALLS=11,
        POWERUP_CANNON=12,
        POWERUP_FAST=13,
        POWERUP_SLOW=14,
        POWERUP_LIFE=15,
        POWERUP_EXIT=16
            
    };
    public GameObject m_paddleStartPos;
    public GameObject m_ballStartPos;
    public ParticleSystem m_brickExplosion;


    private List<ParticleSystem> m_brickExplosionList;
    private List<GameObject> m_ballList;
    private List<GameObject> m_powerupList;
    private List<GameObject> m_brickList;
    private Queue<GameObject> m_bulletList;

    GameObject m_brickGO;
    GameObject m_powerupGO;
    GameObject m_ballGO;
    GameObject m_bulletGO;

    Paddle m_paddle;
    Ball m_ball;
    GameController m_gameController;

    int m_brickNumber;
    int m_ballNumber;
    int m_levelScore;

    void Start()
    {
        m_brickGO = transform.Find("brick").gameObject;
        m_powerupGO=transform.Find("powerup").gameObject;
        m_ballGO=transform.Find("ball").gameObject;
        m_bulletGO=transform.Find("bullet").gameObject;

        m_gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        m_paddle = transform.Find("paddle").gameObject.GetComponent<Paddle>();
        //m_ball = m_ballGO.GetComponent<Ball>();

        m_ballList = new List<GameObject>();
        m_powerupList = new List<GameObject>();
        m_brickList = new List<GameObject>();
        m_levelScore = 0;
        InstatiateEntities();

    }

    void LateUpdate()
    {
        int i;
        for(i = m_brickList.Count - 1; i >= 0; i--)
        {
            GameObject brickGO = m_brickList[i];
            if (brickGO.activeSelf == false)
            {
                Brick br = brickGO.GetComponent<Brick>();
                m_levelScore += br.GetValue();
                m_gameController.UpdateScore(m_levelScore);
                m_brickNumber--;
                Destroy(brickGO);
                m_brickList.RemoveAt(i);
            }  
            if (m_brickList.Count == 0)
            {
                m_gameController.LevelCleared();
            }
        }
        for(i=m_ballList.Count-1; i>=0; i--)
        {
            GameObject ballGO = m_ballList[i];
            if(ballGO.activeSelf==false)
            {
                m_ballList.RemoveAt(i);
                m_ballNumber--;
            }
            if (m_ballList.Count == 0)
            {                                
                m_gameController.LevelPaddleLost();
            }
        }
    }

    private void InstatiateEntities()
    {
        Debug.Log("LevelEntities.InstatiateEntities");
        int i;
        m_brickExplosionList = new List<ParticleSystem>();
        for (i = 0; i < 20; i++)
        {
            ParticleSystem brick_exp = Instantiate(m_brickExplosion);
            brick_exp.Stop();
            m_brickExplosionList.Add(brick_exp);
        }

        m_bulletList = new Queue<GameObject> (10);
        for (i = 0; i < 10; i++) 
        {
            GameObject bullet = Instantiate (m_bulletGO);
            bullet.SetActive (false);
            m_bulletList.Enqueue (bullet);
        }
    }

    public void MouseRelease()
    {        
        if (m_ball.m_status == Ball.BallStatus.BallAttached)
        {
            Debug.LogFormat("LevelEntities.MouseRelease {0}", m_ball.m_status);
            m_paddle.BallRelease();
            m_ball.BallStart();
        }
    }

    public void MouseDrag(Vector2 pos)
    {        
        m_paddle.PaddleMove(pos);
    }
        
    public void LevelComplete()
    {
        m_ball.SetSpeed(0.0f, 0.0f);
    }

    public void LevelStop()
    {
        Debug.Log("LevelEntities.LevelStop");
        m_paddle.Reset();
        m_paddle.gameObject.SetActive(false);
        m_ballGO.SetActive(false);
        foreach(GameObject go in m_ballList)
        {
            go.SetActive(false);
        }
        foreach(GameObject go in m_powerupList)
        {
            go.SetActive(false);
        }
    }

    public void LevelStart()
    {
        Debug.Log("LevelEntities.LevelStart");
        m_paddle.gameObject.SetActive(true);
        GameObject ballGO = Instantiate(m_ballGO);
        ballGO.SetActive(true);
        m_ballList.Add(ballGO);
        m_ball = ballGO.GetComponent<Ball>();
        m_ballNumber = 1;
        Rigidbody2D brb = m_ball.gameObject.GetComponent<Rigidbody2D>();
        m_paddle.BallAttach(brb);
        m_ball.m_status = Ball.BallStatus.BallAttached;
        m_paddle.gameObject.transform.position = m_paddleStartPos.transform.position;
        m_ball.gameObject.transform.position = m_ballStartPos.transform.position;

    }

    void LevelReset()
    {
        m_brickNumber = 0;
        foreach(GameObject go in m_ballList)
        {
            Destroy(go);
        }
        m_ballList.Clear();
        foreach(GameObject go in m_powerupList)
        {
            Destroy(go);
        }
        m_powerupList.Clear();
        foreach (GameObject go in m_brickList)
        {
            Destroy(go);
        }
        m_brickList.Clear();
    }

    public void LevelLoad(string level)
    {
        LevelReset();
        m_brickGO.SetActive(true);
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
        m_powerupGO.SetActive(false);
        //tmx.Dump();
    }

    private void AddBrickExplosion(GameObject brick)
    {
        for (int i = 0; i < m_brickExplosionList.Count; i++)
        {
            if (m_brickExplosionList[i].IsAlive() == false)
            {
                ParticleSystem ps = m_brickExplosionList[i];
                ps.transform.position = brick.transform.position;
                ps.transform.rotation = brick.transform.rotation;
                ps.Play();
                AudioSource ac = ps.gameObject.GetComponent<AudioSource>();
                ac.Play();
                break;
            }                
        }
    }

    void AddBrick(int  i, int j, int brick_id, int powerup_id)
    {
        //Debug.Log("LevelEntities.AddBrick");
        float x = 1.2f - i * 0.2f - 0.1f;
        float y = 1.4f - j * 0.1f - 0.1f;
        //Brick.BrickType brick_type = (Brick.BrickType)brick_id;
        //Powerup.PowerupType powerup_type = (Powerup.PowerupType)powerup_id;
        //GameObject brick = (GameObject)m_brickPrefab.GetPrefab (brick_type);

        GameObject brick_obj=Instantiate (m_brickGO, new Vector3 (x, y, 0.0f), Quaternion.identity) as GameObject;
        m_brickList.Add(brick_obj);
        brick_obj.transform.parent = transform;
        Brick brick = brick_obj.GetComponent<Brick>();
        brick.SetBrickType((BrickType)brick_id);
        brick.m_powerupType = (PowerupType)powerup_id;

        m_brickNumber++;

    }

    public void AddPowerup(GameObject brick_obj)
    {        
        Brick brick = brick_obj.GetComponent<Brick>();
        if (brick.m_powerupType == PowerupType.POWERUP_NONE)
            return;
        Debug.LogFormat("LevelEntities.AddPowerup {0}", brick.m_powerupType);
        GameObject pup_go = Instantiate(m_powerupGO, brick_obj.transform.position, Quaternion.identity);
        pup_go.transform.parent = transform;
        m_powerupList.Add(pup_go);
        pup_go.SetActive(true);
        Rigidbody2D rb = pup_go.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0.0f, -1.5f);
        Powerup pup = pup_go.GetComponent<Powerup>();
        pup.m_powerupType = brick.m_powerupType;

    }

    public void AddBalls()
    {        
        for (int i = 0; i < 3; i++)
        {
            GameObject ball_go = Instantiate(m_ballGO, Vector3.zero, Quaternion.identity);
            ball_go.SetActive(true);
            ball_go.transform.parent = transform;
            m_ballList.Add(ball_go);
            Ball ball = ball_go.GetComponent<Ball>();
            ball.SetSpeed(-1.0f + (float)i, 1.0f);
            m_ballNumber++;
        }
    }

    public void AddBullet(Vector3 pos)
    {
        GameObject bullet = m_bulletList.Dequeue ();
        bullet.SetActive (true);
        bullet.transform.position = pos;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0.0f, 3.0f);
    }

    public void RemoveBullet(GameObject bullet)
    {
        m_bulletList.Enqueue(bullet);
        bullet.SetActive(false);
    }

    public void RemoveBrick(GameObject brick)
    {        
        //Debug.LogFormat("LevelEntities.RemoveBrick {0}", m_brickNumber);
        Brick br = brick.GetComponent<Brick>();

        if (br.IsHit() > 0)
            return;
        
        brick.SetActive(false);
        AddBrickExplosion(brick);

        AddPowerup(brick);
    }

    public void RemoveBall(GameObject ball)
    {
        Debug.LogFormat("LevelEntities.RemoveBall {0}",m_ballNumber);

        ball.SetActive(false);
    }

    public void RemovePowerup(GameObject powerup)
    {
        Powerup pup = powerup.GetComponent<Powerup>();
        if (pup.m_powerupType == PowerupType.POWERUP_BALLS)
        {
            AddBalls();
        }
        powerup.SetActive(false);
    }
}
