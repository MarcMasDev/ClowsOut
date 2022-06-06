using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.GetManager().SetUnlockIndex(GameManager.GetManager().GetCurrentUnlockIndex() + 1);
            Destroy(gameObject);
        }
    }
}
