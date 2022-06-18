using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public Transform m_LifeBarParent;
    public CanvasGroup[] m_IngameCanvas;
    public CanvasGroup m_PauseMenu;
    public CanvasGroup m_RecordWin;
    public BulletHUDFinal m_HudFinal;

    private CanvasGroup m_CurrentBulletMenuCanvas;
    private BulletMenu m_BulletMenu;

    [SerializeField] public bool m_BulletMenuLocked;
    [SerializeField]
    Image m_Reticle;
    PauseMenu m_Pause;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += Init;
        GameManager.GetManager().GetInputManager().OnStartQuitPause += ShowIngameMenuAfterPause;
        GameManager.GetManager().GetInputManager().OnStartPause += ShowPauseGame; // ShowWinMenu;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= Init;
        GameManager.GetManager().GetInputManager().OnStartQuitPause -= ShowIngameMenuAfterPause;
        GameManager.GetManager().GetInputManager().OnStartPause -= ShowPauseGame;//ShowWinMenu;
    }
    public void Init(Scene scene, LoadSceneMode a)
    {
        GameManager.GetManager().SetCanvasManager(this);
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
        if (!GameManager.GetManager().GetLevelData().m_GameStarted)
            return;

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
        m_RecordWin.GetComponent<Animator>().Play("WinnerCanvas");
        HideCanvasGroup(m_IngameCanvas);
        ShowCanvasGroup(m_RecordWin);
        SetPauseConfig();
        Time.timeScale = 0;

    }
    #region pause menu

    public void ShowIngameMenuAfterPause()
    {
        if (!GameManager.GetManager().GetLevelData().m_GameStarted)
            return;
        m_Pause.CloseAllOptions();
        m_Pause.CloseWarning();
        ShowCanvasGroup(m_IngameCanvas);
        HideCanvasGroup(m_PauseMenu);
        SetIngameConfig();

    }
    #endregion
    public void SetPauseConfig()
    {
        m_Reticle.gameObject.SetActive(false);
        MenuCursor();
        GameManager.GetManager().GetInputManager().SwitchToActionMapPauseMenu();
        GameManager.GetManager().GetCameraManager().CameraFixedUpdate();
        Time.timeScale = 0;
    }

    public void SetMenuConfig()
    {
        m_Reticle.gameObject.SetActive(false);
        MenuCursor();
        GameManager.GetManager().GetInputManager().SwitchToMenuActionMap();
        GameManager.GetManager().GetCameraManager().CameraFixedUpdate();
        Time.timeScale = 0;
    }
    public void SetIngameConfig()
    {
        m_Reticle.gameObject.SetActive(true);
        GameCursor();
        GameManager.GetManager().GetInputManager().SwitchToPlayerActionMap();
        GameManager.GetManager().GetCameraManager().CameraLateUpdate();
        Time.timeScale = 1;
    }
    public void ShowReticle()
    {
        m_Reticle.enabled = true;
    }
    public void HideReticle()
    {
        m_Reticle.enabled = false;
    }
    public void SetBulleMenutCanvasGroup(CanvasGroup canv, BulletMenu bm)
    {
        m_CurrentBulletMenuCanvas = canv;
        m_BulletMenu = bm;
        //HideReticle();
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
    private void ShowCanvasGroup(CanvasGroup canvasGroup, float alpha = 1.0f)
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
    private void HideCanvasGroup(CanvasGroup canvasGroup, float alpha = 0)
    {
        canvasGroup.alpha = alpha;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    #endregion
}
