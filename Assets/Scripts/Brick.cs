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
            case LevelEntities.BrickType.BRICK_WHITE:
                m_value = 50;
                m_hits = 1;
                break;

            case LevelEntities.BrickType.BRICK_ORANGE:
                m_value = 60;
                m_hits = 1;
                break;
            case LevelEntities.BrickType.BRICK_LIGHTBLUE:
                m_value = 70;
                m_hits = 1;
                break;              
            case LevelEntities.BrickType.BRICK_GREEN:
                m_value = 80;
                m_hits = 1;
                break;
            case LevelEntities.BrickType.BRICK_RED:
                m_value = 90;
                m_hits = 2;
                break;
            case LevelEntities.BrickType.BRICK_BLUE:
                m_value = 100;
                m_hits = 2;
                break;
            case LevelEntities.BrickType.BRICK_PINK:
                m_value = 110;
                m_hits = 3;
                break;
            case LevelEntities.BrickType.BRICK_YELLOW:
                m_value = 120;
                m_hits = 3;
                break;
            case LevelEntities.BrickType.BRICK_SILVER:
                m_value = 200;
                m_hits = 5;
                break;
            case LevelEntities.BrickType.BRICK_GOLD:
                m_value = 1000;
                m_hits = 8;
                break;
            default:
                m_value = 0;
                m_hits = 1;
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
