﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour 
{    
    public Text m_scoreGUI;
    public Text m_fpsGUI;

	public Bounds targetBounds;

    private GameObject m_ballPrefab;
    private GameObject m_paddlePrefab;
    private GameObject m_bulletPrefab;
    private GameObject m_bulletSpawn;
    private ParticleSystem m_explosionBrick;
    private BrickPrefab m_brickPrefab;
    private PowerupPrefab m_powerupPrefab;
    private Queue<GameObject> m_bulletList;
    private List<ParticleSystem> m_explosionBrickList;
    private int m_brickNumber;
    private int m_powerupNumber;
    private int m_ballNumber;
    private int m_score;
    private GameConfig m_gameConfig;
    float m_deltaTime = 0.000001f;

    /*ball and paddle */
    private List<GameObject> m_ballList;
    private GameObject m_paddle;
    /* store bricks and powerup game objects 
     * if the game objects are marked for deletion then remove them
     */
    private List<GameObject> m_cacheList;

	void Awake()
	{		
        Debug.Log ("GameController awake\n");
        m_score = 0;
        m_ballNumber = 0;
        m_brickNumber = 0;
		CameraSizeChange ();
        LoadPrefab ();
        InstantiatePrefab ();
        m_gameConfig = GetComponent<GameConfig> ();
        m_gameConfig.LoadFile ();
        string level = string.Format ("level_{0:00}", m_gameConfig.m_gameData.m_currentLevel);
        m_cacheList = new List<GameObject> ();
        LoadLevel (level);
	}

    void FixedUpdate () 
    {
        m_deltaTime += (Time.unscaledDeltaTime - m_deltaTime) * 0.1f;   
        for (int i = m_cacheList.Count - 1; i >= 0; i--) 
        {
            GameObject go = m_cacheList [i];
            if (go.activeSelf == false) 
            {
                HandleRemove(go);
                Destroy (go);
                m_cacheList.RemoveAt (i);
            }
        }
    }

    bool HandleRemove(GameObject go)
    {
        switch (go.tag)
        {
            case "Brick":
                {
                    RemoveBrick(go);              
                    if (m_brickNumber <= 0)
                    {
                        LevelComplete();
                    }
                }    
                break;
            case "Powerup":
                {
                    RemovePowerup(go);
                }   
                break;
            default:
                {
                    Debug.Log("No object to remove");
                }
                break;
        }
        return true;
    }

    void OnGUI()
    {
        float msec = m_deltaTime * 1000.0f;
        float fps = 1.0f / m_deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)\n Entities:{2:0.0}", msec, fps,m_cacheList.Count);  
        m_fpsGUI.text = text;
    }

    private void LoadPrefab()
    {        

        m_brickPrefab = new BrickPrefab ();
        m_brickPrefab.LoadPrefabs ();
        m_powerupPrefab = new PowerupPrefab ();
        m_powerupPrefab.LoadPrefabs ();

        /*ball prefab*/
        m_ballPrefab = (GameObject)Resources.Load("Prefab/ball", typeof(GameObject));
        /*paddle prefab*/
        m_paddlePrefab = (GameObject)Resources.Load("Prefab/paddle", typeof(GameObject));
        /* brick explosion */
        m_explosionBrick = (ParticleSystem)Resources.Load("Prefab/explosion_brick", typeof(ParticleSystem));
        /*bullets */
        m_bulletPrefab = (GameObject)Resources.Load("Prefab/bullet", typeof(GameObject));
    
    }

    private void InstantiatePrefab()
    {
        int i;

        /*ball*/
        m_ballList = new List<GameObject>();
        GameObject ball = Instantiate (m_ballPrefab);
        m_ballList.Add (ball);
        /*paddle*/
        m_paddle = Instantiate (m_paddlePrefab);

        /* atach ball to paddle*/
        FixedJoint2D tj = m_paddle.GetComponent<FixedJoint2D>();
        tj.connectedBody = ball.GetComponent<Rigidbody2D>();

        /*Bullet spawn LocationInfo*/
        m_bulletSpawn = GameObject.FindWithTag ("BulletSpawn");

        /* bullets */
        m_bulletList = new Queue<GameObject> (10);
        for (i = 0; i < 10; i++) 
        {
            GameObject bullet = Instantiate (m_bulletPrefab);
            bullet.SetActive (false);
            m_bulletList.Enqueue (bullet);
        }
        /*explosions*/
        m_explosionBrickList = new List<ParticleSystem>();
        for (i = 0; i < 20; i++)
        {
            ParticleSystem brick_exp = Instantiate(m_explosionBrick);
            brick_exp.Stop();
            m_explosionBrickList.Add(brick_exp);
        }

    }

    private void AddBrickExplosion(GameObject brick)
    {
        for (int i = 0; i < m_explosionBrickList.Count; i++)
        {
            if (m_explosionBrickList[i].IsAlive() == false)
            {
                ParticleSystem ps = m_explosionBrickList[i];
                ps.transform.position = brick.transform.position;
                ps.transform.rotation = brick.transform.rotation;
                ps.Play();
                break;
            }                
        }
    }
    void PerspectiveCameraDistance()
    {
        //3.0 is the height of the biggest object
	float frustumHeight = 3.0f / Camera.main.aspect;
	// Compute the distance the camera should be to fit the entire bounding sphere
	float distance = frustumHeight * 0.5f / Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
	Camera.main.transform.position=new Vector3(0.0f,0.0f,-distance);
	Debug.Log(Camera.main.aspect);
	Debug.Log(frustumHeight);
    }
	void CameraSizeChange()
	{
		float screenRatio = (float)Screen.width / (float)Screen.height;
		float targetRatio = targetBounds.size.x / targetBounds.size.y;

		if (screenRatio >= targetRatio)
		{
			Camera.main.orthographicSize = targetBounds.size.y / 2;
		}
		else
		{
			float differenceInSize = targetRatio / screenRatio;
			Camera.main.orthographicSize = targetBounds.size.y / 2 * differenceInSize;
		}
		transform.position = new Vector3(targetBounds.center.x, targetBounds.center.y, -1f);
		//string str = string.Format ("CameraSize: {0}", Camera.main.orthographicSize);
	}

    void AddBrick(int  i, int j, int brick_id, int powerup_id)
    {
        float x = 1.2f - i * 0.2f - 0.1f;
        float y = 1.4f - j * 0.1f - 0.1f;
        Brick.BrickType brick_type = (Brick.BrickType)brick_id;
        Powerup.PowerupType powerup_type = (Powerup.PowerupType)powerup_id;
        GameObject brick = (GameObject)m_brickPrefab.GetPrefab (brick_type);

        if (brick == null)
            return;
        
        GameObject brick_obj=Instantiate (brick, new Vector3 (x, y, 0.07f), Quaternion.identity) as GameObject;
        m_cacheList.Add (brick_obj);
        m_brickNumber++;
        if (powerup_type != Powerup.PowerupType.powerup_none) 
        {               
            brick_obj.GetComponent<Brick> ().m_powerupType = powerup_type;
        }
    }
           
    public void RemoveBrick(GameObject brick)
    {
        //brick.SetActive(true);
        Brick brick_obj = brick.GetComponent<Brick> ();
        Powerup.PowerupType powerup_type = brick_obj.GetPowerup ();
        if (powerup_type != Powerup.PowerupType.powerup_none)
            AddPowerup (brick.transform.position, powerup_type);
		
		UpdateScore (brick_obj.m_brickValue);
        AddBrickExplosion(brick);
        m_brickNumber--;

    }

    public void AddPowerup(Vector3 pos, Powerup.PowerupType powerup_type)
    {       
        Debug.LogFormat ("Add powerup {0}",powerup_type);
        GameObject powerup_prefab = m_powerupPrefab.GetPrefab(powerup_type);
        if (powerup_prefab == null)
            return;        
        GameObject powerup = (GameObject)Instantiate (powerup_prefab, pos, Quaternion.identity);
        m_cacheList.Add (powerup);
        m_powerupNumber++;
    }

    public void RemovePowerup(GameObject powerup)
    {        
        Powerup pup = powerup.GetComponent<Powerup> ();
        if (pup.m_powerupType == Powerup.PowerupType.powerup_balls) 
		{
            for (int i = 0; i < 3; i++) 
			{   
                Vector3 pos = new Vector3 ();
                pos.Set (0.0f, 0.0f, 0.5f);
				AddBall (pos,-1.0f + (float)i,1.0f);                
            }
        } 
		
        Debug.LogFormat("Remove powerup {0}",powerup.tag);
        m_powerupNumber--;
    }

    public void AddBullet()
    {
        GameObject bullet = m_bulletList.Dequeue ();
        if (bullet == null) 
        {
            Debug.Log ("Null bullet");
        }
        if (m_bulletSpawn == null) 
        {
            Debug.Log ("Null bullet spawn");
        }
        bullet.transform.position = m_paddle.transform.GetChild(0).position;
        bullet.SetActive (true);
        Bullet bullet_obj = bullet.GetComponent<Bullet> ();
        bullet_obj.ResetSpeed ();

    }
		
    public void RemoveBullet(GameObject bullet)
    {
        m_bulletList.Enqueue (bullet);
        bullet.SetActive (false);

    }

	public void AddBall(Vector3 pos, float speed_x, float speed_y)
	{		
        GameObject ball = Instantiate (m_ballPrefab, pos, Quaternion.identity);
		Ball ball_obj = ball.GetComponent<Ball> ();
		ball_obj.SetSpeed (speed_x, speed_y);
        m_ballNumber++;
	}

	public void RemoveBall(GameObject ball)
	{        
        if (m_ballNumber > 0) 
        {
            m_ballNumber--;
            Destroy (ball);
        }
	}

    public void PaddleReleaseBall()
    {
        Debug.Log ("ReleaseBall");

        AddBall(m_ballList[0].transform.position, 1.0f, 1.0f);
        Destroy (m_ballList[0]);

    }

    public void LoadLevel(string level)
    {        
        TextAsset m_textAsset = Resources.Load("Levels/"+level) as TextAsset;     
        if (m_textAsset == null) {            
            Debug.LogErrorFormat("File not found {0}",level);
            return;
        } 
        else 
        {
            Debug.Log ("Loading level " + level);
        }
        ParseTMX tmx = new ParseTMX ();
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
    }

    public void UpdateScore(int score)
    {
        m_score += score;
        m_scoreGUI.text = "Score " + m_score;
    }

    private void LevelComplete()
    {
        Debug.Log ("LevelComplete");
    }
}

