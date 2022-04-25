using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof(ShootSystem))]
public class AtackFSM : FSM_AI
{
    private FSM<States> m_brain;
    public States m_CurrentState;
    ShootSystem m_shootSystem;
    public ShootSystem.BulletType m_bulletType;
    public float m_BulletSpeed =10f;
    public Transform m_firepoint;
    BlackboardEnemies m_blackboardEnemies;
    public float m_frequency = 0.2f;
    float m_elapsedTime = 0f;
    int m_counter = 0;
    public int m_MaxAttacks = 2;
    void Awake()
    {
        m_blackboardEnemies = GetComponent<BlackboardEnemies>();
        m_shootSystem = GetComponent<ShootSystem>();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        m_brain.Update();
        m_CurrentState = m_brain.currentState;
    }
    public override void Init()
    {
        m_brain = new FSM<States>(States.INITIAL);
        m_brain.SetReEnter(() =>
        {
            m_elapsedTime = 0f;
            m_counter = 0;
            m_brain.ChangeState(States.INITIAL);
        });
        m_brain.SetExit(() =>
        {
            m_elapsedTime = 0f;
            m_counter = 0;
            this.enabled = false;
        });
        m_brain.SetOnEnter(States.INITIAL, () => {
            m_brain.ChangeState(States.ATACK);
        });
        m_brain.SetOnEnter(States.ATACK, () => {
            m_elapsedTime = 0f;
            m_counter = 0;
        });
        m_brain.SetOnStay(States.INITIAL, () => {
            m_brain.ChangeState(States.ATACK);
        });
        m_brain.SetOnStay(States.ATACK, () => {
            m_elapsedTime += Time.deltaTime;
            if(m_counter < m_MaxAttacks)
            {
                if (m_elapsedTime > m_frequency)
                {
                    m_elapsedTime = 0f;
                    m_counter++;
                    Shoot();
                }
            }
            else
            {
                m_blackboardEnemies.m_FinishAttack = true;
            }
        });
        m_brain.SetOnEnter(States.MOVE_UNTIL_SEES_PLAYER, () =>
        {

        });
        m_brain.SetOnStay(States.MOVE_UNTIL_SEES_PLAYER, () =>
        {

        });
    }
    public override void ReEnter()
    {
        m_brain?.ReEnter();
    }
    public override void Exit()
    {
        m_brain?.Exit();
    }
    public void Shoot()
    {
        Debug.Log("disparo");
        Vector3 l_bulletDir = (m_blackboardEnemies.m_Player.position - m_firepoint.position).normalized;
        m_shootSystem.BulletShoot(m_firepoint.position, l_bulletDir, m_BulletSpeed, m_bulletType);
    }
    public enum States
    {
        INITIAL,
        ATACK,
        MOVE_UNTIL_SEES_PLAYER
    }
    void GoToPlayer()
    {
        Vector3 l_DirectionToPlayer = m_blackboardEnemies.m_Player.position - transform.position;
        l_DirectionToPlayer.y = 0;
        l_DirectionToPlayer.Normalize();
        Vector3 l_Destination = m_blackboardEnemies.m_Player.position - l_DirectionToPlayer
            * UnityEngine.Random.Range(m_blackboardEnemies.m_RangeAttack, m_blackboardEnemies.m_IdealRangeAttack);

        m_NavMeshAgent.destination = l_Destination;
    }
}
