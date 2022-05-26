using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public InputManager m_Inputs;
    public GameObject m_BaseButtons;

    int m_Index=0;
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
        m_BaseButtons.transform.Rotate(Vector3.forward * 120);

        if (m_Index > 0)
            m_Index--;
        else
            m_Index=2;
    }

    private void RightRotation()
    {
        m_BaseButtons.transform.Rotate(Vector3.forward * -120);

        if (m_Index < 2)
            m_Index++;
        else
            m_Index = 0;
    }

    private void AcceptMenu()
    {
        switch (m_Index)
        {
            case 0:
                print("start");
                ///TODO: poder iniciar el nivel correspondiendo (nivel 1,2,3...leveldata) - Ainoa
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
                break;
            case 1:
                print("exit");
                break;
            case 2:
                print("options");
                break;
            default:
                break;
        }

    }
}
