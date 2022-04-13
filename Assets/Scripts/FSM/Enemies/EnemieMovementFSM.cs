using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieMovementFSM : MonoBehaviour
{
    private FSM<States> m_brain;
    public BlackboardEnemies blackboardEnemies;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_brain.Update();
    }

    public void Init()
    {
        m_brain = new FSM<States>(States.INITIAL);
        
    }
    public enum States
    {
        INITIAL
    }
}
