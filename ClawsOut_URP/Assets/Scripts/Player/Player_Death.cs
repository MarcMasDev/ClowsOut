using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Death : MonoBehaviour
{
    HealthSystem m_hp;
    // Start is called before the first frame update
    void Awake()
    {
        m_hp = GetComponent<HealthSystem>();
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

        print("dead");
        CanvasManager.Instance.End(false);
    }
}
