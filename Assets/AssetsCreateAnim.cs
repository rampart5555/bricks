using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
    public void PlayAnimation()
    {
        if (m_toggle == false)
        {
            AnimationState m_state = m_animation["door_left_open"];
            m_state.time = 0;
            m_state.speed = 1;
            m_animation.Play("door_left_open");         
        }
        else
        {
            AnimationState m_state = m_animation["door_left_open"];
            m_state.time = m_state.length;
            m_state.speed = -1;
            m_animation.Play("door_left_open");         

        }
        m_toggle = !m_toggle;
    }
    public void CreateAnimationCurve()
    {
        GameObject scene=GameObject.Find("scene_3");
        GameObject m_doorLeft = GameObject.Find("door_left");
        m_animation= scene.GetComponent<Animation>();
        // create a new AnimationClip
        AnimationClip clip = new AnimationClip();
        clip.legacy = true;
        clip.name = "door_left_open";

        // create a curve to move the GameObject and assign to the clip
        // X

        Keyframe[] keys_X;
        keys_X = new Keyframe[2];
        keys_X[0] = new Keyframe(0.0f, m_doorLeft.transform.localPosition.x);
        keys_X[1] = new Keyframe(2.0f, m_doorLeft.transform.localPosition.x);
        AnimationCurve curve_X = new AnimationCurve(keys_X);
        clip.SetCurve("door_left", typeof(Transform), "localPosition.x", curve_X);
        //Y axis
        Keyframe[] keys_Y;
        keys_Y = new Keyframe[2];
        keys_Y[0] = new Keyframe(0.0f, m_doorLeft.transform.localPosition.y);
        keys_Y[1] = new Keyframe(2.0f, m_doorLeft.transform.localPosition.y+0.3f);
        //keys_Y[1] = new Keyframe(10.0f, 1.5f);
        //keys_Y[2] = new Keyframe(20.0f, 0.0f);
        AnimationCurve curve_Y = new AnimationCurve(keys_Y);
        clip.SetCurve("door_left", typeof(Transform), "localPosition.y", curve_Y);
        //Z axis
        Keyframe[] keys_Z;
        keys_Z = new Keyframe[2];
        keys_Z[0] = new Keyframe(0.0f, m_doorLeft.transform.localPosition.z);
        keys_Z[1] = new Keyframe(2.0f, m_doorLeft.transform.localPosition.z);
        AnimationCurve curve_Z = new AnimationCurve(keys_Z);
        clip.SetCurve("door_left", typeof(Transform), "localPosition.z", curve_Z);

        // now animate the GameObject
        m_animation.AddClip(clip, clip.name);
       
        //AssetDatabase.CreateAsset(clip, "Assets/anim.anim");

        //AssetDatabase.SaveAssets();

    }
}
