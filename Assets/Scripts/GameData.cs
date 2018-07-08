[System.Serializable]
public class GameData
{
    public int m_totalScore;
    public int m_currentLevel;
    public int m_lives;

    public GameData()
    {
        m_totalScore = 0;
        m_currentLevel = 1 ;
        m_lives = 3;
    }
}