using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour
{
    [SerializeField] private DissolveShaderPlayer[] fx;
    [SerializeField] private float moveBetweenTrail = 0.5f;
    private float currentMoveBetweenTrail = 0f;
    private int index = 0;
    [SerializeField] private float yOffset = 0.5f;
    private Player_Blackboard playerBlackboard;
    private void Start()
    {
        playerBlackboard = GetComponent<Player_Blackboard>();
    }
    void Update()
    {
        if (playerBlackboard.m_Animator.GetBool("Dash"))
        {
            if (currentMoveBetweenTrail < 0)
            {
                fx[index].gameObject.SetActive(true);
                fx[index].Dissolve();
                fx[index].transform.position = new Vector3(transform.position.x, transform.position.y - yOffset, transform.position.z);
                fx[index].transform.rotation = transform.rotation;
                currentMoveBetweenTrail = moveBetweenTrail;
                index++;
                index %= fx.Length;
            }
            else
            {
                currentMoveBetweenTrail -= Time.deltaTime;
            }
        }
    }
}
