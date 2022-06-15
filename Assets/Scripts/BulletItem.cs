using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletItem : MonoBehaviour
{
    private bool m_IsBullet;
    [SerializeField]private float yOffset = 0.5f;
    [SerializeField]private float ySpeed= 3f;
    [SerializeField]private float scaleSpeed= 2f;
    private float i = 0;
    private void Awake()
    {
        transform.position -= new Vector3(0, yOffset/3, 0);
        transform.localScale = Vector3.zero;
    }
    private void Update()
    {
        if (i < yOffset)
        {
            transform.position += new Vector3(0, Time.deltaTime*ySpeed, 0);
            i += Time.deltaTime;
            transform.localScale += new Vector3(Time.deltaTime * scaleSpeed, Time.deltaTime * scaleSpeed, Time.deltaTime * scaleSpeed);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!m_IsBullet)
        {
            if (other.CompareTag("Player"))
            {
                m_IsBullet = true;
                GameManager.GetManager().SetCurrentRoomIndex(GameManager.GetManager().GetCurrentRoomIndex() + 1);
                Destroy(gameObject);
            }
        }
    }
}
