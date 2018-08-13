using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelEnvironment: MonoBehaviour
{
    Animation m_animation;
    AnimationClip m_doorLeft;
    GameController m_gameController;

    GameObject m_ballMesh;
    GameObject m_paddleMesh;
    GameObject [] m_paddleMeshSlots;

    GameController.GCState m_state;

    void Awake()
    {
        //AnimationLevelStart();
        //AnimatioPaddleLost(1);
        //AnimatioPaddleLost(2);
        //AnimatioPaddleLost(3);


        //AnimationLevelComplete();
        m_ballMesh = transform.Find("ball_mesh").gameObject;
        m_paddleMesh = transform.Find("paddle_mesh_0").gameObject;
        m_paddleMeshSlots = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            m_paddleMeshSlots[i] = transform.Find(string.Format("paddle_mesh_{0}", i + 1)).gameObject;
        }
    }

    void Start()
    {          
        GameObject gcObj = GameObject.FindGameObjectWithTag("GameController");
        m_gameController=gcObj.GetComponent<GameController>();
    }

    public void DisableGamePortal()
    {
        Transform tr;
        tr= transform.Find("game_portal_bottom");
        tr.gameObject.SetActive(false);
        tr= transform.Find("game_portal_top");
        tr.gameObject.SetActive(false);
        //for (int i = 0; i < go_list.Length; i++)
        //    go_list[i].SetActive(false);
    }
        
    public void EnableEntities()
    {
        m_ballMesh.SetActive(true);
    }

    public void PlayAnimation(string anim_name,GameController.GCState state)
    {            
        m_animation.Play(anim_name);  
        m_state = state;
    }

    public void AnimationCompleted(string anim_name)
    {
        Debug.LogFormat("LevelEnvironment.AnimationCompleted:{0}",anim_name);
        if (anim_name == "paddle_restore_3")
        {
            m_paddleMeshSlots[2].SetActive(false);
            m_gameController.OnStateExit(m_state);
            m_state = GameController.GCState.NONE;
        }
        else if (anim_name == "paddle_restore_2")
        {
            m_paddleMeshSlots[1].SetActive(false);
            m_gameController.OnStateExit(m_state);
            m_state = GameController.GCState.NONE;
        }
        else if (anim_name == "paddle_restore_1")
        {
            m_paddleMeshSlots[0].SetActive(false);
            m_gameController.OnStateExit(m_state);
            m_state = GameController.GCState.NONE;
        }
        else
        {
            m_gameController.OnStateExit(m_state);
            m_state = GameController.GCState.NONE;
        }
    }

    public void CreateAnimationCurve(AnimationClip clip, string obj_name, float[] cp_list)
    {                                
        string[] axis_pos_str = { "localPosition.x", "localPosition.y", "localPosition.z" };
        for (int i = 0; i < 3; i++)
        {            
            //Keyframe[] keys = new Keyframe[cp_list.Length];
            //int index = 0;
            AnimationCurve curve = new AnimationCurve();
            for (int j = 0; j < cp_list.Length; j+=4)
            {                
                curve.AddKey(new Keyframe(cp_list[j],cp_list[j+i+1]));
            }                
            clip.SetCurve(obj_name, typeof(Transform), axis_pos_str[i], curve);
        }       
    }

    public void AddAnimationClip(string clip_name, float event_time, Dictionary<string,float[]> anim_keys)
    {
        AnimationClip clip = new AnimationClip();
        clip.legacy=true;
        clip.name=clip_name;
        foreach(KeyValuePair<string, float[]> entry in anim_keys)
        {
            CreateAnimationCurve(clip,entry.Key, entry.Value);
        }
        AnimationEvent evt;
        evt = new AnimationEvent();
        evt.time = clip.length-0.1f;
        evt.functionName = "AnimationCompleted";
        evt.stringParameter = clip_name;
        clip.AddEvent(evt);
        m_animation= GetComponent<Animation>();
        m_animation.AddClip(clip, clip_name);
    }

    public void AnimationLevelComplete()
    {        
        Dictionary<string,float[]> anim_dict = new Dictionary<string,float[]>();
        Vector3 go_from;
        go_from =  GameObject.Find("door_right").transform.localPosition;
        float[] door_right_keys = 
            {
                0.0f, go_from.x, go_from.y, go_from.z,
                5.0f, go_from.x, go_from.y+0.5f, go_from.z,
            };
        anim_dict.Add("door_right", door_right_keys);
        AddAnimationClip("level_clear", 4.95f, anim_dict);

    }



    public void AnimationLevelStart()
    {   
        Dictionary<string,float[]> anim_dict = new Dictionary<string,float[]>();
        Vector3 go_from,go_to;

        /* door left*/
        go_from =  GameObject.Find("door_left").transform.localPosition;
        float[] door_left_keys = 
        {
                0.0f, go_from.x, go_from.y, go_from.z,
                1.0f, go_from.x, go_from.y+0.5f, go_from.z,
                1.5f, go_from.x, go_from.y+0.5f, go_from.z, 
                2.0f, go_from.x, go_from.y, go_from.z
        };
        anim_dict.Add("door_left", door_left_keys);

        /*paddle*/
        go_from = GameObject.Find("paddle_slot_0").transform.localPosition;
        go_to = GameObject.Find("paddle_start_pos").transform.localPosition;
        float [] paddle_mesh_0_keys={
            1.0f, go_from.x, go_from.y, go_from.z,
            1.5f, go_to.x, go_to.y, go_to.z
        };               
        anim_dict.Add("paddle_mesh_0", paddle_mesh_0_keys);

        /*ball*/
        go_from = GameObject.Find("ball_slot_0").transform.localPosition;
        go_to = GameObject.Find("ball_start_pos").transform.localPosition;
        float [] ball_mesh_keys={
            1.5f, go_from.x, go_from.y, go_from.z,
            1.9f, go_to.x, go_to.y, go_to.z
        }; 

        anim_dict.Add("ball_mesh", ball_mesh_keys);

        AddAnimationClip("level_start", 1.95f, anim_dict);

        //AssetDatabase.CreateAsset(clip, "Assets/anim.anim");
        //AssetDatabase.SaveAssets();

    }

    public void AnimatioPaddleLost(int paddle_slot_index)
    {
        Dictionary<string,float[]> anim_dict = new Dictionary<string,float[]>();
        Vector3 go_from,go_to;

        /*paddle restore index*/
        string paddle_slot = string.Format("paddle_slot_{0}", paddle_slot_index);
        string paddle_mesh = string.Format("paddle_mesh_{0}", paddle_slot_index);
        string paddle_restore = string.Format("paddle_restore_{0}", paddle_slot_index);
        go_from =  GameObject.Find(paddle_slot).transform.localPosition;
        go_to = GameObject.Find("paddle_start_pos").transform.localPosition;
        float [] paddle_mesh_keys={
            0.0f, go_from.x, go_from.y, go_from.z,
            0.5f, go_from.x, go_from.y, go_from.z - 0.5f,
            1.5f, go_to.x, go_to.y, go_to.z - 0.5f,
            2.0f, go_to.x, go_to.y, go_to.z
        }; 
        anim_dict.Add(paddle_mesh, paddle_mesh_keys);
        /*ball*/
        go_from = GameObject.Find("ball_slot_0").transform.localPosition;
        go_to = GameObject.Find("ball_start_pos").transform.localPosition;
        float [] ball_mesh_keys={
            2.0f, go_from.x, go_from.y, go_from.z,
            2.5f, go_to.x, go_to.y, go_to.z
        }; 
        anim_dict.Add("ball_mesh", ball_mesh_keys);

        AddAnimationClip(paddle_restore, 2.45f, anim_dict);
    } 
}
