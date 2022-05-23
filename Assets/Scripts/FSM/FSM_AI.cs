
using UnityEngine;
using UnityEngine.AI;

public class FSM_AI : MonoBehaviour
{
    protected NavMeshAgent m_NavMeshAgent;
  
    public  virtual void Init()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
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
