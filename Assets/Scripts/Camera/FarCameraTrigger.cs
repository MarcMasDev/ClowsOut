using UnityEngine;

public class FarCameraTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.GetManager().GetCameraManager().SetFarCamera();
        if (!GameManager.GetManager().GetPlayer().GetComponent<Player_InputHandle>().Aiming)
        {
            GameManager.GetManager().GetCameraManager().m_SwitchCam.SwitchToNotAimingCamera();
        }
    }
}
