using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class AnimateDrone : MonoBehaviour
{
    //Coment
    [SerializeField]
    Transform m_HelixLeft;
    [SerializeField]
    Transform m_HelixRight;
    [SerializeField]
    float m_speed = 4f;
    bool m_Left;
    private Quaternion m_targetRotation;
    [SerializeField]
    HealthSystem m_hp;
    [SerializeField]
    NavMeshAgent m_nav; 
    [SerializeField]
    float m_RootAngle = 30f;
    [SerializeField]
    VisualEffect m_vfx;
    // Start is called before the first frame update
    void Start()
    {
        m_hp.m_OnHit += OnHit;
        m_vfx.Stop();
    }
    private void OnDisable()
    {
        m_hp.m_OnHit -= OnHit;
    }
    // Update is called once per frame
    void Update()
    {
        RotateHelix();
        if (transform.localRotation != m_targetRotation)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, m_targetRotation, 0.05f);
        }
        
        if (m_nav.velocity.x > 0)
        {
            RotateDrone(Vector3.left);
        }
        else if (m_nav.velocity.x < 0)
        {
            RotateDrone(Vector3.right);
        }
        else
        {
            RotateDrone(Vector3.zero);
        }
    }
    public void RotateHelix()
    {
        m_HelixLeft.rotation = Quaternion.Euler(m_HelixLeft.rotation.eulerAngles.x, m_HelixLeft.rotation.eulerAngles.y + m_speed * Time.deltaTime, m_HelixLeft.rotation.eulerAngles.z );
        m_HelixRight.rotation = Quaternion.Euler(m_HelixRight.rotation.eulerAngles.x, m_HelixRight.rotation.eulerAngles.y + m_speed * Time.deltaTime, m_HelixRight.rotation.eulerAngles.z );
    }
    void RotateDrone(Vector3 dir)
    {
        if (dir == Vector3.left)
        {
            m_targetRotation = Quaternion.Euler(-m_RootAngle, transform.localRotation.eulerAngles.y , transform.localRotation.eulerAngles.z);


        }
        if (dir == Vector3.right)
        {
            m_targetRotation = Quaternion.Euler(m_RootAngle, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        }
        if(dir == Vector3.zero)
        {
            m_targetRotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        }
    }
    public void OnHit(float f)
    {
        m_vfx.Play();
        m_vfx.Stop();
    }
}
