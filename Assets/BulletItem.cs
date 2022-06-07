using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.GetManager().SetRoomIndex(GameManager.GetManager().GetCurrentRoomIndex() + 1);
            Destroy(gameObject);
        }
    }
}
