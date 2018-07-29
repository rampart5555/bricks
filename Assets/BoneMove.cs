using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class BoneMove : MonoBehaviour {

	// Use this for initialization
    public GameObject rootBone;
    public GameObject target;
    GameObject sphere_left;
    float speed;
    bool isMoving;
    Animation m_anim;
    public void CreateAnimationCurve()
    {
        m_anim = sphere_left.GetComponent<Animation>();

        // create a new AnimationClip
        AnimationClip clip = new AnimationClip();
        clip.legacy = true;
        clip.name = "sphere_left_move";

        // create a curve to move the GameObject and assign to the clip
        // X
        Keyframe[] keys_X;
        keys_X = new Keyframe[3];
        keys_X[0] = new Keyframe(0.0f, sphere_left.transform.localPosition.x);
        keys_X[1] = new Keyframe(3.0f, 1.5f);
        keys_X[2] = new Keyframe(5.0f, 0.0f);
        AnimationCurve curve_X = new AnimationCurve(keys_X);
        clip.SetCurve("", typeof(Transform), "localPosition.x", curve_X);
        //Y axis
        Keyframe[] keys_Y;
        keys_Y = new Keyframe[1];
        keys_Y[0] = new Keyframe(0.0f, sphere_left.transform.localPosition.y);
        //keys_Y[1] = new Keyframe(10.0f, 1.5f);
        //keys_Y[2] = new Keyframe(20.0f, 0.0f);
        AnimationCurve curve_Y = new AnimationCurve(keys_Y);
        clip.SetCurve("", typeof(Transform), "localPosition.y", curve_Y);


        // now animate the GameObject
        AssetDatabase.CreateAsset(m_anim, "Assets/anim.anim");
        m_anim.AddClip(clip, clip.name);

        //AssetDatabase.SaveAssets();

    }

	void Start () 
    {
        sphere_left = GameObject.Find("sphere_left");   
        CreateAnimationCurve();
        //speed = 2.0f;
        //sphere_left = rootBone.transform.Find("sphere_left");	
        //isMoving = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            m_anim["sphere_left_move"].speed = 1;
            m_anim["sphere_left_move"].time = 0;
            m_anim.Play("sphere_left_move");
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            m_anim["sphere_left_move"].speed = -1;
            m_anim["sphere_left_move"].time = m_anim["sphere_left_move"].length;
            m_anim.Play("sphere_left_move");
        }
            
        /*
        if (isMoving)
        {
            float step = speed * Time.deltaTime;
            // Move our position a step closer to the target.
            sphere_left.position = Vector3.MoveTowards(sphere_left.position, target.transform.position, step);
            if (sphere_left.position == target.transform.position)
            {
                Debug.Log("Moving coplete");
                isMoving = false;
            }
        }
        */
	}
}
