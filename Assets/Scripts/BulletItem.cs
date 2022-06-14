using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletItem : MonoBehaviour
{
    private bool m_IsBullet;
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
