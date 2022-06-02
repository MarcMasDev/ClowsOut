using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenSpawner : Spawner
{
    [SerializeField] private GameObject m_Enemy;
    [SerializeField] private GameObject m_EnemyParent;
    public override void Spawn()
    {
        Instantiate(m_Enemy, transform.position, transform.rotation, m_EnemyParent.transform);
    }
}
