using System.Collections.Generic;
using UnityEngine;
using static ShootSystemManager;

public class LevelData : MonoBehaviour
{
    private int m_CurrentRoom;

    [Header("PLAYER STATS")]
    [SerializeField] float m_Grade; //30%bullets used, death 50%, time 20%
    [SerializeField] int m_BulletsUsed;
    [SerializeField] int m_PlayerKills;
    [SerializeField] int m_PlayerDeath;
    [SerializeField] float m_CurrTimeLevel;
    [SerializeField] int m_CurrentLevel;

     float m_GradeCopy; 
     int   m_BulletsUsedCopy;
     int   m_PlayerKillsCopy;
     int   m_PlayerDeathCopy;
     float m_CurrTimeLevelCopy;
     int   m_CurrentLevelCopy;


    [Header("Score Board")]
    public float m_MaxGradeTime;
    public float m_MaxGradeDeaths;
    public float m_MaxGradeBulletUsed;
    float m_MaxGradeTimeCopy;
    float m_MaxGradeDeathsCopy;
    float m_MaxGradeBulletUsedCopy;



    [SerializeField] float m_PercentsBulletUsed = 0.3f, m_PercentTimer = 0.2f, m_PercentDeaths = 0.5f;
    float m_PercentsBulletUsedCopy = 0.3f, m_PercentTimerCopy = 0.2f, m_PercentDeathsCopy = 0.5f;
    [HideInInspector] public float m_MaxGrade;
    float m_MaxGradeCopy;
    [Header("Score Board")]
    [SerializeField] List<string> m_NameLevel = new List<string>();
    List<string> m_NameLevelCopy = new List<string>();
    [SerializeField] BulletType[] m_BulletsSelected = new BulletType[3];
    BulletType[] m_BulletsSelectedCopy = new BulletType[3];

    public bool m_GameStarted;
    public string[] m_SceneNames;
    public int m_CurrentLevelPlayed;

    bool m_GameStartedCopy;
    string[] m_SceneNamesCopy;
    int m_CurrentLevelPlayedCopy;
    public void CopiStartVar()
    {
        m_GradeCopy              = m_Grade;
        m_BulletsUsedCopy        = m_BulletsUsed;
        m_PlayerKillsCopy        = m_PlayerKills;
        m_PlayerDeathCopy        = m_PlayerDeath;
        m_CurrTimeLevelCopy      = m_CurrTimeLevel;
        m_CurrentLevelCopy       = m_CurrentLevel;

        m_MaxGradeTimeCopy       = m_MaxGradeTime;
        m_MaxGradeDeathsCopy     = m_MaxGradeDeaths;
        m_MaxGradeBulletUsedCopy = m_MaxGradeBulletUsed;

        m_PercentsBulletUsedCopy = m_PercentsBulletUsed;
        m_PercentTimerCopy       = m_PercentTimer;
        m_PercentDeathsCopy      = m_PercentDeaths;

        m_MaxGradeCopy           = m_MaxGrade;
        m_NameLevelCopy          = m_NameLevel;
        m_BulletsSelectedCopy    = m_BulletsSelected;

        m_GameStartedCopy        = m_GameStarted;
        m_SceneNamesCopy         = m_SceneNames;
        m_CurrentLevelPlayedCopy = m_CurrentLevelPlayed;
    }
    public void RestartVariables()
    {
        m_Grade = m_GradeCopy;
        m_BulletsUsed             =m_BulletsUsedCopy                ;
        m_PlayerKills            =m_PlayerKillsCopy                 ;
        m_PlayerDeath            =m_PlayerDeathCopy                 ;
        m_CurrTimeLevel           =m_CurrTimeLevelCopy              ;
        m_CurrentLevel         =m_CurrentLevelCopy                  ;
                                  
        m_MaxGradeTime             =m_MaxGradeTimeCopy              ;
        m_MaxGradeDeaths           =m_MaxGradeDeathsCopy            ;
        m_MaxGradeBulletUsed       =m_MaxGradeBulletUsedCopy        ;
                                
        m_PercentsBulletUsed       =m_PercentsBulletUsedCopy        ;
        m_PercentTimer          =m_PercentTimerCopy                 ;
        m_PercentDeaths           =m_PercentDeathsCopy              ;
           
        m_MaxGrade             = m_MaxGradeCopy                      ;
        m_NameLevel               = m_NameLevelCopy                  ;
        //m_BulletsSelected         = m_BulletsSelectedCopy            ;
        for (int i = 0; i < m_BulletsSelected.Length; i++)
        {
            m_BulletsSelected[i] = m_BulletsSelectedCopy[i];
        }
                                
        m_GameStarted            =m_GameStartedCopy                 ;
        m_SceneNames            =m_SceneNamesCopy                   ;
        m_CurrentLevelPlayed = m_CurrentLevelPlayedCopy;
    }
    private void Awake()
    {
        if (GameManager.GetManager().GetLevelData() == null)
        {
            GameManager.GetManager().SetLevelData(this);
           // DontDestroyOnLoad(gameObject);
            CopiStartVar();
        }
        else if (GameManager.GetManager().GetLevelData() != this)
        {
            Destroy(gameObject);
        }

        m_MaxGrade = m_MaxGradeTime * m_PercentTimer + m_MaxGradeBulletUsed * m_PercentsBulletUsed + m_MaxGradeDeaths * m_PercentDeaths;
    }

    private void Update()
    {
        if (!m_GameStarted)
            return;

        m_CurrTimeLevel += Time.deltaTime;
    }

    public void SaveDataPlayerBullets(BulletType[] savedBullets) { m_BulletsSelected = savedBullets; }
    public BulletType[] LoadDataPlayerBullets() { return m_BulletsSelected; }

    public void SaveDeathsPlayer() { m_PlayerDeath++; }
    public int LoadDeathsPlayer() { return m_PlayerDeath; }

    public void SaveBulletsUsed() { m_BulletsUsed++; }
    public int LoadBulletsUsed() { return m_BulletsUsed; }

    public float LoadTotalTime() { return m_CurrTimeLevel; }
    public void ResetTotalTime() 
    {
        m_GameStarted = false;
        m_CurrTimeLevel = 0; 
    }

    public float LoadGrade()
    {
        float l_Average = m_BulletsUsed * m_PercentsBulletUsed + m_PlayerDeath * m_PercentDeaths + (LoadTotalTime() / 60) * m_PercentTimer;
        return m_Grade = Mathf.Clamp(l_Average, 0, m_MaxGrade);
    }

    public void SaveRoom(int i) { m_CurrentLevel = i; }
    public int LoadRoom() { return m_CurrentRoom; }

    public void SaveLevel(int i) { m_CurrentRoom = i; }
    public string LoadLevelName() { return m_NameLevel[m_CurrentLevel]; }

    public void SaveKills() { m_PlayerKills++; }
    public int LoadKills() { return m_PlayerKills; }
}
