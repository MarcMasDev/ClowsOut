using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePath : MonoBehaviour
{
    [SerializeField]
    GameObject m_Trigger;
    [SerializeField]
    LayerMask m_CollisionLayerMask;
    [SerializeField]
    Transform m_PreviusNode = null;
    [SerializeField]
    Transform   m_NextNode = null;
    public NodePath[] m_Conections;
    public bool m_IsAnEntrance = false;
    // Start is called before the first frame update
    void Start()
    {
        ChangePosTrigger();
    }
    public void ChangePosTrigger()
    {
        RaycastHit m_hits;
        m_hits = new RaycastHit();
        Vector3 dir = transform.TransformDirection(Vector3.down);
        Physics.Raycast(transform.position, dir, out m_hits, Mathf.Infinity, m_CollisionLayerMask);
        if (m_hits.collider != null)
        {
            m_Trigger.transform.position = m_hits.point;
        }
    }
}
