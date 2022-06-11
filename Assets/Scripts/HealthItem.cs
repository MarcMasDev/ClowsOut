using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{ 
    HealthSystem m_hp;
    [SerializeField] private ParticleSystem FX;
    [SerializeField] private Vector3 rotationOffset;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!m_hp)
                m_hp = other.GetComponent<HealthSystem>();

            if (m_hp.GetCurrentLife < m_hp.m_MaxLife)
            {
                if (FX)
                {
                    Transform t = Instantiate(FX, other.transform.position, Quaternion.identity).transform;
                    t.SetParent(other.transform);
                    t.rotation = Quaternion.Euler(rotationOffset);
                    t.gameObject.SetActive(true);
                }

                m_hp.TakeHealth();

                Destroy(gameObject);
            }
        }
    }
}
