using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour {

    public Button m_start;
	// Use this for initialization
	void Start () 
    {
        Button but=m_start.GetComponent<Button>();
        but.onClick.AddListener(delegate {StartGame("Start Game"); });
	}
    void StartGame(string msg)
    {
        //Output this to console when the Button is clicked
        //SceneManager.LoadScene("MainScene", LoadSceneMode.Additive);
        SceneManager.LoadScene("MainScene",LoadSceneMode.Single);
    }
}
