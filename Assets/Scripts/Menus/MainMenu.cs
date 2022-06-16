using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public InputManager m_Inputs;
    public GameObject m_BaseButtons;
    public OptionsMenu m_OptionsMenu;
    public GameObject m_Menu, m_Effect;
    public Animator m_Dolores, m_Dogger;

    [SerializeField] protected bool m_InOptions;
    [SerializeField] protected bool m_Clocking;
    [SerializeField] protected int m_Index = 0;
    public float m_maxTimerClock = 0.25f;
    public float m_Speed;

    private void OnEnable()
    {
        m_Inputs.OnStartRightRotation += RightRotation;
        m_Inputs.OnStartLeftRotation += LeftRotation;
    }

    private void OnDisable()
    {
        m_Inputs.OnStartRightRotation -= RightRotation;
        m_Inputs.OnStartLeftRotation -= LeftRotation;
    }

    protected virtual void LeftRotation()
    {
        if (m_InOptions || m_Clocking)
            return;
        StartCoroutine(ClockBullets(true));
        m_Index = m_Index > 0 ? m_Index - 1 : 2;
    }

    protected virtual void RightRotation()
    {
        if (m_InOptions || m_Clocking)
            return;

        StartCoroutine(ClockBullets());
        m_Index = m_Index < 2 ? m_Index + 1 : 0;
    }

    protected virtual void Options()
    {
        m_InOptions = true;
        m_Menu.SetActive(false);
        m_OptionsMenu.OpenOptions();
    }
    public virtual void CloseOptions()
    {
        m_InOptions = false;
        m_Menu.SetActive(true);
        m_OptionsMenu.CloseOptions();
    }

    public void PlayGame()
    {
        if (m_InOptions)
            return;

        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        m_Dolores.Play("Shoot");
        m_Effect.SetActive(true);
        yield return null;
        m_Dogger.Play("Death");
        yield return new WaitForSecondsRealtime(1.2f);
        GameManager.GetManager().GetSceneLoader().LoadWithLoadingScene(1);
    }

    public void OptionsGame()
    {
        Options();
    }

    public virtual void QuitGame()
    {
        Application.Quit();
    }

    public IEnumerator ClockBullets(bool left = false)
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
