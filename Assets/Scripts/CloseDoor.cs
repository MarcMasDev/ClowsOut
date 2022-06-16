using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    [SerializeField]
    Animator m_doorAnimator;
    bool m_fisrt = true;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (m_fisrt)
            {
                m_fisrt = false;
                m_doorAnimator.SetBool("Open", false);
            }
                
        }
    }
}
