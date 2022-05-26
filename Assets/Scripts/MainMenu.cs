using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public InputManager m_Inputs;
    public GameObject m_BaseButtons;
    public GameObject m_OptionsMenu;


  
   [SerializeField] bool m_InOptions;
   [SerializeField] int m_Index=0;
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

    private void LeftRotation()
    {

        if (m_InOptions)
            return;
        m_BaseButtons.transform.Rotate(Vector3.forward * 120);

        if (m_Index > 0)
            m_Index--;
        else
            m_Index=2;
    }

    private void RightRotation()
    {
        if (m_InOptions)
            return;
        m_BaseButtons.transform.Rotate(Vector3.forward * -120);

        if (m_Index < 2)
            m_Index++;
        else
            m_Index = 0;
    }

    public void OpenOptions()
    {
        m_OptionsMenu.SetActive(true);
        m_InOptions = true;


    }
    public void CloseOptions()
    {
        m_OptionsMenu.SetActive(false);
        m_InOptions = false;

    }

    private void AcceptMenu()
    {
        if (m_InOptions)
            return;
        switch (m_Index)
        {
            case 0:
                print("start");
                ///TODO: poder iniciar el nivel correspondiendo (nivel 1,2,3...leveldata) - Ainoa
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
                break;
            case 1:
                print("options");
                OpenOptions();
               
                break;
            case 2:
                print("exit");
                Application.Quit();
                break;
            default:
                break;
        }
    }
}
