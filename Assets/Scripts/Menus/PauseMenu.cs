using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MainMenu
{
    public GameObject m_CloseWarning;

    private void OnEnable()
    {
       
        GameManager.GetManager().GetInputManager().OnStartRightRotation += RightRotation;
        GameManager.GetManager().GetInputManager().OnStartLeftRotation += LeftRotation;
        GameManager.GetManager().GetInputManager().OnStartAccept += AcceptMenu;
    }

    private void OnDisable()
    {
        GameManager.GetManager().GetInputManager().OnStartRightRotation -= RightRotation;
        GameManager.GetManager().GetInputManager().OnStartLeftRotation -= LeftRotation;
        GameManager.GetManager().GetInputManager().OnStartAccept -= AcceptMenu;
    }

    private void Warning()
    {
        m_InOptions = true;
        m_CloseWarning.SetActive(true);
    }

    public void CloseWarning() 
    {
        m_InOptions = false;
        m_CloseWarning.SetActive(false);
    }
    public void QuitGame()
    {
        SceneManager.LoadScene("Menu");
    }

    public override void CloseOptions()
    {
        base.CloseOptions();
    }
    protected override void AcceptMenu()
    {
        if (m_InOptions)
            return;
        switch (m_Index)
        {
            case 0:
                GameManager.GetManager().GetCanvasManager().ShowIngameMenu();
                break;
            case 1:
                OpenOptions();
                break;
            case 2:
                Warning();
                break;
            default:
                break;
        }
    }
}
