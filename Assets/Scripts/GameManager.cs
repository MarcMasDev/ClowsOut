using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private static bool newManager;
    public static GameManager m_Instance = null;
    public static GameManager GetManager() => m_Instance;

    [SerializeField] private CameraManager m_CameraManager;
    [SerializeField] private CanvasManager m_CanvasManager;
    [SerializeField] private InputManager m_InputManager;
    [SerializeField] private RestartElements m_RestartElements;
    [SerializeField] private LevelData m_LevelData;
    [SerializeField] private Player_BulletManager m_PlayerBulletManager;

    public void SetCameraManager(CameraManager camera) { m_CameraManager = camera; }
    public void SetCanvasManager(CanvasManager canvas) { m_CanvasManager = canvas; }
    public void SetInputManager(InputManager input) { m_InputManager = input; }
    public void SetRestartElements(RestartElements restart) { m_RestartElements = restart; }
    public void SetLevelData(LevelData leveldata) { m_LevelData = leveldata; }
    public void SetPlayerBulletManager(Player_BulletManager bullet) { m_PlayerBulletManager = bullet; }

    public CameraManager GetCameraManager() => m_CameraManager;
    public CanvasManager GetCanvasManager() => m_CanvasManager;
    public InputManager GetInputMaager() => m_InputManager;
    public RestartElements GetRestartManager() => m_RestartElements;
    public LevelData GetLevelData() => m_LevelData;
    public Player_BulletManager GetPlayerBulletManager() => m_PlayerBulletManager;

    Action m_Action;
    protected void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
            DontDestroyOnLoad(m_Instance);
        }
        else if (m_Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += LoadDataGameManager;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= LoadDataGameManager;
    }

    private void LoadDataGameManager(Scene scene, LoadSceneMode mode)
    {
        
        //print(scene.buildIndex);
        //if (scene.buildIndex < GetLevelData().m_CurrentScene)
        //{
        //    GetLevelData().LoadData();
        //}
    }
}
