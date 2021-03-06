using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrizmoHeight : MonoBehaviour
{
    public float m_Height = 15f;
    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(new Vector3(transform.position.x, m_Height, transform.position.z), "HeighGizmo", false);
        Gizmos.DrawSphere(new Vector3(transform.position.x, m_Height, transform.position.z), 1f);

    }
}

