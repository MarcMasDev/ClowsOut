using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeRecord 
{
    public float estimatedCostToTarget;
    public float costFromStart;
    public NodeRecord predecessor;
    public NodePath node;
    public float cost = 1f;
    public int id=0;
    public NodeRecord(int id) {
        this.id = id;
    }

}
