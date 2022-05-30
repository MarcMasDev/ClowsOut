using UnityEngine;

public class SwitchCam : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.GetManager().GetInputManager().OnStartAiming += SwitchToAimCamera;
        GameManager.GetManager().GetInputManager().OnStopAiming += SwitchToThirdCamera;
        //GameManager.GetManager().GetPlayer().GetComponent<Player_FSM>().OnStartDashing += SwitchToDashCamera;
        //GameManager.GetManager().GetPlayer().GetComponent<Player_FSM>().OnStopDashing += SwitchToThirdCamera;
    }
    private void OnDisable()
    {
        GameManager.GetManager().GetInputManager().OnStopAiming -= SwitchToThirdCamera;
        GameManager.GetManager().GetInputManager().OnStartAiming -= SwitchToAimCamera;
        GameManager.GetManager().GetPlayer().GetComponent<Player_FSM>().OnStartDashing -= SwitchToDashCamera;
        GameManager.GetManager().GetPlayer().GetComponent<Player_FSM>().OnStopDashing -= SwitchToThirdCamera;
    }
    private void Start()
    {
        GameManager.GetManager().GetPlayer().GetComponent<Player_FSM>().OnStartDashing += SwitchToDashCamera;
        GameManager.GetManager().GetPlayer().GetComponent<Player_FSM>().OnStopDashing += SwitchToThirdCamera;
    }
    public void SwitchToBulletMenuCamera()
    {
        if (!GameManager.GetManager().GetCameraManager().m_Locked )
        {
            //GameManager.GetManager().GetCameraManager().m_CameraShake.Priority = 0;
            GameManager.GetManager().GetCameraManager().m_AimCamera.Priority = 0;
            GameManager.GetManager().GetCameraManager().m_DashCamera.Priority = 0;
            if (GameManager.GetManager().GetCameraManager().m_CurrentBulletMenu != null)
            {
                GameManager.GetManager().GetCameraManager().m_CurrentBulletMenu.Priority += GameManager.GetManager().GetCameraManager().m_IncreseCamPriority;
            }
        }
    }
    public void SwitchToAimCamera()
    {
        if (!GameManager.GetManager().GetCameraManager().m_Locked)
        {
            //GameManager.GetManager().GetCameraManager().m_CameraShake.Priority = 0;
            GameManager.GetManager().GetCameraManager().m_AimCamera.Priority += GameManager.GetManager().GetCameraManager().m_IncreseCamPriority;
            GameManager.GetManager().GetCameraManager().m_DashCamera.Priority = 0;
        }
    }
    public void SwitchToDashCamera()
    {
        if (!GameManager.GetManager().GetCameraManager().m_Locked)
        {
            //GameManager.GetManager().GetCameraManager().m_CameraShake.Priority = 0;
            GameManager.GetManager().GetCameraManager().m_AimCamera.Priority = 0;
            GameManager.GetManager().GetCameraManager().m_DashCamera.Priority += GameManager.GetManager().GetCameraManager().m_IncreseCamPriority;
        }
    }
    public void SwitchToThirdCamera()
    {
        if (!GameManager.GetManager().GetCameraManager().m_Locked)
        {
            if (GameManager.GetManager().GetCameraManager().m_CurrentBulletMenu != null)
            {
                GameManager.GetManager().GetCameraManager().m_CurrentBulletMenu.Priority = 0; 
            }
            GameManager.GetManager().GetCameraManager().m_AimCamera.Priority = 0;
            GameManager.GetManager().GetCameraManager().m_DashCamera.Priority = 0;
            //GameManager.GetManager().GetCameraManager().m_CameraShake.Priority = 0;
        }
    }
    public void SwitchToShakeCamera()
    {
        if (!GameManager.GetManager().GetCameraManager().m_Locked)
        {
            GameManager.GetManager().GetCameraManager().m_AimCamera.Priority = 0;
            GameManager.GetManager().GetCameraManager().m_DashCamera.Priority = 0;
            //GameManager.GetManager().GetCameraManager().m_CameraShake.Priority += GameManager.GetManager().GetCameraManager().m_IncreseCamPriority;
        }
    }
}
