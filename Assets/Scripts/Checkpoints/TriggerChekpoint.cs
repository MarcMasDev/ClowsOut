using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChekpoint : MonoBehaviour
{
    [SerializeField]
    Transform m_respawnPos;
    private void OnTriggerEnter(Collider other)
    {
        GameManager.GetManager().GetCheckpointsManager().LastCheckpoint(m_respawnPos);   
    }
}
