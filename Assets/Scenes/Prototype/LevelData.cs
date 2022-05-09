using UnityEngine;
using UnityEngine.SceneManagement;
using static ShootSystem;

public class LevelData : MonoBehaviour
{
    public int m_CurrentScene;
    public int m_PreviousScene; 

    private int m_RoomIndex;
    [SerializeField]private BulletType[] m_BulletsSelected = new BulletType[3];




    private void Awake()
    {
        m_CurrentScene = SceneManager.GetActiveScene().buildIndex;
        m_PreviousScene = m_CurrentScene==0 ? 0 : SceneManager.GetActiveScene().buildIndex;

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
    //private void Start()
    //{
    //    GameManager.GetManager().SetLevelData(this);
    //}


    public void SaveDataPlayerBullets(BulletType[] savedBullets) { m_BulletsSelected = savedBullets; }//= GameManager.GetManager().GetPlayerBulletManager().m_UpdatableBulletList; }
    public BulletType[] LoadDataPlayerBullets() { return m_BulletsSelected; }

    public void LoadData()
    {
        //GameManager.GetManager().GetPlayerBulletManager().m_UpdatableBulletList = m_BulletsSelected;
    }

    
}
