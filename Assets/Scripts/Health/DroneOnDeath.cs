using System.Collections.Generic;
using UnityEngine;

public class DroneOnDeath : MonoBehaviour
{
    HealthSystem m_hp;
    List<GameObject> m_EnemiesList = new List<GameObject>();
    [SerializeField]
    GameObject m_Enemy;
    void Awake()
    {
        m_hp = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        /* for (int i = 0; i < transform.parent.childCount; i++)
         {
             m_EnemiesList.Add(transform.parent.GetChild(i).gameObject);
         }*/
    }

    private void OnEnable()
    {
        m_hp.m_OnDeath += OnDeath;
    }
    private void OnDisable()
    {
        m_hp.m_OnDeath -= OnDeath;
    }
    public void OnDeath(GameObject g)
    {
        GameManager.GetManager().SetLastEnemyDeathPos(g.GetComponent<BlackboardEnemies>().m_nav.transform.position);
        GameManager.GetManager().GetLevelData().SaveKills();
        gameObject.SetActive(false);
        if (m_Enemy)
        {
            Destroy(m_Enemy);
        }
        /*if (AllDeaths())
        {
            GameManager.GetManager().GetCanvasManager().End(true);
        }*/
    }

    private bool AllDeaths()
    {
        for (int i = 0; i < m_EnemiesList.Count; i++)
        {
            if (m_EnemiesList[i].activeSelf)
                return false;
        }
        return true;
    }
}