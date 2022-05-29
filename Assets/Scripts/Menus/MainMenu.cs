using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public InputManager m_Inputs;
    public GameObject m_BaseButtons;
    public GameObject m_OptionsMenu;
  
    protected bool m_InOptions;
    protected int m_Index =0;
    private void OnEnable()
    {
        m_Inputs.OnStartRightRotation += RightRotation;
        m_Inputs.OnStartLeftRotation += LeftRotation;
        m_Inputs.OnStartAccept += AcceptMenu;
    }

    private void OnDisable()
    {
        m_Inputs.OnStartRightRotation -= RightRotation;
        m_Inputs.OnStartLeftRotation -= LeftRotation;
        m_Inputs.OnStartAccept -= AcceptMenu;
    }

    protected virtual void LeftRotation()
    {
        if (m_InOptions)
            return;
        m_BaseButtons.transform.Rotate(Vector3.forward * 120);

        if (m_Index > 0)
            m_Index--;
        else
            m_Index=2;
    }

    protected virtual void RightRotation()
    {
        if (m_InOptions)
            return;
        m_BaseButtons.transform.Rotate(Vector3.forward * -120);

        if (m_Index < 2)
            m_Index++;
        else
            m_Index = 0;
    }

    protected virtual void OpenOptions()
    {
        GameManager.GetManager().GetOptionsMenu().gameObject.SetActive(true);
        m_InOptions = true;
    }
    public virtual void CloseOptions()
    {
        GameManager.GetManager().GetOptionsMenu().gameObject.SetActive(false);
        m_InOptions = false;
    }

    protected virtual void AcceptMenu()
    {
        if (m_InOptions)
            return;
        switch (m_Index)
        {
            case 0:
                ///TODO: poder iniciar el nivel correspondiendo (nivel 1,2,3...leveldata) - Ainoa
                GameManager.GetManager().GetLevelData().m_GameStarted = true;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
                break;
            case 1:
                OpenOptions();
                break;
            case 2:
                Application.Quit();
                break;
            default:
                break;
        }
    }
}
