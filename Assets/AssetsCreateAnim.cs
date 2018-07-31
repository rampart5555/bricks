using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/*
{
    {"door_left",{0.0f,i_pos.x, i_pos.y, i_pos.z}},
    {"door_left",{1.0f,t_pos.x, t_pos.y, t_pos.z}},
}
*/
public struct ControlPoint
{    
    
    public float  m_time;
    public float[] vec3;
    public ControlPoint(float time, float x,float y, float z)
    {
        m_time = time;
        vec3 = new float[3];
        vec3[0] = x;
        vec3[1] = y;
        vec3[2] = z;
    }
};

public class AssetsCreateAnim: MonoBehaviour
{
    Animation m_animation;
    AnimationClip m_doorLeft;

    void Start()
    {     
        AnimationLevelStart();
        AnimatioPaddleLost(3);

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            PlayAnimation();
    }
    public void PlayAnimation()
    {       
        //AnimationState m_state = m_animation["level_start"];
        m_animation.Play("paddle_restore_3");         
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

    public void AnimationLevelStart()
    {   
        Dictionary<string,float[]> anim_dict = new Dictionary<string,float[]>();
        Vector3 go_from,go_to;
        /* door left*/
        go_from =  GameObject.Find("door_left").transform.localPosition;
        float[] door_left_keys = 
        {
                0.0f, go_from.x, go_from.y, go_from.z,
                1.0f, go_from.x, go_from.y+2.7f, go_from.z,
                1.5f, go_from.x, go_from.y+2.7f, go_from.z, 
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

        AnimationClip clip = new AnimationClip();
        clip.legacy=true;
        clip.name="level_start";
        foreach(KeyValuePair<string, float[]> entry in anim_dict)
        {
            CreateAnimationCurve(clip,entry.Key, entry.Value);
        }
        m_animation= GetComponent<Animation>();
        m_animation.AddClip(clip, "level_start");

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

        AnimationClip clip = new AnimationClip();
        clip.legacy=true;
        clip.name=paddle_restore;
        foreach(KeyValuePair<string, float[]> entry in anim_dict)
        {
            CreateAnimationCurve(clip,entry.Key, entry.Value);
        }
        m_animation= GetComponent<Animation>();
        m_animation.AddClip(clip,paddle_restore);
    }
}
