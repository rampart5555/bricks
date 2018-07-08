using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameConfig : MonoBehaviour
{
    public GameData m_gameData;


    public void SaveFile()
    {
        string destination = Application.persistentDataPath + "/config.dat";
        FileStream file;

        if (File.Exists (destination))
            file = File.OpenWrite (destination);
        else 
            file = File.Create (destination);        

        m_gameData = new GameData();
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, m_gameData);
        file.Close();
    }

    public void LoadFile()
    {
        string destination = Application.persistentDataPath + "/config.dat";
        FileStream file;

        if(File.Exists(destination)) 
            file = File.OpenRead(destination);
        else
        {
            Debug.LogErrorFormat("File not found {0}", destination);
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        m_gameData= (GameData) bf.Deserialize(file);
        file.Close();
    }
}

