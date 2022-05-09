using UnityEngine;

using static ShootSystem;

public class LevelData : MonoBehaviour
{
    public int m_CurrentScene;
    public int m_PreviousScene;

    private int m_RoomIndex;
    public BulletType[] m_BulletsSelected;

    private void Start()
    {
        GameManager.GetManager().SetLevelData(this);
    }


    public void SaveData()
    {
        print("A");
        m_BulletsSelected = GameManager.GetManager().GetPlayerBulletManager().m_UpdatableBulletList;
    }

    public void LoadData()
    { 
    
    
    }

}
