using UnityEngine.AI;
using UnityEngine;

public class OnCollisionEnemy : MonoBehaviour
{
    NavMeshAgent m_NavMesh;

    private void Awake()
    {
        m_NavMesh = GetComponent<NavMeshAgent>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            m_NavMesh.enabled = true;
        }
    }
}
