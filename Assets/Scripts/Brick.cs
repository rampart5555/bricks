using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Brick : MonoBehaviour 
{
    LevelEntities.BrickType m_brickType;
    public  LevelEntities.PowerupType m_powerupType{ set; get; }
    int m_value;
    int m_hits;

    void Start()
    {
        
    }

    public void SetBrickType(LevelEntities.BrickType btype)
    {
        m_brickType = btype;
        switch (btype)
        {
            case LevelEntities.BrickType.BRICK_BLUE:
                m_value = 10;
                m_hits = 2;
                break;
            case LevelEntities.BrickType.BRICK_BROWN:
                m_value = 10;
                m_hits = 2;
                break;              
            case LevelEntities.BrickType.BRICK_RED:
                m_value = 10;
                m_hits = 2;
                break;
            case LevelEntities.BrickType.BRICK_WHITE:
                m_value = 10;
                m_hits = 2;
                break;
            default:
                m_value = 0;
                m_hits = 2;
                break;
        }
    }

    public int IsHit()
    {
        m_hits--;
        return m_hits;            
    }
    public int GetValue()
    {
        return m_value;
    }
}