class BrickPrefab
{
    public Dictionary<Brick.BrickType,GameObject> m_brickMap;
    Dictionary<Brick.BrickType,string > m_brickMapDef = new Dictionary<Brick.BrickType,string >()
    {        
        {Brick.BrickType.brick_brown,     "Prefab/brick_brown"},
        {Brick.BrickType.brick_red,       "Prefab/brick_red"},
        {Brick.BrickType.brick_turquoise, "Prefab/brick_turquoise"},
        {Brick.BrickType.brick_white,     "Prefab/brick_white"}
    };

    public BrickPrefab()
    {
        
    }

    public void LoadPrefabs()
    {   
        m_brickMap = new Dictionary<Brick.BrickType, GameObject> ();
        foreach(KeyValuePair<Brick.BrickType, string> entry in m_brickMapDef)
        {
            // do something with entry.Value or entry.Key
            GameObject brick = (GameObject)Resources.Load(entry.Value, typeof(GameObject));
            if (brick == null) 
            {
                Debug.LogWarningFormat ("Prefab not found: " + entry.Value);
                continue;
            }
            m_brickMap.Add (entry.Key, brick);
        }            
    }
    public GameObject GetPrefab(Brick.BrickType brick_type)
    {
        if (!m_brickMap.ContainsKey (brick_type)) 
        {
            Debug.LogWarningFormat ("Brick prefab not found {0}", brick_type);
            return null;
        }
        return m_brickMap [brick_type];
    }
}

