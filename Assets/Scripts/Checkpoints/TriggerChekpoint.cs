using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChekpoint : MonoBehaviour
{
    [SerializeField]
    Transform m_respawnPos;
    private void OnTriggerEnter(Collider other)
    {
        CheckPoints.m_instance.LastCheckpoint(m_respawnPos);
    }
}
