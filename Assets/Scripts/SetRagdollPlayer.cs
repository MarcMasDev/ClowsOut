using UnityEngine;
using UnityEngine.AI;

public class SetRagdollPlayer : MonoBehaviour
{
    [SerializeField]
    Collider[] m_colliders;
    [SerializeField]
    private Animator m_Animator;
    [SerializeField]
    private GameObject m_PlayerGO;
    private MonoBehaviour[] m_Scripts;
    private NavMeshAgent m_NavAgent;
    [SerializeField]
    private DissolveShaderPlayer m_Shader;
    private void Start()
    {
        m_colliders = GetComponentsInChildren<Collider>();
        m_Scripts = m_PlayerGO.GetComponents<MonoBehaviour>();
        m_NavAgent = m_PlayerGO.GetComponent<NavMeshAgent>();
        TurnOffRagdoll();
    }

    public void TurnOffRagdoll()
    {
        foreach (var collider in m_colliders)
        {
            if (collider.gameObject != this.gameObject)
            {
                collider.isTrigger = true;
                collider.attachedRigidbody.isKinematic = true;
            }

        }
    }
    public void TurnOnRagdoll()
    {
        foreach (var collider in m_colliders)
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
        foreach (MonoBehaviour c in m_Scripts)
        {
            c.enabled = false;
        }
        m_NavAgent.enabled = false;
    }
}
