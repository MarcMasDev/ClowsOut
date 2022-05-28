using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectOtherNodesTrigger : MonoBehaviour
{
    [SerializeField]
    NodeChecker nodeChecker;
    
    private void OnTriggerEnter(Collider other)
    {
        NodePath l_node = other.gameObject.GetComponentInParent<NodePath>();
        if (l_node != null)
        {
            nodeChecker.CheckPossibleConection(l_node);
            Debug.Log("Manzana");
        }
        print(transform.parent.gameObject.name + " collide with " + other.transform.parent.gameObject.name);
    }
}
