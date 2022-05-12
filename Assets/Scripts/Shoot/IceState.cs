using System.Collections;
using UnityEngine;

public class IceState : MonoBehaviour
{
    [SerializeField]
    Material m_IceMat;
    [SerializeField]
    Material m_normalsMat;
    [SerializeField]
    Renderer m_Renderer;


    BlackboardEnemies m_BlackBoard;

    private void Awake()
    {
        m_BlackBoard = GetComponent<BlackboardEnemies>();
    }
    public void StartStateIce()
    {
        m_Renderer.material = m_IceMat;
        StartCoroutine(ReturnToPreviousColor());
    }
    IEnumerator ReturnToPreviousColor()
    {
        yield return new WaitWhile(() => m_BlackBoard.isIceState);
        //yield return new WaitForSeconds(5);
        m_Renderer.material = m_normalsMat;
    }
}
