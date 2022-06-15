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
    private NavMeshObstacle m_NavAgent;
  
    Material m_playerMat;
    [SerializeField]
    Player_Death m_PlayerDeath;
    private void Start()
    {
        m_colliders = GetComponentsInChildren<Collider>();
        m_Scripts = m_PlayerGO.GetComponents<MonoBehaviour>();
        m_NavAgent = m_PlayerGO.GetComponent<NavMeshObstacle>();
        m_playerMat = m_PlayerGO.GetComponent<Material>();
        TurnOffRagdoll();
        m_PlayerDeath.m_OnReviveS += TurnOffRagdollRevive;
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
        m_Animator.enabled = true;
    }
    public void TurnOffRagdollRevive()
    {
        print("animator enabled");
        foreach (var collider in m_colliders)
        {
            if (collider.gameObject != this.gameObject)
            {
                collider.isTrigger = true;
                collider.attachedRigidbody.isKinematic = true;
            }
        }
        m_Animator.enabled = true;
        m_Animator.ResetTrigger("Revive");
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
        print("Die");
        TurnOnRagdoll();
        m_Animator.enabled = false;
        //m_Shader.Dissolve();
        foreach (MonoBehaviour c in m_Scripts)
        {
            //c.enabled = false;
        }
        m_NavAgent.enabled = false;
    }
}
