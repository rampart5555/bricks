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
    private int m_brickNumber;
    private int m_score;

    float m_deltaTime = 0.000001f;
	void Start()
	{		        
        m_score = 0;
        m_butLoadLevel.onClick.AddListener(delegate {LoadLevel("level_01");});
		CameraSizeChange ();
        LoadBrickPrefab ();
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

    private void LoadBrickPrefab()
    {
        m_brickPrefab = new BrickPrefab ();
        m_brickPrefab.LoadPrefabs ();
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

    void AddBrick(int  i, int j, int brick_id)
    {
        float x = 1.2f - i * 0.2f - 0.1f;
        float y = 1.4f - j * 0.1f - 0.1f;
        Brick.BrickType brick_type = (Brick.BrickType)brick_id;
        if (m_brickPrefab.m_brickMap.ContainsKey (brick_type)) 
        {
            Instantiate (m_brickPrefab.m_brickMap [brick_type], new Vector3 (x, y, 0.07f), Quaternion.identity);
            m_brickNumber++;
        }
    }

    public void RemoveBrick(GameObject brick)
    {
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
                if (brick_id != 0) 
                {                    
                    AddBrick (i, j, brick_id);
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
    public Dictionary<Brick.BrickType,Transform> m_brickMap;
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
        m_brickMap = new Dictionary<Brick.BrickType, Transform> ();
        foreach(KeyValuePair<Brick.BrickType, string> entry in m_brickMapDef)
        {
            // do something with entry.Value or entry.Key
            Transform brick = (Transform)Resources.Load(entry.Value, typeof(Transform));
            if (brick == null) 
            {
                Debug.LogError ("Prefab not found: " + entry.Value);
                continue;
            }
            m_brickMap.Add (entry.Key, brick);
        }            
    }

}