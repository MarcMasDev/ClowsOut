using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarCreatePath : MonoBehaviour
{
    public List<NodeRecord> m_openList = new List<NodeRecord>();
    public List<NodeRecord> m_closedList = new List<NodeRecord>();
    NodeRecord m_ClosedNode;
    public bool m_pathFound = false;
    public List<NodePath> m_path = new List<NodePath>();
    NodeRecord m_lastPointOfPath;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public List<NodePath> Inizialize(NodePath start, NodePath goal)
    { 
        // initialize the nodeRecord for the start node
        NodeRecord nr = new NodeRecord(0);
        nr.node = start;
        nr.predecessor = null;
        nr.costFromStart = 0;
        nr.estimatedCostToTarget = Heuristic(start.transform, goal.transform);
        // create the lists for open and closed nodes
        m_openList = new List <NodeRecord>();
        m_closedList = new List< NodeRecord>();
        m_openList.Add(nr); // add the nodeRecord for the start node to the openList
        m_pathFound = false;
        return CalculatePath( start,  goal);
    }
     List<NodePath> CalculatePath(NodePath start, NodePath goal)
    {
        int counter = 1;
        while (m_openList.Count>0 && m_pathFound == false )
        {
            
            NodeRecord l_CurrentNodePath = GetCurrentNode();
            counter++;
            if (l_CurrentNodePath.node.gameObject == goal.gameObject)
            {
                print(true);
            }
                if (l_CurrentNodePath.node == goal)
            {
                print("a"+true);
                m_pathFound = true;
                Debug.Log("path complete");
                m_lastPointOfPath = l_CurrentNodePath;
            }
            else
            {
                foreach (NodePath Conection in l_CurrentNodePath.node.m_Conections)
                {
                    //CASE 1: successor is neither in openList nor in closedList (it’s an UNVISITED node)
                    //CASE 2: successor is in openList
                    //CASE 3: successor is in closedList
                    bool l_ConectionNotInOpen = true;
                    bool l_ConectionIsInOpen = false;
                    bool l_ConectionIsInClosed= false;
                    foreach (var nodeRecord in m_openList)
                    {
                        if(nodeRecord.node == Conection)
                        {
                            l_ConectionIsInOpen = true;
                            l_ConectionNotInOpen = false;
                        }
                    }
                    foreach (var nodeRecord in m_closedList)
                    {
                        if(nodeRecord.node == Conection)
                        {
                            l_ConectionIsInClosed = true;
                            l_ConectionNotInOpen = false;
                            m_ClosedNode = nodeRecord;
                        }
                    }
                    if (l_ConectionNotInOpen)
                    {
                        NodeRecord nr = new NodeRecord(counter);
                        nr.node = Conection;
                        nr.predecessor = l_CurrentNodePath;
                        nr.costFromStart = l_CurrentNodePath.costFromStart + Conection.cost;
                        nr.estimatedCostToTarget = l_CurrentNodePath.costFromStart + Conection.cost + Heuristic(Conection.transform, goal.transform);
                        m_openList.Add(nr);
                        Debug.Log("case1 " + m_openList.Count);
                    }
                    if (l_ConectionIsInOpen)
                    {
                        foreach (var nodeRecord in m_openList)
                        {
                            if (nodeRecord.node == Conection)
                            {
                                if ((l_CurrentNodePath.costFromStart + Conection.cost) < nodeRecord.costFromStart)
                                {
                                    Debug.Log("case2 " + m_openList.Count);
                                    nodeRecord.predecessor = l_CurrentNodePath;
                                    nodeRecord.costFromStart = l_CurrentNodePath.costFromStart + Conection.cost;
                                    nodeRecord.estimatedCostToTarget = l_CurrentNodePath.costFromStart + Conection.cost + Heuristic(Conection.transform, goal.transform);
                                }
                                else { }
                            }
                        }
                        
                    }
                    if (l_ConectionIsInClosed)
                    {
                        if ((l_CurrentNodePath.costFromStart + Conection.cost) < m_ClosedNode.costFromStart)
                        {
                           
                            m_ClosedNode.predecessor = l_CurrentNodePath;
                            m_ClosedNode.costFromStart = l_CurrentNodePath.costFromStart + Conection.cost;
                            m_ClosedNode.estimatedCostToTarget = l_CurrentNodePath.costFromStart + Conection.cost + Heuristic(Conection.transform, goal.transform);
                            Debug.Log("case3 "+ m_closedList.Count);
                            m_closedList.Remove(m_ClosedNode);
                            Debug.Log("case3 " + m_closedList.Count);
                            m_openList.Add(m_ClosedNode);
                            //Remove from closed
                            //add to open
                        }
                        else { }
                            
                    }

                }
                m_openList.Remove(l_CurrentNodePath);
                m_closedList.Add(l_CurrentNodePath);
            }

        }
        if (m_pathFound)
        {
          NodeRecord  l_current = m_lastPointOfPath;
            while (l_current.node != start)
            {
                m_path.Add(l_current.node);
                l_current = l_current.predecessor;
            }
            m_path.Add(l_current.node);
            m_path.Reverse();
            return m_path;
        }
        else
        {
            Debug.Log("No path found");
            return null;
            
        }

    }
    private NodeRecord GetCurrentNode()
    {
        //current = find in openList the nodeRecord with the lowest estimated cost to target
        NodeRecord l_CurrentNodePath = m_openList[0];
        for (int i = 0; i < m_openList.Count; i++)
        {
            Debug.Log("estimatedCostToTarget id:"+ l_CurrentNodePath.id + " cost:" +  m_openList[i].estimatedCostToTarget);
            if (l_CurrentNodePath.estimatedCostToTarget > m_openList[i].estimatedCostToTarget)
            {
                l_CurrentNodePath = m_openList[i];
                Debug.Log("currentNode " + l_CurrentNodePath.id);
            }
        }
        return l_CurrentNodePath;
    }
    private float Heuristic(Transform start, Transform goal)
    {
       return Vector3.Distance(start.position, goal.position);
    }
}
