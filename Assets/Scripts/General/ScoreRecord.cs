using UnityEngine;
using TMPro;

public class ScoreRecord : MonoBehaviour
{
    public TMP_Text m_Grade;
    public TMP_Text m_Bullets;
    public TMP_Text m_Deaths;
    public TMP_Text m_TotalTime;
    public TMP_Text m_CurrentLevel;


    private float minutes, seconds;
    public void Start()
    {

         minutes = Mathf.Floor(GameManager.GetManager().GetLevelData().LoadTotalTime() / 60);
        seconds = Mathf.RoundToInt(GameManager.GetManager().GetLevelData().LoadTotalTime() % 60);


        m_Grade.text = " Grade: " + Mathf.Round(GameManager.GetManager().GetLevelData().LoadGrade()) + "%";
        m_Bullets.text = " Bullets Used: " + GameManager.GetManager().GetLevelData().LoadBulletsUsed();
        m_Deaths.text = " Deaths: " + GameManager.GetManager().GetLevelData().LoadDeathsPlayer();
        m_TotalTime.text = " Total Time: " + minutes + " minutes and " + seconds + " second(s)";
        m_CurrentLevel.text = " Level: " + GameManager.GetManager().GetLevelData().LoadLevelName();
    }
}