class PowerupPrefab
{
    public Dictionary<Powerup.PowerupType,GameObject> m_powerupMap;
    Dictionary<Powerup.PowerupType,string > m_powerupMapDef = new Dictionary<Powerup.PowerupType,string >()
    {        
        {Powerup.PowerupType.powerup_balls, "Prefab/powerup_balls"},
        {Powerup.PowerupType.powerup_cannon, "Prefab/powerup_cannon"}
    };

    public PowerupPrefab()
    {

    }

    public void LoadPrefabs()
    {   
        m_powerupMap = new Dictionary<Powerup.PowerupType, GameObject> ();
        foreach(KeyValuePair<Powerup.PowerupType, string> entry in m_powerupMapDef)
        {
            // do something with entry.Value or entry.Key
            GameObject powerup = (GameObject)Resources.Load(entry.Value, typeof(GameObject));
            if (powerup == null) 
            {
                Debug.LogWarningFormat("Prefab not found:{0} ",entry.Value);
                continue;
            }
            m_powerupMap.Add (entry.Key, powerup);
        }            
    }

    public GameObject GetPrefab(Powerup.PowerupType powerup_type)
    {
        if (!m_powerupMap.ContainsKey (powerup_type)) 
        {
            Debug.LogWarningFormat ("Powerup prefab not found {0}", powerup_type);
            return null;
        }
        return m_powerupMap [powerup_type];
    }
}
