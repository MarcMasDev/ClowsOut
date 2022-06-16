using UnityEngine;
using UnityEngine.UI;

public class HealthBarPlayer : MonoBehaviour, IRestart
{
    [SerializeField]
    Slider m_HealthBar;
    [HideInInspector]
    public HealthSystem m_hp;
    private void Start()
    {
        m_hp = GameManager.GetManager().GetPlayer().GetComponent<HealthSystem>();

        if (m_hp != null)
        {
            m_hp.m_OnHit += SetValue;
            m_hp.m_OnHealth += SetValue;
            m_hp.m_OnDeath += OnDeath;
        }
        AddRestartElement();
    }
    private void OnEnable()
    {
        if (m_hp != null)
        {
            m_hp.m_OnHit += SetValue;
            m_hp.m_OnHealth += SetValue;
            m_hp.m_OnDeath += OnDeath;
        }

    }
    private void OnDisable()
    {
        m_hp.m_OnHit -= SetValue;
        m_hp.m_OnHealth -= SetValue;
    }

    public void SetValue(float amount)
    {
        print("HEALTH");
        m_HealthBar.value = amount;
    }
    public void OnDeath(GameObject a)
    {
        GameManager.GetManager().GetLevelData().SaveDeathsPlayer();
    }

    public void AddRestartElement()
    {
        GameManager.GetManager().GetRestartManager().addRestartElement(this, transform);
    }

    public void Restart()
    {
        gameObject.SetActive(true);
        m_HealthBar.value = 1;
    }
}
