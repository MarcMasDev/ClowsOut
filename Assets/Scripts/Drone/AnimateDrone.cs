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
            //transform.localRotation = Quaternion.Lerp(transform.localRotation, m_targetRotation, 0.05f);
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
            m_targetRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, 0.0f, -30);


        }
        if (dir == Vector3.right)
        {
            m_targetRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, 0.0f, 30);
        }

    }
}
