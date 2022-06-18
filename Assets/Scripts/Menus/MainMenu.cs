using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public InputManager m_Inputs;
    public GameObject m_BaseButtons;
    public OptionsMenu m_OptionsMenu;
    public GameObject m_Menu, m_Effect;

    private void Start()
    {
        Time.timeScale = 1;
    }

    protected virtual void Options()
    {
        m_Menu.SetActive(false);
        m_OptionsMenu.OpenOptions();
    }
    public virtual void CloseOptions()
    {
        m_Menu.SetActive(true);
        m_OptionsMenu.CloseOptions();
    }

    public void PlayGame()
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
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
}
