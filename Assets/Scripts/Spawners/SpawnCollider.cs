using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCollider : MonoBehaviour
{
    [SerializeField] public Spawner[] m_Spawners;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            for (int i = 0; i < m_Spawners.Length; i++)
            {
                m_Spawners[i].Spawn();
            }
            gameObject.SetActive(false);
        }
    }
}
