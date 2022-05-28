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
    public void Start()
    {

        m_Minutes = Mathf.Floor(GameManager.GetManager().GetLevelData().LoadTotalTime() / 60);
        m_Seconds = Mathf.RoundToInt(GameManager.GetManager().GetLevelData().LoadTotalTime() % 60);


        m_Grade.text = " Grade: " + Mathf.Round(GameManager.GetManager().GetLevelData().LoadGrade()) + "%";
        m_Bullets.text = " Bullets Used: " + GameManager.GetManager().GetLevelData().LoadBulletsUsed();
        m_Deaths.text = " Deaths: " + GameManager.GetManager().GetLevelData().LoadDeathsPlayer();
        m_Kills.text = " x"+GameManager.GetManager().GetLevelData().LoadKills().ToString();
        m_TotalTime.text = " Total Time: " + m_Minutes + " minutes and " + m_Seconds + " second(s)";
        m_CurrentLevel.text = " Level: " + GameManager.GetManager().GetLevelData().LoadLevelName();
    }
}
