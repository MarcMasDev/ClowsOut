using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnClickBulletMachine : MonoBehaviour,IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.GetManager().GetCameraManager().m_SwitchCam.SwitchToNotAimingCamera(); 
        GameManager.GetManager().GetCanvasManager().ShowIngameMenu();
    }
    public void OnAcceptBulletSwitchCam()
    {
       // GameManager.GetManager().GetCameraManager().m_SwitchCam.SwitchToNotAimingCamera();
        //GameManager.GetManager().GetCanvasManager().ShowReticle();
    } 
    public void OnAcceptRestTimeScale()
    {
       // GameManager.GetManager().GetCanvasManager().ShowIngameMenu();
    }

}
