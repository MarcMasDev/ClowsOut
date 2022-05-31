using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockHandMovement : MonoBehaviour
{
    [SerializeField]
    Transform m_HourHand, m_MinHand;
    [SerializeField]
    float m_HourSpeed, m_MinSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_MinHand.rotation =Quaternion.Euler(m_MinHand.rotation.eulerAngles.x, m_MinHand.rotation.eulerAngles.y, m_MinHand.rotation.eulerAngles.z + m_MinSpeed * Time.deltaTime);
        m_HourHand.rotation =Quaternion.Euler(m_HourHand.rotation.eulerAngles.x, m_HourHand.rotation.eulerAngles.y, m_HourHand.rotation.eulerAngles.z + m_HourSpeed * Time.deltaTime);
    }
}
