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


    
    public void UpdateRecord()
    {
        GameManager.GetManager().GetLevelData().SaveTotalTime();
        m_Minutes = Mathf.Floor(GameManager.GetManager().GetLevelData().LoadTotalTime() / 60);
        m_Seconds = Mathf.RoundToInt(GameManager.GetManager().GetLevelData().LoadTotalTime() % 60);
      //  m_MaxGradeFloat = m_MaxGradeBulletUsed * 0.3f + m_MaxGradeDeaths * 0.5f + m_MaxGradeTime * 0.2f;
        //if (m_GradeFloat
        m_Grade.text = " Grade: " + Mathf.Round(GameManager.GetManager().GetLevelData().LoadGrade()) + "%";
        m_Bullets.text = " Bullets Used: " + GameManager.GetManager().GetLevelData().LoadBulletsUsed();
        m_Deaths.text = " Deaths: " + GameManager.GetManager().GetLevelData().LoadDeathsPlayer();
        m_Kills.text = GameManager.GetManager().GetLevelData().LoadKills().ToString()+" Kills";
        m_TotalTime.text = " Total Time: " + m_Minutes + " minutes and " + m_Seconds + " second(s)";
        m_CurrentLevel.text = " Level: " + GameManager.GetManager().GetLevelData().LoadLevelName();
    }
}
