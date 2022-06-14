using UnityEngine;
using UnityEngine.UI;

public class HealthBarEnemy : MonoBehaviour, IRestart
{
    [SerializeField]
    Image m_HealthBar;
    [HideInInspector]
    public HealthSystem m_hp;
    private void Start()
    {
        AddRestartElement();
    }
    public void Init()
    {
        m_hp.m_OnHit += SetValue;
        m_hp.m_OnDeath += OnDeath;
    }
    private void OnDisable()
    {
        m_hp.m_OnHit -= SetValue;
    }

    public void SetValue(float amount)
    {
        m_HealthBar.fillAmount = amount;
        print("Hp_Bar " + amount);
    }
    public void OnDeath(GameObject a)
    {//TODO pooling 
        GameManager.GetManager().SetLastEnemyDeathPos(a.GetComponent<BlackboardEnemies>().m_nav.transform.position);
        GameManager.GetManager().GetLevelData().SaveKills();
        gameObject.SetActive(false);

        // Destroy(gameObject);
    }

    public void AddRestartElement()
    {
        GameManager.GetManager().GetRestartManager().addRestartElement(this);
    }

    public void Restart()
    {
        gameObject.SetActive(false);
    }
}
