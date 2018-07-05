using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameConfig : MonoBehaviour
{
    public int m_totalScore;
    public int m_currentLevel;
    public int m_lives;

    public void SaveFile()
    {
        string destination = Application.persistentDataPath + "/config.dat";
        FileStream file;

        if (File.Exists (destination))
            file = File.OpenWrite (destination);
        else 
        {            
            file = File.Create (destination);
            m_totalScore = 0;
            m_currentLevel = 1;
            m_lives = 3;
        }

        GameData data = new GameData(m_totalScore, m_currentLevel, m_lives);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
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
            Debug.LogError("File not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        GameData data = (GameData) bf.Deserialize(file);
        file.Close();

        m_totalScore = data.m_totalScore;
        m_currentLevel = data.m_currentLevel;
        m_lives = data.m_lives;       
    }
}

