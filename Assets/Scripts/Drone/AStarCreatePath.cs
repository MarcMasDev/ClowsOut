using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarCreatePath : MonoBehaviour
{
    List<NodeRecord> m_openList = new List<NodeRecord>();
    List<NodeRecord> m_closedList = new List<NodeRecord>();

    bool m_pathFound = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Inizialize(NodePath start, NodePath goal)
    { 
        // initialize the nodeRecord for the start node
        NodeRecord nr = new NodeRecord();
        nr.node = start;
        nr.predecessor = null;
        nr.costFromStart = 0;
        nr.estimatedCostToTarget = heuristic(start.transform, goal.transform);
        // create the lists for open and closed nodes
        m_openList = new List <NodeRecord>();
        m_closedList = new List< NodeRecord>();
        m_openList.Add(nr); // add the nodeRecord for the start node to the openList
        m_pathFound = false;
    }
    public void CalculatePath(NodePath goal)
    {
        while (m_openList.Count>0 && m_pathFound == false)
        {
            NodeRecord l_CurrentNodePath = GetCurrentNode();
            if (l_CurrentNodePath.node == goal)
            {
                m_pathFound = true;
            }
            else
            {
                foreach (NodePath Conection in l_CurrentNodePath.node.m_Conections)
                {

                }
            }

        }
    }
    public NodeRecord GetCurrentNode()
    {
        //current = find in openList the nodeRecord with the lowest estimated cost to target
        NodeRecord l_CurrentNodePath = m_openList[0];
        for (int i = 0; i < m_openList.Count; i++)
        {
            if(l_CurrentNodePath.estimatedCostToTarget> m_openList[i].estimatedCostToTarget)
            {
                l_CurrentNodePath = m_openList[i];
            }
        }
        return l_CurrentNodePath;
    }
    private float heuristic(Transform start, Transform goal)
    {
       return Vector3.Distance(start.position, goal.position);
    }
}
