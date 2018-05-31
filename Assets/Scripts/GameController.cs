using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour 
{
    public Button m_butLoadLevel;
    public Text m_scoreGUI;
    public Text m_fpsGUI;

	public Bounds targetBounds;

    private BrickPrefab m_brickPrefab;
    private PowerupPrefab m_powerupPrefab;
    private int m_brickNumber;
    private int m_score;

    float m_deltaTime = 0.000001f;
	void Start()
	{		        
        m_score = 0;
        m_butLoadLevel.onClick.AddListener(delegate {LoadLevel("level_01");});
		CameraSizeChange ();
        LoadPrefab ();
	}
    void Update () 
    {
        m_deltaTime += (Time.unscaledDeltaTime - m_deltaTime) * 0.1f;       
    }
    void OnGUI()
    {
        float msec = m_deltaTime * 1000.0f;
        float fps = 1.0f / m_deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);  
        m_fpsGUI.text = text;
    }

    private void LoadPrefab()
    {
        m_brickPrefab = new BrickPrefab ();
        m_brickPrefab.LoadPrefabs ();
        m_powerupPrefab = new PowerupPrefab ();
        m_powerupPrefab.LoadPrefabs ();
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
		string str = string.Format ("CameraSize: {0}", Camera.main.orthographicSize);
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
        m_brickNumber++;

        if (powerup_type != Powerup.PowerupType.powerup_none) 
        {               
            brick_obj.GetComponent<Brick> ().m_powerupType = powerup_type;
        }
    }

    public void AddPowerup(Vector3 pos, Powerup.PowerupType powerup_type)
    {       
        GameObject powerup = m_powerupPrefab.GetPrefab(powerup_type);
        if (powerup == null)
            return;        
        Instantiate (powerup, pos, Quaternion.identity);
    }

    public void RemoveBrick(GameObject brick)
    {
        Brick brick_obj = brick.GetComponent<Brick> ();
        Powerup.PowerupType powerup_type = brick_obj.GetPowerup ();
        if (powerup_type != Powerup.PowerupType.powerup_none)
            AddPowerup (brick.transform.position, powerup_type);
        else
            Debug.LogWarningFormat ("Not found {0}", powerup_type);
        Destroy (brick);
        m_brickNumber--;
    }

    public void LoadLevel(string level)
    {
        TextAsset m_textAsset = Resources.Load("Levels/"+level) as TextAsset;     
        if (m_textAsset == null) 
        {            
            Debug.Log ("File not found\n");
            return;
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
                Debug.LogWarningFormat("Prefab not found:{} ",entry.Value);
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
