using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eliminar : MonoBehaviour
{
    [SerializeField]
    NodePath start;
    [SerializeField]
    NodePath goal;
    [SerializeField]
    AStarCreatePath astar;
    // Start is called before the first frame update
    void Start()
    {
       List<NodePath> a= astar.Inizialize(start, goal);
        foreach (NodePath item in a)
        {
            print(item.gameObject.name);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
