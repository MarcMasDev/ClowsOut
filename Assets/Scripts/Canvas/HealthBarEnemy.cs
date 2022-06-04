using UnityEngine;
using UnityEngine.UI;

public class HealthBarEnemy : MonoBehaviour, IRestart
{
    [SerializeField]
    Image m_HealthBar;
    [SerializeField]
    bool m_IsPlayer = false;
     public HealthSystem m_hp;
    private void Start()
    {
        if (m_hp != null)
        {
            m_hp.m_OnHit += SetValue;
            m_hp.m_OnDeath += OnDeath;
        }
        AddRestartElement();


    }
    private void OnEnable()
    {
        if (m_hp != null)
        {
            m_hp.m_OnHit += SetValue;
            m_hp.m_OnDeath += OnDeath;
        }
           
    }
    private void OnDisable()
    {
        m_hp.m_OnHit -= SetValue;
    }

    public void SetValue(float amount)
    {
        m_HealthBar.fillAmount = amount;
    }
    public void OnDeath(GameObject a)
    {//TODO pooling 
        if (!m_IsPlayer)
        {
            GameManager.GetManager().GetLevelData().SaveKills();
            gameObject.SetActive(false);
        }
        else{
            GameManager.GetManager().GetLevelData().SaveDeathsPlayer();
        }
        
       // Destroy(gameObject);
    }

    public void AddRestartElement()
    {
        GameManager.GetManager().GetRestartManager().addRestartElement(this);
    }

    public void Restart()
    {
        if (!m_IsPlayer)
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
       
    }
}
