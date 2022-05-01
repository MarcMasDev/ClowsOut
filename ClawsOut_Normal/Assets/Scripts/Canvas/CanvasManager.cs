using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public Transform m_LifeBarParent;
    public CanvasGroup[] m_IngameCanvas;
    public CanvasGroup m_BulletMenuCanvas;
    public BulletMenu m_BulletMenu;

    public Animator m_WinCanvas;
    public Animator m_LoseCanvas;

    //TODO: Gamecontroller
    private static CanvasManager m_Instance = null;
    public static CanvasManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = GameObject.FindObjectOfType<CanvasManager>();
            }
            return m_Instance;
        }
    }

    private void OnEnable()
    {
        InputManager.Instance.OnStartBacking += ShowIngameMenu;
    }
    private void OnDisable()
    {
        InputManager.Instance.OnStartBacking -= ShowIngameMenu;
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
    public void ShowBulletMenu()
    {
        ShowCanvasGroup(m_BulletMenuCanvas);
        HideCanvasGroup(m_IngameCanvas);
        m_BulletMenu.UpdateBulletMenu();
        SetMenuConfig();
    }
    public void ShowIngameMenu()
    {
        ShowCanvasGroup(m_IngameCanvas);
        HideCanvasGroup(m_BulletMenuCanvas);
        SetIngameConfig();
    }
    public void ExitBulletMenu()
    {
        Player_BulletManager.Instance.Reload();
    }
    public void SetMenuConfig()
    {
        MenuCursor();
        InputManager.Instance.SwitchToMenuActionMap();
        CameraManager.Instance.CameraFixedUpdate();
        Time.timeScale = 0;
    }
    public void SetIngameConfig()
    {
        GameCursor();
        InputManager.Instance.SwitchToPlayerActionMap();
        CameraManager.Instance.CameraLateUpdate();
        Time.timeScale = 1;
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
    public void End(bool win)
    {
        if (win)
        {
            m_WinCanvas.SetTrigger("End");
        }
        else
        {
            m_LoseCanvas.SetTrigger("End");
        }
    }
    #endregion
}
