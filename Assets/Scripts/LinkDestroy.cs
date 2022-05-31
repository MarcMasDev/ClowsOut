using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkDestroy : MonoBehaviour
{
    [SerializeField] private bool destroy = false;
    void Update()
    {
        if (destroy)
        {
            Destroy(gameObject);
        }
    }
}
