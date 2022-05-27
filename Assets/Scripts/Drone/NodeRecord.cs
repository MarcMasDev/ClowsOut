using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeRecord : MonoBehaviour
{
    public float estimatedCostToTarget;
    public float costFromStart;
    public NodeRecord predecessor;
    public NodePath node;
    public float cost = 1f;
    
}
