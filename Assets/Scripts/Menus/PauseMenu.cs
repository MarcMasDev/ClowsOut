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
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        m_InOptions = true;
        m_CloseWarning.SetActive(true);
    }

    public void CloseWarning() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; //**hardcoded
        m_InOptions = false;
        m_CloseWarning.SetActive(false);
    }
    public void QuitGame()
    {
        GameManager.GetManager().GetLevelData().m_GameStarted = false;
        SceneManager.LoadScene("Menu");
    }

    public override void CloseOptions()
    {
        base.CloseOptions();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected override void LeftRotation()
    {
        base.LeftRotation();
    }

    protected override void RightRotation()
    {
        base.RightRotation();
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
                base.Options();
                break;
            case 2:
                Warning();
                break;
            default:
                break;
        }
    }
}
