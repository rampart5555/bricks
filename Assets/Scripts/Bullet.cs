using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    LevelEntities m_levelEntities;
    void Start()
    {
        GameObject le=GameObject.FindWithTag("LevelEntities");
        m_levelEntities=le.GetComponent<LevelEntities>();
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.LogFormat("Bullet.OnCollisionEnter2D {0}",col.gameObject.name);
        if (col.gameObject.tag == "Brick") 
        {   
            m_levelEntities.RemoveBullet(gameObject);
        }
        else if (col.gameObject.name == "wall_top")
        {                       
            m_levelEntities.RemoveBullet(gameObject);
        }
    }
}

