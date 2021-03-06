using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager m_Instance = null;
    public static GameManager GetManager() => m_Instance;

    [SerializeField] private CameraManager m_CameraManager;
    [SerializeField] private CanvasManager m_CanvasManager;
    [SerializeField] private InputManager m_InputManager;
    [SerializeField] private RestartElements m_RestartElements;
    [SerializeField] private LevelData m_LevelData;
    [SerializeField] private Player_BulletManager m_PlayerBulletManager;
    [SerializeField] private GameObject m_Player;
    [SerializeField] private ShootSystemManager m_ShootSystemManager;
    [SerializeField] private CameraShake m_camShake;
    [SerializeField] private OptionsMenu m_OptionsMenu;
    [SerializeField] private PauseMenu m_PauseMenu;
    [SerializeField] private CheckPoints m_CheckpointsManager;
    [SerializeField] private SceneLoader m_SceneLoader;

    private int roomIndex = 0;
    private Vector3 lastDeathEnemyPos;

    public void SetCameraManager(CameraManager camera) { m_CameraManager = camera; }
    public void SetCanvasManager(CanvasManager canvas) { m_CanvasManager = canvas; }
    public void SetInputManager(InputManager input) { m_InputManager = input; }
    public void SetRestartElements(RestartElements restart) { m_RestartElements = restart; }
    public void SetLevelData(LevelData leveldata) { m_LevelData = leveldata; }
    public void SetPlayerBulletManager(Player_BulletManager bullet) { m_PlayerBulletManager = bullet; }
    public void SetPlayer(GameObject player) { m_Player = player; }
    public void SetShootSystem(ShootSystemManager shoot) { m_ShootSystemManager = shoot; }
    public void SetCameraShake(CameraShake camShake) { m_camShake = camShake; }
    public void SetOptions(OptionsMenu options) { m_OptionsMenu = options; }
    public void SetPauseMenu(PauseMenu pause) { m_PauseMenu = pause; }
    public void SetCheckpointsManager(CheckPoints checpoint) { m_CheckpointsManager = checpoint; }
    public void SetCurrentRoomIndex(int i) { roomIndex = i; }
    public void SetLastEnemyDeathPos(Vector3 death_enemy) { lastDeathEnemyPos = death_enemy; }
    public void SetSceneLoader(SceneLoader sceneloader) { m_SceneLoader = sceneloader; }
    public CameraManager GetCameraManager() => m_CameraManager;
    public CanvasManager GetCanvasManager() => m_CanvasManager;
    public InputManager GetInputManager() => m_InputManager;
    public RestartElements GetRestartManager() => m_RestartElements;
    public LevelData GetLevelData() => m_LevelData;
    public Player_BulletManager GetPlayerBulletManager() => m_PlayerBulletManager;
    public GameObject GetPlayer() => m_Player;
    public ShootSystemManager GetShootSystemManager() => m_ShootSystemManager;
    public CameraShake GetCameraShake() => m_camShake;
    public OptionsMenu GetOptionsMenu() => m_OptionsMenu;
    public PauseMenu GetPauseMenu() => m_PauseMenu;
    public CheckPoints GetCheckpointsManager() => m_CheckpointsManager;
    public SceneLoader GetSceneLoader() => m_SceneLoader;
    public int GetCurrentRoomIndex() => roomIndex;
    public Vector3 GetLastEnemyDeathPos() => lastDeathEnemyPos;
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


    public IEnumerator StartGame()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(0.5f);
        m_Instance.GetCameraManager().GetComponent<SwitchCam>().SwitchInitCam();
        yield return new WaitForSecondsRealtime(1.35f);
        Time.timeScale = 1;
    }
}
