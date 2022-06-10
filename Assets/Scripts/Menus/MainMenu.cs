using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public InputManager m_Inputs;
    public GameObject m_BaseButtons;
    public OptionsMenu m_OptionsMenu;
  
    [SerializeField]protected bool m_InOptions;
    [SerializeField] protected bool m_Clocking;
    [SerializeField]protected int m_Index =0;
    public float m_maxTimerClock=0.25f;
    public float m_Speed;

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
        if (m_InOptions || m_Clocking)
            return;
        StartCoroutine(ClockBullets(true));
        //m_BaseButtons.transform.Rotate(Vector3.forward * 120);

        m_Index = m_Index > 0 ? m_Index - 1 : 2;
    }

    protected virtual void RightRotation()
    {
        if (m_InOptions || m_Clocking)
            return;

        StartCoroutine(ClockBullets());
      //  m_BaseButtons.transform.Rotate(Vector3.forward * -120);
        m_Index = m_Index < 2 ? m_Index+1 : 0; 
    }

    protected virtual void Options()
    {
        m_InOptions = true;
        m_OptionsMenu.OpenOptions();
    }
    public virtual void CloseOptions()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        m_InOptions = false;
        m_OptionsMenu.CloseOptions();
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
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
                //SceneLoader.Instance.LoadLevel(1);
                GameManager.GetManager().GetSceneLoader().LoadLevel(1);

                break;
            case 1:
                Options();
                break;
            case 2:
                Application.Quit();
                break;
            default:
                break;
        }
    }

    public IEnumerator ClockBullets(bool left=false)
    {
        float t = 0;
        float rot = m_BaseButtons.transform.localEulerAngles.z;
        float dest = left ? rot + 120 : rot - 120;
        m_Clocking = true;
        while (t < m_maxTimerClock)
        {
            t += Time.unscaledDeltaTime;
            float z = Mathf.Lerp(rot, dest, t / m_maxTimerClock);
            m_BaseButtons.transform.localEulerAngles = new Vector3(0, 0, z);
            yield return null;
        }
        m_Clocking = false;
    }
}
