using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
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
    // Start is called before the first frame update
    void Start()
    {
        CheckInsideRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        }
        if (!l_inside)
        {
            Destroy(gameObject);    
        }
    }

}
