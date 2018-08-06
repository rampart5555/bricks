using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelEnvironment: MonoBehaviour
{
    Animation m_animation;
    AnimationClip m_doorLeft;
    GameController m_gameController;

    void Awake()
    {
        AnimationLevelStart();
        AnimatioPaddleLost(3);
        AnimationLevelComplete();
    }

    void Start()
    {          
        GameObject gcObj = GameObject.FindGameObjectWithTag("GameController");
        m_gameController=gcObj.GetComponent<GameController>();
    }

    public void DisablePaddleBall()
    {
        Debug.Log("LevelEnvironment.DisablePaddleBall");
        Transform mesh_tr;
        mesh_tr = transform.Find("ball_mesh");
        mesh_tr.gameObject.SetActive(false);
        mesh_tr=transform.Find("paddle_mesh_0");
        mesh_tr.gameObject.SetActive(false);
    }

    public void PlayAnimation(string anim_name)
    {       
        //AnimationState m_state = m_animation["level_start"];
        //m_animation.Play("level_complete");         
        //m_animation.Play("level_start");         
        m_animation.Play(anim_name);         
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
        AddAnimationClip("level_complete", 4.95f, anim_dict);

    }

    public void AnimationCompleted(string anim_name)
    {
        Debug.LogFormat("LevelEnvironment.AnimationCompleted:{0}",anim_name);
        m_gameController.AnimationComplete(anim_name);

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
