using UnityEngine;
using TMPro;

public class ScoreRecord : MonoBehaviour
{
    public TMP_Text m_Grade;
    public TMP_Text m_Bullets;
    public TMP_Text m_Kills;
    public TMP_Text m_Deaths;
    public TMP_Text m_TotalTime;
    public TMP_Text m_CurrentLevel;

    private float m_Minutes, m_Seconds;
    private float m_MaxGradeFloat;
    private float m_CurrGrade;
    private string m_WordGrade;
    
    public void UpdateRecord()
    {
        GameManager.GetManager().GetLevelData().m_GameStarted = false;

        m_Minutes = Mathf.Floor(GameManager.GetManager().GetLevelData().LoadTotalTime() / 60);
        m_Seconds = Mathf.RoundToInt(GameManager.GetManager().GetLevelData().LoadTotalTime() % 60);

        m_CurrGrade = GameManager.GetManager().GetLevelData().LoadGrade();
        m_MaxGradeFloat = GameManager.GetManager().GetLevelData().m_MaxGrade;

        GetWordGraded();

        m_Grade.text =  m_WordGrade; 
        m_Bullets.text = GameManager.GetManager().GetLevelData().LoadBulletsUsed() + " bullet(s) used";
        m_Deaths.text = GameManager.GetManager().GetLevelData().LoadDeathsPlayer()+ " death(s)";
        m_Kills.text = GameManager.GetManager().GetLevelData().LoadKills().ToString()+" Kill(s)";
        m_TotalTime.text = m_Minutes + " minutes and " + m_Seconds + " second(s) played";
        m_CurrentLevel.text = GameManager.GetManager().GetLevelData().LoadLevelName();
    }

    private void GetWordGraded()
    {
        if (m_CurrGrade < m_MaxGradeFloat * 1.1)
        {
            m_WordGrade = "Grade: A";
        }
        else if ((m_CurrGrade >= m_MaxGradeFloat * 1.1f && m_CurrGrade <= m_MaxGradeFloat * 1.3f))
        {
            m_WordGrade = "Grade: B";
        }
        else if ((m_CurrGrade > m_MaxGradeFloat * 1.3f && m_CurrGrade >= m_MaxGradeFloat * 1.5f))
        {
            m_WordGrade = "Grade: C";
        }
        else if ((m_CurrGrade > m_MaxGradeFloat * 1.5f && m_CurrGrade >= m_MaxGradeFloat * 1.7f))
        {
            m_WordGrade = "Grade: E";
        }
        else
            m_WordGrade = "Grade: F";
    }
    public void BackMenu()
    {
        GameManager.GetManager().GetSceneLoader().LoadLevel(0);
    }
}
