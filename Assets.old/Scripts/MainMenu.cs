using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetMouseButtonUp(0)) 
        {                        
            //Debug.Log ("Mouse click");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast (ray, out hit)) 
            {                   
                Debug.Log(hit.transform.gameObject.name);
                SceneManager.LoadScene ("scene", LoadSceneMode.Single);
                    
            } 
        }	
	}
}
