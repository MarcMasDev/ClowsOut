using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{ 
    HealthSystem m_hp;
    public float m_CuantityOfHealth = 50f;
    [SerializeField] private ParticleSystem FX;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (FX)
            {
                FX.Play();
            }
            m_hp = other.GetComponent<HealthSystem>();
            m_hp.TakeHealth(m_CuantityOfHealth);
            
            Destroy(gameObject);
        }
    }
}
