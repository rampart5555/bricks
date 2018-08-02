﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GamePortal : MonoBehaviour {

    Animation m_animation;

	// Use this for initialization
	void Start () 
    {        
        CreateAnimation();
        StartGame();
	}

    void StartGame()
    {        
        AnimationState state= m_animation["game_portal_open"];
        state.time = 0;
        state.speed = 1;
        m_animation.Play("game_portal_open");
    }

    void NextLevel()
    {
        AnimationState state= m_animation["game_portal_open"];
        state.time = state.length;
        state.speed = -1;
        m_animation.Play("game_portal_open");
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

    void CreateAnimation()
    {   
        Vector3 go_from;

        float dist=4.0f;
        AnimationClip clip = new AnimationClip();
        clip.name = "game_portal_open";
        clip.legacy = true;
        go_from = GameObject.Find("game_portal_top").transform.position;
        float[] gpo_top_keys = {
                0.0f, go_from.x, go_from.y, go_from.z,
                1.0f, go_from.x, go_from.y + dist, go_from.z,
            };
        CreateAnimationCurve(clip,"game_portal_top",gpo_top_keys);
        go_from = GameObject.Find("game_portal_bottom").transform.position;
        float[] gpo_bottom_keys = {
                0.0f, go_from.x, go_from.y, go_from.z,
                1.0f, go_from.x, go_from.y - dist, go_from.z,
            };
        CreateAnimationCurve(clip,"game_portal_bottom",gpo_bottom_keys);
        m_animation = GetComponent<Animation>();
        m_animation.AddClip(clip,clip.name);
    }
}
