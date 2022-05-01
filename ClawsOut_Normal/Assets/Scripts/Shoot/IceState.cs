using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceState : MonoBehaviour
{
    [SerializeField]
    Material m_IceMat;
    [SerializeField]
    Material m_normalsMat;
    [SerializeField]
    Renderer m_Renderer;
    

    
    public void StartStateIce()
    {
        m_Renderer.material = m_IceMat;
        StartCoroutine(ReturnToPreviousColor());
    }
    IEnumerator ReturnToPreviousColor()
    {
        yield return new WaitForSeconds(5);
        m_Renderer.material = m_normalsMat;
    }
}
