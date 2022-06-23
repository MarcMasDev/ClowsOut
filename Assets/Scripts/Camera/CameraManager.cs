using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CameraManager : MonoBehaviour
{
    public Camera m_Camera;
    public SwitchCam m_SwitchCam;
    public CinemachineBrain m_CinemachineBrain;
    public CinemachineVirtualCamera m_AimCamera;
    public CinemachineVirtualCamera m_MediumCamera;
    public CinemachineVirtualCamera m_FarCamera;
    [HideInInspector]
    public CinemachineVirtualCamera m_CurrentCamera;
    public CinemachineVirtualCamera m_LoadingCamera;
    [HideInInspector] public CinemachineVirtualCamera m_CurrentBulletMenu;
    //public CinemachineVirtualCamera m_CameraShake;
    public int m_IncreseCamPriority = 10;
    [HideInInspector] public bool m_Locked;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += Init;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= Init;
    }
    private void Init(Scene scene, LoadSceneMode mode)
    {
        Player_Blackboard l_playerBlackboard = GameManager.GetManager().GetPlayer().GetComponent<Player_Blackboard>();
        m_CinemachineBrain = l_playerBlackboard.m_CinemachineBrain;
        m_Camera = l_playerBlackboard.m_CinemachineBrain.GetComponent<Camera>(); 
        m_AimCamera = l_playerBlackboard.m_AimCamera;
        m_MediumCamera = l_playerBlackboard.m_MediumCamera;
        m_FarCamera = l_playerBlackboard.m_MediumCamera;
        m_SwitchCam.SwitchInitCam();
        //m_CurrentCamera = m_MediumCamera;
        CameraLateUpdate();
    }

    private void Awake()
    {
        GameManager.GetManager().SetCameraManager(this);

    }


    private void Start()
    {
        GameManager.GetManager().SetCameraManager(this);
    }

    public void CameraFixedUpdate()
    {
        m_CinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.FixedUpdate;
    }
    public void CameraLateUpdate()
    {
        m_CinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.LateUpdate;
    }

    public void SetBulletMachineCamera(CinemachineVirtualCamera cam)
    {
        Debug.Log("Switch Cam");
        m_CurrentBulletMenu = cam;
    }
    public void SetMediumCamera()
    {
        m_CurrentCamera = m_MediumCamera;
    }
    public void SetFarCamera()
    {
        m_CurrentCamera = m_FarCamera;
    }
}
