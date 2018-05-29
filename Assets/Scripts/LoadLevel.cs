using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class LoadLevel : MonoBehaviour {

	// Use this for initialization
    private Dictionary<Brick.BrickType,Transform> brick_map;

	void Start () 
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(Load);

        brick_map=new Dictionary<Brick.BrickType,Transform>();
        Transform brick;
        brick = LoadPrefab("Prefab/brick_brown");
        brick_map.Add (Brick.BrickType.brick_brown, brick);

        brick = LoadPrefab("Prefab/brick_red");
        brick_map.Add (Brick.BrickType.brick_red, brick);

        brick = LoadPrefab("Prefab/brick_turquoise");
        brick_map.Add (Brick.BrickType.brick_turquoise, brick);

        brick = LoadPrefab("Prefab/brick_white");
        brick_map.Add (Brick.BrickType.brick_white, brick);
	}

    private Transform LoadPrefab(string path)
    {        
        Transform brick = (Transform)Resources.Load(path,typeof(Transform));            
        if (brick == null) 
        {
            Debug.LogError ("Prefab not found: " + path);
            return null;
        }
        return brick;
    }

    public void Load()
    {
        TextAsset m_textAsset = Resources.Load("Levels/level_01") as TextAsset;     
        if (m_textAsset == null) 
        {            
            Debug.Log ("File not found\n");
            return;
        }        
        ParseTMX tmx = new ParseTMX ();
        tmx.ParseFile (m_textAsset.text);
        for (int j = 0; j < tmx.m_height; j++)
            for (int i = 0; i < tmx.m_width; i++) 
            {
                int brick_id=tmx.m_brickData[j * tmx.m_width + i];
                if (brick_id != 0) 
                {
                    float x = 1.2f - i * 0.2f - 0.1f;
                    float y = 1.4f - j * 0.1f - 0.1f;
                    Brick.BrickType brick_type = (Brick.BrickType)brick_id;
                    if (brick_map.ContainsKey (brick_type)) 
                    {
                            Instantiate (brick_map[brick_type], new Vector3 (x, y, 0.07f), Quaternion.identity);
                    }
                    
                }
            }
        GameObject go_ball = GameObject.FindWithTag("Ball");
        if (go_ball != null) 
        {
            Ball ball = go_ball.GetComponent<Ball> ();
            ball.SetSpeed (1.0f, 1.0f);
        }

    }
		
}
