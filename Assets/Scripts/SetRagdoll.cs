using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SetRagdoll : MonoBehaviour
{
    [SerializeField]
    Collider[] m_colliders;
    [SerializeField]
    private Animator m_Animator;
    [SerializeField]
    private GameObject m_EnemyGO;
    private MonoBehaviour[] m_Scripts;
    private NavMeshAgent m_NavAgent;
    [SerializeField]
    private DissolveShaderEnemy m_Shader;
    [SerializeField]
    private Collider m_ColliderEnemy;
    private void Start()
    {
        m_colliders = GetComponentsInChildren<Collider>();
        m_Scripts = m_EnemyGO.GetComponents<MonoBehaviour>();
        m_NavAgent = m_EnemyGO.GetComponent<NavMeshAgent>();
        TurnOffRagdoll();
    }

    public void TurnOffRagdoll()
    {
        foreach (var collider in m_colliders)
        {
            if(collider.gameObject != this.gameObject)
            {
                collider.isTrigger = true;
                collider.attachedRigidbody.isKinematic = true;
            }
           
        }
    }
    public void TurnOnRagdoll()
    {
        foreach (var collider in m_colliders )
        {
            collider.isTrigger = false;
            collider.attachedRigidbody.isKinematic = false;
            collider.attachedRigidbody.velocity = Vector3.zero;
        }
    }
    public void Die()
    {
        TurnOnRagdoll();
        m_Animator.enabled = false;
        m_Shader.Dissolve();
        m_ColliderEnemy.enabled = false;
        foreach (MonoBehaviour c in m_Scripts)
        {
            c.enabled = false;
        }
        m_NavAgent.enabled = false;
    }
}
