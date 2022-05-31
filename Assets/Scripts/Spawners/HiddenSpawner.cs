using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenSpawner : Spawner
{
    [SerializeField] private GameObject m_Enemy;

    public override void Spawn()
    {
        Instantiate(m_Enemy, transform.position, transform.rotation);
    }
}
