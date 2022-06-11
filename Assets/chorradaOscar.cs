using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chorradaOscar : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Impulse");
            var l_Controller = other.GetComponent<Player_MovementController>();
            Vector3 l_Direction = l_Controller.m_Direction;
            if (l_Direction == Vector3.zero)
            {
                l_Direction = l_Controller.m_DashDirection;
            }
            float l_Rotate = Random.Range(0, 360);
            m_Rigidbody.AddForce(Quaternion.Euler(0, l_Rotate, 0) * transform.right * 8, ForceMode.Impulse);
        }
    }
}
