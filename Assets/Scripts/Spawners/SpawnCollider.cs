using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnCollider : MonoBehaviour, IRestart
{
    [SerializeField]
    public Transform m_Enemies;
    [SerializeField] public Spawner[] m_Spawners;
    [SerializeField] public float[] i_Spawners;
    [SerializeField] public GameObject[] linkedspawners;
    private bool usedSpawner = false;

    [SerializeField] private bool relocateEnemies = false;
    GameObject[] enemies;
    List<GameObject> checkedVisibleEnemies = new List<GameObject>();
    private bool noMoreEnemiesToRepos = false;
    private void Start()
    {
        AddRestartElement();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (relocateEnemies && !usedSpawner)
            {
                usedSpawner = true;
                checkedVisibleEnemies.Clear();
                enemies = GameObject.FindGameObjectsWithTag("EnemyRenderer");
                for (int i = 0; i < m_Spawners.Length; i++)
                {
                    if(!noMoreEnemiesToRepos)
                    {
                        GameObject nonVisibleEnemy = GetNonVisibleEnemy();

                        if (!nonVisibleEnemy)
                        {
                            noMoreEnemiesToRepos = true;
                            m_Spawners[i].Spawn();
                        }
                        else
                        {
                            NavMeshAgent e = nonVisibleEnemy.GetComponentInParent<NavMeshAgent>();
                            e.transform.position = m_Spawners[i].transform.position;
                        }
                    }
                    else
                    {
                        m_Spawners[i].Spawn();
                    }
                }
            }
            else if (!usedSpawner)
            {
                usedSpawner = true;
                StartCoroutine(WaitSpawn());
            }
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

    private GameObject GetNonVisibleEnemy()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (!checkedVisibleEnemies.Contains(enemies[i]))
            {
                checkedVisibleEnemies.Add(enemies[i]);
                if (!enemies[i].GetComponent<MeshRenderer>().isVisible)
                {
                    return enemies[i];
                }
            }
        }
        return null;
    }

    public void AddRestartElement()
    {
        GameManager.GetManager().GetRestartManager().addRestartElement(this);
    }

    public void Restart()
    {
        gameObject.SetActive(true);
        usedSpawner = false;//TODO revisar si es solo esto imagino que no
    }
}
