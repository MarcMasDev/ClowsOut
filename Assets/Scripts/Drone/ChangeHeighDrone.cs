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
    //float rateVelocity;//= 1f / Vector3.Distance(startPos, endPos) * m_speed;
    float t = 0.0f;
    Vector3 l_temp = Vector3.zero;
    Vector3 start;
    Vector3 end;
    float l_nextY;

    float m_MaxTime;
    bool m_DoLerp = true;
    IEnumerator IDoLerp;
    private void Start()
    {
        DistanceToFloor();
        l_nextY = transform.position.y + (m_distanceToFloor / 2);
        start = m_drone.position;
        end = new Vector3(m_drone.position.x, l_nextY, m_drone.position.z);
        //rateVelocity = 1f / Vector3.Distance(start,end) * m_speed;
        t = 0.0f;
        m_MaxTime = Vector3.Distance(m_drone.position, end) / m_speed;
        IDoLerp = DoLerp2();
        StartCoroutine(IDoLerp);
    }
    private void Update()
    {
        DistanceToFloor();
        //CalculateHeight();
        // moveY();
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
       // l_nextY = transform.position.y + (m_distanceToFloor / 2);
        
        if (m_distanceToFloor != m_PreviousDistanceToFloor)
        {
            start = m_drone.position;
            end = new Vector3(m_drone.position.x, l_nextY, m_drone.position.z);
             //rateVelocity = 1f / Vector3.Distance(m_drone.position, end) * m_speed;
             t = 0.0f;
        }
    }
    void moveY()
    {

       // m_y = LerpY();
        m_drone.position = new Vector3(m_drone.position.x, m_y, m_drone.position.z);
    }
    float LerpY ()
    {
        if (t <= m_MaxTime)
        {
            // t += Time.deltaTime * rateVelocity;//rate = 1f / Vector3.Distance(m_drone.position, end) * m_speed;
            t += Time.deltaTime;
            l_temp = Vector3.Lerp(start, end, t/m_MaxTime);
        }
        else
        {
            m_DoLerp = false;
        }
        return l_temp.y;
    }
    private void OnTriggerEnter(Collider other)
    {
        t = 0.0f;
        HeightZoneInfo l_info =other.GetComponent<HeightZoneInfo>();
        if(l_info != null)
        {
            t = 0.0f;
            Debug.Log("heigh change");
            l_nextY = l_info.m_Height;
            //m_drone.position = new Vector3(m_drone.position.x, l_nextY, m_drone.position.z);
            end = new Vector3(m_drone.position.x, l_nextY, m_drone.position.z);
            //rateVelocity = 1f / Vector3.Distance(m_drone.position, end) * m_speed;
            m_MaxTime = Mathf.Abs((l_nextY - m_drone.transform.position.y)) / m_speed;
            m_DoLerp = true;
            StopCoroutine(IDoLerp);
            IDoLerp = DoLerp2();
            StartCoroutine(IDoLerp);
        }
        
    }
    IEnumerator DoLerp()
    {
        t = 0.0f;
        while (t <= 1f)
        {
            //  print("t: " + t);
            t += Time.deltaTime;
            l_temp = Vector3.Lerp(start, end, t );
            m_y = l_temp.y;
            moveY();
            yield return null;
            
        }
    }
    IEnumerator DoLerp2()
    {
        t = 0.0f;
        print("y: " + m_drone.position.y + " NextY: " + l_nextY);
        while (Mathf.Abs(m_drone.position.y - l_nextY) > 1f)
        {
            if (m_drone.position.y < l_nextY)
            {
                m_y += m_speed* Time.deltaTime;
            }
            else
            {
                m_y -= m_speed * Time.deltaTime;
            }
            moveY();
            yield return null;
            
        }
    }
    
}
