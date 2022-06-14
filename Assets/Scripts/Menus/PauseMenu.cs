using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject m_CloseWarning;
    public CanvasGroup m_PauseMenu;

    private void OnEnable()
    {
        //GameManager.GetManager().GetInputManager().OnStartRightRotation += RightRotation;
        //GameManager.GetManager().GetInputManager().OnStartLeftRotation += LeftRotation;
        //GameManager.GetManager().GetInputManager().OnStartAccept += AcceptMenu;
    }

    private void OnDisable()
    {
        //GameManager.GetManager().GetInputManager().OnStartRightRotation -= RightRotation;
        //GameManager.GetManager().GetInputManager().OnStartLeftRotation -= LeftRotation;
        //GameManager.GetManager().GetInputManager().OnStartAccept -= AcceptMenu;
    }

    public void Warning()
    {
        m_CloseWarning.SetActive(true);
        GameManager.GetManager().GetCanvasManager().MenuCursor();
    }

    public void CloseWarning()
    {
        m_CloseWarning.SetActive(false);
        GameManager.GetManager().GetCanvasManager().GameCursor();
    }

    public  void QuitGame()
    {
        GameManager.GetManager().GetLevelData().m_GameStarted = false;
        GameManager.GetManager().GetSceneLoader().LoadWithLoadingScene(0);
    }

    public  void CloseOptions()
    {

        //base.CloseOptions();
    }

    

    //protected override void LeftRotation()
    //{
    //    base.LeftRotation();
    //}


    //protected override void RightRotation()
    //{
    //    base.RightRotation();
    //}


    public void ResumeGame() 
    {
        GameManager.GetManager().GetCanvasManager().ShowIngameMenu();
    }
    //protected void AcceptMenu()
    //{
    //    if (m_InOptions)
    //        return;
        
    //    switch (m_Index)
    //    {
    //        case 0:
    //            GameManager.GetManager().GetCanvasManager().ShowIngameMenu();
    //            break;
    //        case 1:
    //            base.Options();
    //            break;
    //        case 2:
    //            Warning();
    //            break;
    //        default:
    //            break;
    //    }
    //}
}
