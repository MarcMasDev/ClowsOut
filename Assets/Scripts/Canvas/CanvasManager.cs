using UnityEngine.SceneManagement;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public Transform m_LifeBarParent;
    public CanvasGroup[] m_IngameCanvas;
    public CanvasGroup m_PauseMenu;
    public BulletHUDFinal m_HudFinal;

    private CanvasGroup m_CurrentBulletMenuCanvas;
    private BulletMenu m_BulletMenu;

    public Animator m_WinCanvas;
    public Animator m_LoseCanvas;
    [SerializeField] public bool m_BulletMenuLocked;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += Init;
        GameManager.GetManager().GetInputManager().OnStartBacking += ShowIngameMenu;
        GameManager.GetManager().GetInputManager().OnStartPause += ShowPauseGame;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= Init;
        GameManager.GetManager().GetInputManager().OnStartBacking -= ShowIngameMenu;
        GameManager.GetManager().GetInputManager().OnStartPause -= ShowPauseGame;

    }
    public void Init(Scene scene, LoadSceneMode a)
    {
        GameManager.GetManager().SetCanvasManager(this);
    }

    private void Start()
    {
        ShowIngameMenu();
    }

    private static void MenuCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    private static void GameCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowPauseGame()
    {
        ShowCanvasGroup(m_PauseMenu);
        HideCanvasGroup(m_IngameCanvas);
        SetPauseConfig();
    }

    public void ShowBulletMenu()
    {
        m_BulletMenuLocked = true;
        if (m_CurrentBulletMenuCanvas != null)
        {
            ShowCanvasGroup(m_CurrentBulletMenuCanvas);
        }
        HideCanvasGroup(m_IngameCanvas);
        m_BulletMenu.UpdateBulletMenu();
        SetMenuConfig();
    }
    public void ShowIngameMenu()
    {
        m_BulletMenuLocked = false;
        ShowCanvasGroup(m_IngameCanvas);
        HideCanvasGroup(m_PauseMenu);
        //if (m_CurrentBulletMenuCanvas != null)
        //{
        //    HideCanvasGroup(m_CurrentBulletMenuCanvas);
        //}
        SetIngameConfig();
    }
    public void SetPauseConfig()
    {
        MenuCursor();
        GameManager.GetManager().GetInputManager().SwitchToActionMapPauseMenu();
        GameManager.GetManager().GetCameraManager().CameraFixedUpdate();
        Time.timeScale = 0;
    }

    public void SetMenuConfig()
    {
        MenuCursor();
        GameManager.GetManager().GetInputManager().SwitchToMenuActionMap();
        GameManager.GetManager().GetCameraManager().CameraFixedUpdate();
    }
    public void SetIngameConfig()
    {
        GameCursor();
        GameManager.GetManager().GetInputManager().SwitchToPlayerActionMap();
        GameManager.GetManager().GetCameraManager().CameraLateUpdate();
        GameManager.GetManager().GetPlayerBulletManager().Reload();
        Time.timeScale = 1;
    }

    public void SetBulleMenutCanvasGroup(CanvasGroup canv, BulletMenu bm)
    {
        m_CurrentBulletMenuCanvas = canv;
        m_BulletMenu = bm;
    }

    #region Show/Hide
    private void ShowCanvasGroup(CanvasGroup[] canvasGroups)
    {
        for (int i = 0; i < canvasGroups.Length; i++)
        {
            ShowCanvasGroup(canvasGroups[i]);
        }
    }
    private void ShowCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    private void HideCanvasGroup(CanvasGroup[] canvasGroups)
    {
        for (int i = 0; i < canvasGroups.Length; i++)
        {
            HideCanvasGroup(canvasGroups[i]);
        }
    }
    private void HideCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    #endregion

    #region Win/Death canvas


    /// <summary>
    /// if player deads endValue = false |
    /// if player wins endValue = true
    /// </summary>
    /// <param name="win"></param>
    public void End(bool win = false)
    {
        //if (win)
        //{
        //    m_WinCanvas.SetTrigger("End");
        //}
        //else
        //{
        m_LoseCanvas.SetTrigger("End");
        //}
    }
    #endregion
}
