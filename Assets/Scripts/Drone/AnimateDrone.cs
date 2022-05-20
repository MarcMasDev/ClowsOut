using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateDrone : MonoBehaviour
{
    [SerializeField]
    Transform m_HelixLeft;
    [SerializeField]
    Transform m_HelixRight;
    [SerializeField]
    float m_speed = 4f;
    bool m_Left;
    private Quaternion m_targetRotation;
    [SerializeField]
    Rigidbody m_rigidBody; 
    [SerializeField]
    float m_RootAngle = 30f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateHelix();
        if (transform.localRotation != m_targetRotation)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, m_targetRotation, 0.05f);
        }
        if (m_rigidBody.velocity.x > 0)
        {
            RotateDrone(Vector3.left);
        }
        else if (m_rigidBody.velocity.x < 0)
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
        m_HelixLeft.rotation = Quaternion.Euler(m_HelixLeft.rotation.eulerAngles.x, m_HelixLeft.rotation.eulerAngles.y, m_HelixLeft.rotation.eulerAngles.z + m_speed * Time.deltaTime);
        m_HelixRight.rotation = Quaternion.Euler(m_HelixRight.rotation.eulerAngles.x, m_HelixRight.rotation.eulerAngles.y, m_HelixRight.rotation.eulerAngles.z + m_speed * Time.deltaTime);
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
}
