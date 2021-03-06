using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject m_CloseWarning;
    public CanvasGroup m_PauseMenu;
    public CanvasGroup m_Options;
    public GameObject m_buttons;

    private void Awake()
    {
        GameManager.GetManager().SetPauseMenu(this);
    }
    public void Warning()
    {
        m_buttons.SetActive(false);
        m_CloseWarning.SetActive(true);
    }

    public void CloseWarning()
    {
        m_buttons.SetActive(true);
        m_CloseWarning.SetActive(false);
    }

    public  void QuitGame()
    {
        GameManager.GetManager().GetLevelData().m_GameStarted = false;
        GameManager.GetManager().GetSceneLoader().LoadLevel(0);
    }

    public  void CloseOptions()
    {
        m_PauseMenu.alpha = 1;
        m_PauseMenu.interactable = true;
        m_PauseMenu.blocksRaycasts = true;

        m_Options.alpha = 0;
        m_Options.interactable = false;
        m_Options.blocksRaycasts = false;
    }
    public void CloseAllOptions()
    {
        m_Options.alpha = 0;
        m_Options.interactable = false;
        m_Options.blocksRaycasts = false;
    }

    public void OpenOptions()
    {
        m_Options.alpha = 1;
        m_Options.interactable = true;
        m_Options.blocksRaycasts = true;

        m_PauseMenu.alpha = 0;
        m_PauseMenu.interactable = false;
        m_PauseMenu.blocksRaycasts = false;
    }

    public void ResumeGame() 
    {
        GameManager.GetManager().GetCanvasManager().ShowIngameMenu();
    }
}
