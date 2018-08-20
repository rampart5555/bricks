using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEntities : MonoBehaviour 
{   
    public enum BrickType{
        BRICK_NONE=0,
        BRICK_BROWN,
        BRICK_RED,
        BRICK_BLUE,
        BRICK_WHITE
    };
    public enum PowerupType{
        POWERUP_NONE=0,
        POWERUP_CANNON=5,
        POWERUP_BALLS=6
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
    Brick m_brick;
    GameController m_gameController;
    int m_brickNumber;
    int m_ballNumber;

    void Start()
    {
        m_brickGO = transform.Find("brick").gameObject;
        m_powerupGO=transform.Find("powerup").gameObject;
        m_ballGO=transform.Find("ball").gameObject;

        m_gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        m_paddle = transform.Find("paddle").gameObject.GetComponent<Paddle>();
        m_ball = m_ballGO.GetComponent<Ball>();

        m_ballList = new List<GameObject>();
        m_powerupList = new List<GameObject>();
        m_brickList = new List<GameObject>();
        m_ballNumber = 1;
        InstatiateEntities();

    }

    void LateUpdate()
    {
        //Debug.Log("lateupdate");
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
        m_paddle.gameObject.SetActive(false);
        m_ball.gameObject.SetActive(false);
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
        m_ball.gameObject.SetActive(true);
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
        Debug.Log("LevelEntities.AddBrick");
        float x = 1.2f - i * 0.2f - 0.1f;
        float y = 1.4f - j * 0.1f - 0.1f;
        //Brick.BrickType brick_type = (Brick.BrickType)brick_id;
        //Powerup.PowerupType powerup_type = (Powerup.PowerupType)powerup_id;
        //GameObject brick = (GameObject)m_brickPrefab.GetPrefab (brick_type);

        GameObject brick_obj=Instantiate (m_brickGO, new Vector3 (x, y, 0.0f), Quaternion.identity) as GameObject;
        m_brickList.Add(brick_obj);
        brick_obj.transform.parent = transform;
        Brick brick = brick_obj.GetComponent<Brick>();
        brick.m_brickType = (BrickType)brick_id;
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
            ball_go.transform.parent = transform;
            m_ballList.Add(ball_go);
            Ball ball = ball_go.GetComponent<Ball>();
            ball.SetSpeed(-1.0f + (float)i, 1.0f);
            m_ballNumber++;
        }
    }

    public void AddBullet()
    {
        GameObject bullet = m_bulletList.Dequeue ();
        bullet.SetActive (true);
    }

    public void RemoveBrick(GameObject brick)
    {
        Debug.LogFormat("LevelEntities.RemoveBrick {0}", m_brickNumber);
        AddBrickExplosion(brick);
        m_brickNumber--;

        if (m_brickNumber <= 0)
        {
            m_gameController.LevelCleared();
        }
        AddPowerup(brick);
    }

    public void RemoveBall(GameObject ball)
    {
        Debug.LogFormat("LevelEntities.RemoveBall {0}",m_ballNumber);
        m_ballNumber--;
        if (m_ballNumber <= 0)
        {
            m_gameController.LevelPaddleLost();
        }
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
