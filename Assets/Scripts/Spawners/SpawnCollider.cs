using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCollider : MonoBehaviour
{
    [SerializeField] public Spawner[] m_Spawners;
    [SerializeField] public float[] i_Spawners;
    [SerializeField] public GameObject[] linkedspawners;
    private bool usedSpawner = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")&&!usedSpawner)
        {
            usedSpawner = true;
            StartCoroutine(WaitSpawn());
        }
    }
    IEnumerator WaitSpawn()
    {
        for (int i = 0; i < m_Spawners.Length; i++)
        {
            m_Spawners[i].Spawn();
            yield return new WaitForSeconds(i_Spawners[i]);
        }
        for (int i = 0; i < linkedspawners.Length; i++)
        {
            linkedspawners[i].SetActive(false);
        }
        gameObject.SetActive(false);
    }
}
