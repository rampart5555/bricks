using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour {


    public Button m_start;
    public Button m_nextLevel;
	// Use this for initialization
	void Start () 
    {        
        Button but_1=m_start.GetComponent<Button>();
        but_1.onClick.AddListener(delegate {StartGame("Start game"); });
        //Button but_2 = m_nextLevel.GetComponent<Button>();
        //but_2.onClick.AddListener(delegate {NextLevel("Next Level"); });      
	}

    void StartGame(string msg)
    {
        //Output this to console when the Button is clicked
        //SceneManager.LoadScene("MainScene", LoadSceneMode.Additive);
        SceneManager.LoadScene("MainScene",LoadSceneMode.Single);    

    }
}
