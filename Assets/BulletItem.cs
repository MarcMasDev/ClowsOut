using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletItem : MonoBehaviour
{
    public enum BulletType {ATTRACTOR, TELEPORT, MARK, STICKY, ICE, ENERGY}
    [SerializeField] private BulletType bulletUnlock;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }
}
