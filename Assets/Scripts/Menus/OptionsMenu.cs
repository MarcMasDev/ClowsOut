using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public Dropdown m_ResolutionsDropdown;
    public Options m_OptionsData;
    public Slider m_FrameRate;
    public TMP_Text m_FPStext;
    public Toggle m_FullScreen;
    public Toggle m_VSync;
    List<string> options = new List<string>();
    Resolution[] m_Resolutions;

    int m_IndexResolut;
    public CanvasGroup m_CanvasGroup;
    
    private void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
        GameManager.GetManager().SetOptions(this);
    }

    public void Start()
    {
        LoadDataSO();
    }
    public void SetFullscreen(bool mode)
    {
        m_OptionsData.m_Fullscreen = mode;
        Screen.fullScreen = m_OptionsData.m_Fullscreen;
    }

    public void SetVSync(bool mode)
    {
        m_OptionsData.m_Vysnc = mode;
        QualitySettings.vSyncCount = m_OptionsData.m_Vysnc ? 1 : 0;
    }

    public void SetResolution(int index)
    {
        m_OptionsData.m_IndexResolution = index;
        Screen.SetResolution(m_Resolutions[index].width, m_Resolutions[index].height, Screen.fullScreen);
    }

    public void SetFrameRate()
    {
        m_OptionsData.m_FPS = Mathf.RoundToInt(m_FrameRate.value);
        m_FPStext.text = m_OptionsData.m_FPS + " FPS";
        Application.targetFrameRate = Mathf.RoundToInt(m_OptionsData.m_FPS);
    }
    public void LoadDataSO()
    {
        m_FrameRate.value = m_OptionsData.m_FPS;
        Application.targetFrameRate = m_OptionsData.m_FPS;

        m_FullScreen.isOn = m_OptionsData.m_Fullscreen;
        Screen.fullScreen = m_OptionsData.m_Fullscreen;

        m_VSync.isOn = m_OptionsData.m_Vysnc;
        QualitySettings.vSyncCount = m_OptionsData.m_Vysnc ? 1 : 0;

        m_Resolutions = Screen.resolutions;
        m_ResolutionsDropdown.ClearOptions();

        for (int i = 0; i < m_Resolutions.Length; i++)
        {
            string resol = m_Resolutions[i].width + " x " + m_Resolutions[i].height;
            options.Add(resol);

            //REVISAR AINOA >>>>
            if (m_Resolutions[i].width == Screen.currentResolution.width && m_Resolutions[i].height == Screen.currentResolution.height)
            {
                m_OptionsData.m_IndexResolution = i;
                break;
            }
            else
            {
                m_IndexResolut = m_OptionsData.m_IndexResolution;
            }
            //<<<<
            m_IndexResolut = m_OptionsData.m_IndexResolution;
        }

        m_ResolutionsDropdown.AddOptions(options);
        m_ResolutionsDropdown.value = m_IndexResolut;
        m_ResolutionsDropdown.RefreshShownValue();
    }

    public void CloseOptions()
    {
        m_CanvasGroup.alpha = 0;
        m_CanvasGroup.interactable = false;
        m_CanvasGroup.blocksRaycasts = false;
    }
    public void OpenOptions()
    {
        m_CanvasGroup.alpha = 1;
        m_CanvasGroup.interactable = true;
        m_CanvasGroup.blocksRaycasts = true;
    }
}
