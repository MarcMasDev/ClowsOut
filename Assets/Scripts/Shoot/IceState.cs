using System.Collections;
using UnityEngine;

public class IceState : MonoBehaviour
{
    [SerializeField]
    ParticleSystem m_IceFX;
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
        m_IceFX.gameObject.SetActive(true);
        StartCoroutine(ReturnToPreviousColor());
    }
    IEnumerator ReturnToPreviousColor()
    {
        yield return new WaitWhile(() => m_BlackBoard.m_isIceState);
        //yield return new WaitForSeconds(5);
        m_IceFX.gameObject.SetActive(false);
        m_Renderer.material = m_normalsMat;
    }
}
