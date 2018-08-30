using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour
{    
    public  LevelEntities.PowerupType m_powerupType{ set; get; }

    void Start()
    {
	
    }	

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "wall_bottom")
        {
            gameObject.SetActive(false);
        }
    }
}

