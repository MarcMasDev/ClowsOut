using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumCameraTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("MEDIUM CAMERA");
        GameManager.GetManager().GetCameraManager().SetMediumCamera();
        if (!GameManager.GetManager().GetPlayer().GetComponent<Player_InputHandle>().Aiming)
        {
            //GameManager.GetManager().GetCameraManager().m_SwitchCam.SwitchToNotAimingCamera();
        }
    }
}
