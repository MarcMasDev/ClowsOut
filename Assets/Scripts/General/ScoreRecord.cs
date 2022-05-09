using UnityEngine;
using TMPro;

public class ScoreRecord : MonoBehaviour
{
    public TMP_Text m_Grade;
    public TMP_Text m_Bullets;
    public TMP_Text m_Deaths;
    public TMP_Text m_TotalTime;
    public TMP_Text m_CurrentLevel;

    public void Start()
    {
        m_Grade.text = " Grade: " + GameManager.GetManager().GetLevelData().LoadGrade() + "%";
        m_Bullets.text = " Bullets Used: " + GameManager.GetManager().GetLevelData().LoadBulletsUsed();
        m_Deaths.text = " Deaths: " + GameManager.GetManager().GetLevelData().LoadDeathsPlayer();
        m_TotalTime.text = " Total Time: " + GameManager.GetManager().GetLevelData().LoadTotalTime();
        m_CurrentLevel.text = " Level: " + GameManager.GetManager().GetLevelData().LoadLevelName();
    }
}
