using System.Collections.Generic;
using UnityEngine;
using static ShootSystemManager;

public class LevelData : MonoBehaviour
{
    private int m_CurrentRoom;

    [Header("PLAYER STATS")]
    [SerializeField] float m_Grade; //30%bullets used, death 50%, time 20%
    [SerializeField] int m_BulletsUsed;
    [SerializeField] int m_PlayerDeath;
    [SerializeField] float m_CurrTimeLevel;
    [SerializeField] float m_TotalTimeLevel;
    [SerializeField] int m_CurrentLevel;
    [SerializeField] List<string> m_NameLevel = new List<string>();
    [SerializeField] BulletType[] m_BulletsSelected = new BulletType[3];

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
    }

    private void Update()
    {
        m_CurrTimeLevel += Time.deltaTime;
    }

    public void SaveDataPlayerBullets(BulletType[] savedBullets) { m_BulletsSelected = savedBullets; }//= GameManager.GetManager().GetPlayerBulletManager().m_UpdatableBulletList; }
    public BulletType[] LoadDataPlayerBullets() { return m_BulletsSelected; }

    public void SaveDeathsPlayer() { m_PlayerDeath++; }
    public int LoadDeathsPlayer() { return m_PlayerDeath; }

    public void SaveBulletsUsed() { m_BulletsUsed++; }
    public int LoadBulletsUsed() { return m_BulletsUsed; }

    public void SaveTotalTime() { m_TotalTimeLevel = m_CurrTimeLevel;}
    public float LoadTotalTime() { return m_TotalTimeLevel; }
    public void ResetTotalTime() { m_TotalTimeLevel = 0; }

    public float LoadGrade()
    {
        float l_Average = m_BulletsUsed * 0.3f + m_PlayerDeath * 0.5f + (LoadTotalTime()/60) * 0.2f;
      
        m_Grade = Mathf.Clamp(l_Average, 0, 100);

        return m_Grade;
    }

    public void SaveRoom(int i) { m_CurrentLevel = i; }
    public int LoadRoom() { return m_CurrentRoom; }

    public void SaveLevel(int i) { m_CurrentRoom = i; }
    public string LoadLevelName() { return m_NameLevel[m_CurrentLevel]; }

}
