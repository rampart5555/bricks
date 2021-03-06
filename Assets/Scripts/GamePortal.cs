﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GamePortal : MonoBehaviour {

    #if false
    Animation m_animation;
    GameController m_gameController;
    GameController.GCState m_state;

    void Awake()
    {
        Debug.Log("GamePortal.Awake");
        CreateAnimationGamePortalOpen();
        CreateAnimationGamePortalClose();

    }
	// Use this for initialization
	void Start () 
    {        
        Debug.Log("GamePortal.Start");
        GameObject gcObj = GameObject.FindGameObjectWithTag("GameController");
        m_gameController=gcObj.GetComponent<GameController>();

	}

    public void PlayAnimation(string anim_name,GameController.GCState state)
    {
        m_state = state;
        m_animation.Play(anim_name);
    }

    public void AnimationComplete(string animation)
    {
        Debug.LogFormat("GamePortal.AnimationComplete {0}",animation);
        m_gameController.OnStateExit(m_state);
        m_state = GameController.GCState.NONE;
    }

    void CreateAnimationCurve(AnimationClip clip, string obj_name, float[] cp_list)
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

    void CreateAnimationGamePortalOpen()
    {   
        Vector3 go_from;

        float dist=4.0f;
        AnimationClip clip = new AnimationClip();
        clip.name = "game_portal_open";
        clip.legacy = true;
        go_from = GameObject.Find("game_portal_top").transform.localPosition;
        float[] gpo_top_keys = {
                0.0f, go_from.x, go_from.y, go_from.z,
                0.5f, go_from.x, go_from.y, go_from.z,
                3.0f, go_from.x, go_from.y + dist, go_from.z,
            };
        CreateAnimationCurve(clip,"game_portal_top",gpo_top_keys);
        go_from = GameObject.Find("game_portal_bottom").transform.localPosition;
        float[] gpo_bottom_keys = {
                0.0f, go_from.x, go_from.y, go_from.z,
                0.5f, go_from.x, go_from.y, go_from.z,
                3.0f, go_from.x, go_from.y - dist, go_from.z,
            };
        CreateAnimationCurve(clip,"game_portal_bottom",gpo_bottom_keys);

        AnimationEvent evt;
        evt = new AnimationEvent();
        evt.time = clip.length-0.1f;
        evt.functionName = "AnimationComplete";
        evt.stringParameter = clip.name;
        clip.AddEvent(evt);

        m_animation = GetComponent<Animation>();
        m_animation.AddClip(clip,clip.name);
    }
    void CreateAnimationGamePortalClose()
    {   
        Vector3 go_from;

        float dist=4.0f;
        AnimationClip clip = new AnimationClip();
        clip.name = "game_portal_close";
        clip.legacy = true;
        go_from = GameObject.Find("game_portal_top").transform.localPosition;
        float[] gpo_top_keys = {
            0.0f, go_from.x, go_from.y+dist, go_from.z,
            0.5f, go_from.x, go_from.y+dist, go_from.z,
            3.0f, go_from.x, go_from.y, go_from.z,
        };
        CreateAnimationCurve(clip,"game_portal_top",gpo_top_keys);
        go_from = GameObject.Find("game_portal_bottom").transform.localPosition;
        float[] gpo_bottom_keys = {
            0.0f, go_from.x, go_from.y-dist, go_from.z,
            0.5f, go_from.x, go_from.y-dist, go_from.z,
            3.0f, go_from.x, go_from.y, go_from.z,
        };
        CreateAnimationCurve(clip,"game_portal_bottom",gpo_bottom_keys);

        AnimationEvent evt;
        evt = new AnimationEvent();
        evt.time = clip.length-0.1f;
        evt.functionName = "AnimationComplete";
        evt.stringParameter = clip.name;
        clip.AddEvent(evt);

        m_animation = GetComponent<Animation>();
        m_animation.AddClip(clip,clip.name);
    }
#endif
}
