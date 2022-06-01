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
        GameManager.GetManager().GetLevelData().SaveTotalTime();
        m_Minutes = Mathf.Floor(GameManager.GetManager().GetLevelData().LoadTotalTime() / 60);
        m_Seconds = Mathf.RoundToInt(GameManager.GetManager().GetLevelData().LoadTotalTime() % 60);

        m_CurrGrade = GameManager.GetManager().GetLevelData().LoadGrade();


        m_MaxGradeFloat = GameManager.GetManager().GetLevelData().m_MaxGrade;


        if (m_CurrGrade < m_MaxGradeFloat * 1.1)
        {
            m_WordGrade = "A";
        } else if ((m_CurrGrade >= m_MaxGradeFloat * 1.1f && m_CurrGrade <= m_MaxGradeFloat * 1.3f))
        {
            m_WordGrade = "B";
        }
        else if ((m_CurrGrade > m_MaxGradeFloat * 1.3f && m_CurrGrade >= m_MaxGradeFloat * 1.5f))
        {
            m_WordGrade = "C";
        }
        else if ((m_CurrGrade > m_MaxGradeFloat * 1.5f && m_CurrGrade >= m_MaxGradeFloat * 1.7f))
        {
            m_WordGrade = "E";
        }
        else
            m_WordGrade = "F";

        m_Grade.text = " Grade: " + m_WordGrade; ///*Mathf.Round(m_CurrGrade)*/ + "%";
        m_Bullets.text = " Bullets Used: " + GameManager.GetManager().GetLevelData().LoadBulletsUsed();
        m_Deaths.text = " Deaths: " + GameManager.GetManager().GetLevelData().LoadDeathsPlayer();
        m_Kills.text = GameManager.GetManager().GetLevelData().LoadKills().ToString()+" Kills";
        m_TotalTime.text = " Total Time: " + m_Minutes + " minutes and " + m_Seconds + " second(s)";
        m_CurrentLevel.text = " Level: " + GameManager.GetManager().GetLevelData().LoadLevelName();
    }
}
