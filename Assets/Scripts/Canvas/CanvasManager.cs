using UnityEngine.SceneManagement;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public Transform m_LifeBarParent;
    public CanvasGroup[] m_IngameCanvas;
    public CanvasGroup m_PauseMenu;
    public CanvasGroup m_RecordWin;
    public BulletHUDFinal m_HudFinal;

    private CanvasGroup m_CurrentBulletMenuCanvas;
    private BulletMenu m_BulletMenu;

    public Animator m_WinCanvas;
    public Animator m_LoseCanvas;
    [SerializeField] public bool m_BulletMenuLocked;
    [SerializeField]
    CanvasGroup m_Reticle;
    PauseMenu m_Pause;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += Init;
        //GameManager.GetManager().GetInputManager().OnStartBacking += ShowIngameMenu;
        GameManager.GetManager().GetInputManager().OnStartQuitPause += ShowIngameMenuAfterPause;
        GameManager.GetManager().GetInputManager().OnStartPause += ShowPauseGame;// ShowWinMenu;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= Init;
       //GameManager.GetManager().GetInputManager().OnStartBacking -= ShowIngameMenu;
        GameManager.GetManager().GetInputManager().OnStartQuitPause -= ShowIngameMenuAfterPause;
        GameManager.GetManager().GetInputManager().OnStartPause -= ShowPauseGame;//ShowWinMenu;// 
    }
    public void Init(Scene scene, LoadSceneMode a)
    {
        GameManager.GetManager().SetCanvasManager(this);
    }
    private void Awake()
    {
        m_Pause = m_PauseMenu.GetComponent<PauseMenu>();
    }
    private void Start()
    {
        ShowIngameMenu();
    }

    public void MenuCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    public void GameCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowPauseGame()
    {
        //MenuCursor();
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
        SetIngameConfig();
        m_CurrentBulletMenuCanvas = null;
        m_BulletMenu = null;
        GameManager.GetManager().GetPlayer().GetComponent<Player_Interact>().ResetInteractale();
        GameManager.GetManager().GetPlayerBulletManager().Reload();
    }

    public void ShowWinMenu()
    {
        MenuCursor();
        m_RecordWin.GetComponent<ScoreRecord>().UpdateRecord();
        HideCanvasGroup(m_IngameCanvas);
        ShowCanvasGroup(m_RecordWin);
        Time.timeScale = 0;
       
    }
    #region pause menu

    public void ShowIngameMenuAfterPause()
    {
        ShowCanvasGroup(m_IngameCanvas);
        HideCanvasGroup(m_PauseMenu);
        m_Pause.CloseOptions();
        m_Pause.CloseWarning();
        SetIngameConfig();
    }
    #endregion
    public void SetPauseConfig()
    {
        GameManager.GetManager().GetInputManager().SwitchToActionMapPauseMenu();
        GameManager.GetManager().GetCameraManager().CameraFixedUpdate();
        Time.timeScale = 0;
    }

    public void SetMenuConfig()
    {
        MenuCursor();
        GameManager.GetManager().GetInputManager().SwitchToMenuActionMap();
        GameManager.GetManager().GetCameraManager().CameraFixedUpdate();
        Time.timeScale = 0;
    }
    public void SetIngameConfig()
    {
        GameCursor();
        GameManager.GetManager().GetInputManager().SwitchToPlayerActionMap();
        GameManager.GetManager().GetCameraManager().CameraLateUpdate();
        Time.timeScale = 1;
        print("UwU");
    }
    public void ShowReticle()
    {
        ShowCanvasGroup(m_Reticle);
    }
    public void HideReticle()
    {
        HideCanvasGroup(m_Reticle);
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
            ShowCanvasGroupModified(canvasGroups[i]);
        }
    }
    private void ShowCanvasGroupModified(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = GameManager.GetManager().GetOptionsMenu().m_OptionsData.m_HudOpacity;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    private void ShowCanvasGroup(CanvasGroup canvasGroup, float alpha=1.0f)
    {
        canvasGroup.alpha = alpha;
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
    private void HideCanvasGroup(CanvasGroup canvasGroup, float alpha=0)
    {
        canvasGroup.alpha = alpha;
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
        // m_LoseCanvas.SetTrigger("End");
        ShowWinMenu();
        //}
    }
    #endregion
}
