using UnityEngine;
[RequireComponent (typeof(ShootSystem))]
public class AtackFSM : FSM_AI
{
    private FSM<States> m_brain;
    public States m_CurrentState;
    ShootSystem m_shootSystem;
    HighFSM m_HighFSM;
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
        m_HighFSM = GetComponent<HighFSM>();
        m_blackboardEnemies = GetComponent<BlackboardEnemies>();
        m_shootSystem = GetComponent<ShootSystem>();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        m_brain.Update();
        m_CurrentState = m_brain.currentState;
        transform.LookAt(m_blackboardEnemies.m_Player);
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
                    Shoot();
                    m_elapsedTime = 0f;
                    m_counter++;
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
        Vector3 l_bulletDir = (m_blackboardEnemies.m_Player.position - m_firepoint.position).normalized;
        m_shootSystem.BulletShoot(m_firepoint.position, l_bulletDir, m_BulletSpeed, m_bulletType);
    }
    public enum States
    {
        INITIAL,
        ATACK,
        MOVE_UNTIL_SEES_PLAYER
    }
}
