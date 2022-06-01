using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRagdoll : MonoBehaviour
{
    [SerializeField]
    Collider[] m_colliders;
    private void Start()
    {
        m_colliders = GetComponentsInChildren<Collider>();
        TurnOffRagdoll();
        
    }

    public void TurnOffRagdoll()
    {
        foreach (var collider in m_colliders)
        {
            if(collider.gameObject != this.gameObject)
            {
                collider.isTrigger = true;
                collider.attachedRigidbody.isKinematic = true;
            }
           
        }
    }
    public void TurnOnRagdoll()
    {
        foreach (var collider in m_colliders )
        {
            collider.isTrigger = false;
            collider.attachedRigidbody.isKinematic = false;
            collider.attachedRigidbody.velocity = Vector3.zero;
        }
    }
}
