using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class NodePath : MonoBehaviour
{
    [SerializeField]
    GameObject m_Trigger;
    [SerializeField]
    LayerMask m_CollisionLayerMask;
    //public NodePath[] m_Conections;
    public  List<NodePath> m_Conections;
    public bool m_IsAnEntrance = false;
    public float cost = 1f;
    // Start is called before the first frame update
    void Start()
    {
        //ChangePosTrigger();
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
    public void AddConection(NodePath nodePath)
    {
        if (m_Conections.Contains(nodePath))
        {}
        else
        {
            m_Conections.Add(nodePath);
        }
    }
}
