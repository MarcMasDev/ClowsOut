using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHeighDrone : MonoBehaviour
{
    [SerializeField]
    Transform m_1,m_2;
    [SerializeField]
    LayerMask m_layer;
    float m_distanceToFloor = 0;
    float m_PreviousDistanceToFloor = 0;
    [SerializeField]
    Transform m_drone;
    public float m_speed =0f;
    float m_y =0;

    [Header("Lerp")]
    float rateVelocity;//= 1f / Vector3.Distance(startPos, endPos) * m_speed;
    float t = 0.0f;
    Vector3 l_temp = Vector3.zero;
    Vector3 start;
    Vector3 end;
    private void Start()
    {
        DistanceToFloor();
        float l_nextY = transform.position.y + (m_distanceToFloor / 2);
        start = m_drone.position;
        end = new Vector3(m_drone.position.x, l_nextY, m_drone.position.z);
        rateVelocity = 1f / Vector3.Distance(start,end) * m_speed;
        t = 0.0f;
    }
    private void Update()
    {
        DistanceToFloor();
        CalculateHeight();
        m_PreviousDistanceToFloor = m_distanceToFloor;
        
    }
    void DistanceToFloor()
    {
        RaycastHit l_hit;
        Physics.Raycast(m_1.position, Vector3.up, out l_hit, Mathf.Infinity, m_layer);
        if (l_hit.collider != null)
        {
            m_distanceToFloor = l_hit.distance;
        }
        Physics.Raycast(m_2.position, Vector3.up, out l_hit, Mathf.Infinity, m_layer);
        if (l_hit.collider != null)
        {
            if (l_hit.distance < m_distanceToFloor)
            {
                m_distanceToFloor = l_hit.distance;
            }

        }
    }
    void CalculateHeight()
    {
        float l_nextY = transform.position.y + (m_distanceToFloor / 2);
        if (m_distanceToFloor != m_PreviousDistanceToFloor)
        {
            start = m_drone.position;
            end = new Vector3(m_drone.position.x, l_nextY, m_drone.position.z);
             rateVelocity = 1f / Vector3.Distance(m_drone.position, end) * m_speed;
             t = 0.0f;
        }
        m_y = LerpY();
        m_drone.position = new Vector3(m_drone.position.x, m_y, m_drone.position.z);
    }
    float LerpY ()
    {
        if (t <= 1f)
        {
            t += Time.deltaTime * rateVelocity;
            l_temp = Vector3.Lerp(start, end, t);
        }
        return l_temp.y;
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
}
