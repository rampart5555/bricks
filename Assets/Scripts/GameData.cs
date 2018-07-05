[System.Serializable]
public class GameData
{
    public int m_totalScore;
    public int m_currentLevel;
    public int m_lives;

    public GameData(int totalScore, int currentLevel,int lives)
    {
        m_totalScore = totalScore;
        m_currentLevel = currentLevel;
        m_lives = lives;
    }
}