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
    [SerializeField] float m_TotalTimeLevel;
    [SerializeField] int m_CurrentLevel;

    [Header("Score Board")]
    public float m_MaxGradeTime;
    public float m_MaxGradeDeaths;
    public float m_MaxGradeBulletUsed;
    [SerializeField] float m_PercentsBulletUsed = 0.3f, m_PercentTimer = 0.2f, m_PercentDeaths = 0.5f;
    [HideInInspector] public float m_MaxGrade;
    [Header("Score Board")]
    [SerializeField] List<string> m_NameLevel = new List<string>();
    [SerializeField] BulletType[] m_BulletsSelected = new BulletType[3];

    [SerializeField] public bool m_GameStarted;

    [Header("OPTIONS VALUE")]
/*    [HideInInspector] */public bool m_Fullscreen;
/*    [HideInInspector] */public int m_ResolutionIndex, m_FPS,/* m_FOV,*/ m_VYsnc;
     public bool m_ResolutionChanged;
    private void Awake()
    {
        if (GameManager.GetManager().GetLevelData() == null)
        {
            GameManager.GetManager().SetLevelData(this);
            DontDestroyOnLoad(gameObject);
        }
        else if (GameManager.GetManager().GetLevelData() != this)
        {
            Destroy(gameObject);
        }

        Screen.fullScreen = m_Fullscreen;
        QualitySettings.vSyncCount = 0;

        m_MaxGrade = m_MaxGradeTime * m_PercentTimer + m_MaxGradeBulletUsed * m_PercentsBulletUsed + m_MaxGradeDeaths * m_PercentDeaths;
    }

    private void Update()
    {
        if (!m_GameStarted)
            return;

        m_CurrTimeLevel += Time.deltaTime;
    }

    public void SaveDataPlayerBullets(BulletType[] savedBullets) { m_BulletsSelected = savedBullets; }//= GameManager.GetManager().GetPlayerBulletManager().m_UpdatableBulletList; }
    public BulletType[] LoadDataPlayerBullets() { return m_BulletsSelected; }

    public void SaveDeathsPlayer() { m_PlayerDeath++; }
    public int LoadDeathsPlayer() { return m_PlayerDeath; }

    public void SaveBulletsUsed() { m_BulletsUsed++; }
    public int LoadBulletsUsed() { return m_BulletsUsed; }

    public void SaveTotalTime() { m_TotalTimeLevel = m_CurrTimeLevel; }
    public float LoadTotalTime() { return m_TotalTimeLevel; }
    public void ResetTotalTime() { m_TotalTimeLevel = 0; }

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


    //OPTIONS SETTINGS
  
    //public void SaveFOV(int val) { m_FOV = val; }
    //public float LoadFOV() => m_FOV;

    public void SaveOptions(/*int fov,*/ int fps, bool fullscreen, int vysnc )
    {
       // m_FOV = fov;
        m_FPS = fps;
        m_Fullscreen=fullscreen;
        m_VYsnc=vysnc;
    }
}
