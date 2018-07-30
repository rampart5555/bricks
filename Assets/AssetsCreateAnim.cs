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
    public ControlPoint(float time,float x,float y, float z)
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
    bool m_toggle;
    void Start()
    {
        m_toggle = false;
        CreateAnimationCurve();

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            PlayAnimation();
    }
    public void PlayAnimation()
    {
        if (m_toggle == false)
        {
            AnimationState m_state = m_animation["door_left_open"];
            /*
            m_state.time = 0;
            m_state.speed = 1;
            */
            m_animation.Play("door_left_open");         

        }
        else
        {
            AnimationState m_state = m_animation["door_left_close"];
            /*
            m_state.time = m_state.length;
            m_state.speed = -1;
            */
            m_animation.Play("door_left_close");         

        }
        m_toggle = !m_toggle;
    }

    public AnimationClip CreateAnim(string clip_name, GameObject go, List<ControlPoint> cp_list)
    {                        
        AnimationClip clip = new AnimationClip();
        clip.legacy = true;
        clip.name = clip_name;
        string[] axis_pos_str = { "localPosition.x", "localPosition.y", "localPosition.z" };
        for (int i = 0; i < 3; i++)
        {
            Keyframe[] keys = new Keyframe[cp_list.Count];
            for (int j=0;j<cp_list.Count;j++)
            {                
                keys[j] = new Keyframe(cp_list[j].m_time, cp_list[j].vec3[i]);             
            }
            AnimationCurve curve = new AnimationCurve(keys);
            clip.SetCurve(go.name, typeof(Transform), axis_pos_str[i], curve);
        }
        return clip;

    }

    public void CreateAnimationCurve()
    {
        GameObject scene = GameObject.Find("scene_3");
        GameObject m_doorLeft = GameObject.Find("door_left");

        Vector3 pos = m_doorLeft.transform.position;
        List<ControlPoint> cp_list = new List<ControlPoint>()
        {
            new ControlPoint( 0.0f, pos.x, pos.y, pos.z),
            new ControlPoint( 1.0f, pos.x+1.2f, pos.y, pos.z),
            new ControlPoint( 2.0f, pos.x+1.2f, pos.y+1.5f, pos.z)
        };
        AnimationClip clip = CreateAnim("door_left_open", m_doorLeft, cp_list);
        cp_list.Reverse();
        AnimationClip clip_2 = CreateAnim("door_left_close", m_doorLeft, cp_list);
        Debug.Log(clip_2.name);
        // create a new AnimationClip
        m_animation= scene.GetComponent<Animation>();
        m_animation.AddClip(clip_2, clip.name);
        m_animation.AddClip(clip, clip.name);

       
        //AssetDatabase.CreateAsset(clip, "Assets/anim.anim");
        //AssetDatabase.SaveAssets();

    }
}
