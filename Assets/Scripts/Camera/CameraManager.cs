using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CameraManager : MonoBehaviour
{
    public Camera m_Camera;
    public CinemachineBrain m_CinemachineBrain;
    public CinemachineVirtualCamera m_AimCamera;
    public CinemachineVirtualCamera m_DashCamera;
    public CinemachineVirtualCamera m_BulletMenu;
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
        GameManager.GetManager().SetCameraManager(this);
    }

    private void Awake()
    {
        CameraLateUpdate();
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
}
