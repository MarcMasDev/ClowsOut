using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeChecker : MonoBehaviour
{
    Vector3[] m_directions = new Vector3[]{
        Vector3.right,
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.up,
        Vector3.down
    };
    [SerializeField]
    LayerMask m_CollisionLayerMask;
    RaycastHit[] m_hits;
    [SerializeField]
    NodePath m_NodePath;
    public List<NodePath> m_listPosiblesNodes = new List<NodePath>();
    // Start is called before the first frame update
    void Start()
    {
        CheckInsideRoom();
        
    }

    

    void CheckInsideRoom()
    {
        bool l_inside = true;
        m_hits = new RaycastHit[m_directions.Length];
        for (int i = 0; i < m_directions.Length; i++)
        {
            //Ponemos la direcion en global
            Vector3 dir = transform.TransformDirection(m_directions[i]);
            Physics.Raycast(transform.position, dir, out m_hits[i], Mathf.Infinity,m_CollisionLayerMask);
            if (m_hits[i].collider == null)
            {
                l_inside = false;
            }
            else
            {
                if (m_hits[i].distance < 1.5f)
                {
                    transform.position = transform.position - m_directions[i]*1.5f;
                }
            }
        }
        if (!l_inside)
        {
            Destroy(gameObject);    
        }
    }
   
    public void CheckPossibleConection(NodePath node )
    {
        Transform pos = node.transform;
        RaycastHit l_hits = new RaycastHit();
        Vector3 dir = pos.TransformDirection(pos.position) - transform.position;
        dir.Normalize();
        Physics.Raycast(transform.position, dir, out l_hits, Mathf.Infinity, m_CollisionLayerMask);
        if (l_hits.collider == null)
        {
            m_listPosiblesNodes.Add(node);
            m_NodePath.AddConection(node);
        }
    }
}
