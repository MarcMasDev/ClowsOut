using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera m_Camera;
    public CinemachineBrain m_CinemachineBrain;
    public CinemachineVirtualCamera m_aimCamera;
    public int m_IncreseCamPriority = 10;
    [HideInInspector] public bool m_Locked;

    //private static CameraManager m_Instance = null;

    private void Awake()
    {
        CameraLateUpdate();
    }

    private void Start()
    {
        GameManager.GetManager().SetCameraManager(this);
    }
    //public static CameraManager Instance
    //{
    //    get
    //    {
    //        if (m_Instance == null)
    //        {
    //            m_Instance = GameObject.FindObjectOfType<CameraManager>();
    //        }
    //        return m_Instance;
    //    }
    //}

    public void CameraFixedUpdate()
    {
        m_CinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.FixedUpdate;
    }
    public void CameraLateUpdate()
    {
        m_CinemachineBrain.m_UpdateMethod = CinemachineBrain.UpdateMethod.LateUpdate;
    }
}
