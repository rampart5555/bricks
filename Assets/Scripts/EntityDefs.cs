using System;
using UnityEngine;
using System.Collections.Generic;

public class EntityDefs
{
    public enum BrickType : uint  // your custom enumeration
    {
        brick_none,
        brick_brown, 
        brick_red, 
        brick_turquoise,
        brick_white
    };

    private static EntityDefs instance=null;

    private Dictionary <BrickType,string> m_materialMap = new Dictionary<BrickType,String>()
    {
        { BrickType.brick_brown,"brick_brown"},
        { BrickType.brick_red,"brick_red"},
        { BrickType.brick_turquoise,"brick_turquoise"},
        { BrickType.brick_white,"brick_white"}

    };
    private Dictionary <BrickType,Material> m_materialList=new Dictionary<BrickType,Material>();

    public static EntityDefs Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new EntityDefs();
            }

            return instance;
        }
    }
    public Material GetMaterial(BrickType btype)
    {
        Material mat = null;
        if (!m_materialList.ContainsKey(btype)) 
        {
            string mat_str = "Materials/" + m_materialMap [btype];
            mat = (Material)Resources.Load (mat_str);
            if (mat == null) 
            {
                Debug.Log ("Material not found " + mat_str);
                return null;
            }                
        }  
        else
            mat=m_materialList[btype];
        return mat;
    }    
}



