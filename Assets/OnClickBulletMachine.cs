using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickBulletMachine : MonoBehaviour
{
    public void OnAcceptBulletSwitchCam()
    {
        GameManager.GetManager().GetCameraManager().m_SwitchCam.SwitchToNotAimingCamera();
  
    } 
    public void OnAcceptRestTimeScale()
    {
        GameManager.GetManager().GetCanvasManager().ShowIngameMenu();
    }

}
