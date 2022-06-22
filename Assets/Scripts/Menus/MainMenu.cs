using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MainMenu : MonoBehaviour
{
    public InputManager m_Inputs;
    public GameObject m_BaseButtons;
    public OptionsMenu m_OptionsMenu;
    public GameObject m_Menu, m_Effect;
    public CanvasGroup m_loading;
    public TMP_Text m_percent;

    public Slider m_loadingSlider;
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
        m_loading.alpha = 1;
        m_loading.blocksRaycasts = true;
        m_loading.interactable = true;
        m_Menu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        yield return new WaitForSecondsRealtime(1.2f);
        GameManager.GetManager().GetSceneLoader().LoadWithLoadingScene(1,true);
    }

    public void OptionsGame()
    {
        Options();
    }

    public virtual void QuitGame()
    {
        Application.Quit();
    }

    public void SetLoadingVar(float value)
    {
        m_loadingSlider.value = value;
        m_percent.text = value + "%";
    }
}
