using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateGraph : MonoBehaviour
{
    [SerializeField]
    GameObject m_node;
    [SerializeField]
    Transform m_StartPosition;
    [SerializeField]
    float m_SeperationBetweenNodes = 0.5f;
    [SerializeField]
    float m_VerticaLength = 20;
    [SerializeField]
    float m_HorizontaLength = 25;
    int counter = 0;
    private void Start()
    {
        SpawnNodes();
    }
    public void SpawnNodes()
    {
        float l_quantity = (m_VerticaLength / m_SeperationBetweenNodes)* (m_VerticaLength / m_SeperationBetweenNodes)* (m_VerticaLength / m_SeperationBetweenNodes);
        float l_quantityForRow = m_VerticaLength / m_SeperationBetweenNodes;
        float l_quantityForCol = m_HorizontaLength / m_SeperationBetweenNodes;
        for (int z = 0; z < l_quantityForCol; z++)//Pos z
        {
            for (int y = 0; y < l_quantityForRow; y++)
            {
                for (int x = 0; x < l_quantityForCol; x++)
                {
                    Vector3 l_pos = m_StartPosition.position + new Vector3(x*m_SeperationBetweenNodes,y* m_SeperationBetweenNodes,z* m_SeperationBetweenNodes);
                    GameObject go = Instantiate(m_node, l_pos, Quaternion.identity, m_StartPosition);
                    go.name = counter.ToString();
                    counter++;
                }
            }
        }
        //AssetDatabase.CreateAsset(m_StartPosition.gameObject, "Assets/Prefabs/Graph/ParentGraph.prefab");
        PrefabUtility.CreatePrefab("Assets/Prefabs/Graph/ParentGraph.prefab", m_StartPosition.gameObject);
    }
}
