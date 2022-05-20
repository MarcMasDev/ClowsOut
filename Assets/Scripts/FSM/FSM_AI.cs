
using UnityEngine;
using UnityEngine.AI;

public class FSM_AI : MonoBehaviour
{
    [SerializeField]
    protected NavMeshAgent m_NavMeshAgent;
    public bool m_ExternAgent = false;

    public  virtual void Init()
    {
        if (!m_ExternAgent)
        {
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
        }
    }
    public virtual void ReEnter()
    {
    }
    public virtual void Exit()
    {
    }

    public void ChangeSpeed(float speed)
    {
        m_NavMeshAgent.speed = speed;
    }
}
