using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using FMOD.Studio;

public class OptionsMenu : MonoBehaviour
{
    public Dropdown m_ResolutionsDropdown;
    public Options m_OptionsData;
    public Slider m_FrameRate;
    public TMP_Text m_FPStext;
    public Toggle m_FullScreen;
    public Toggle m_VSync;
    public Toggle m_Muted;
    List<string> options = new List<string>();
    Resolution[] m_Resolutions;

    private VCA m_MasterVCA;
    private VCA m_MusicVCA;
    private VCA m_SFXVCA;
    public Slider m_MasterSlider;
    public Slider m_MusicSlider;
    public Slider m_SFXSlider;

    int m_IndexResolut;
    public CanvasGroup m_CanvasGroup;
    
    private void Awake()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();
        GameManager.GetManager().SetOptions(this);
    }

    public void Start()
    {
        m_MasterVCA = RuntimeManager.GetVCA(m_OptionsData.m_PathMaster);
        m_MusicVCA = RuntimeManager.GetVCA(m_OptionsData.m_PathMusic);
        m_SFXVCA = RuntimeManager.GetVCA(m_OptionsData.m_PathSFX);
        LoadDataSO();
    }

    #region SetVolumes
    public void SetMasterVolume()
    {
        m_OptionsData.m_MasterVolume = m_MasterSlider.value;//Mathf.Clamp(m_MasterSlider.value, 0.0001f, 1);
        m_MasterVCA.setVolume(m_MasterSlider.value/*Mathf.Log10(m_OptionsData.m_MasterVolume) * 20*/);
        m_Muted.isOn = m_OptionsData.m_GameMuted = false;
    }
    public void SetMusicVolume()
    {
        m_OptionsData.m_MusicVolume = m_MusicSlider.value;// Mathf.Clamp(m_MusicSlider.value,0.0001f,1);
        m_MusicVCA.setVolume(m_MusicSlider.value/*Mathf.Log10(m_OptionsData.m_MusicVolume) * 20*/);
        m_Muted.isOn = m_OptionsData.m_GameMuted = false;

    }
    public void SetSFXVolume()
    {
        m_OptionsData.m_SFXVolume = m_SFXSlider.value; //Mathf.Clamp(m_SFXSlider.value, 0.0001f, 1);
        m_SFXVCA.setVolume(m_SFXSlider.value/*Mathf.Log10(m_OptionsData.m_SFXVolume) * 20*/);
        m_Muted.isOn = m_OptionsData.m_GameMuted = false;
    }

    public void SetMuted(bool muted)
    {
        if (muted)
            m_MasterVCA.setVolume(0);
        else
            m_MasterVCA.setVolume(m_OptionsData.m_SFXVolume);

        
    }
    #endregion
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
        //m_FPStext.text = m_OptionsData.m_FPS + " FPS";
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

        m_Muted.isOn = m_OptionsData.m_GameMuted;

        m_MasterSlider.value = m_OptionsData.m_MasterVolume;
        m_MusicSlider.value = m_OptionsData.m_MusicVolume;
        m_SFXSlider.value = m_OptionsData.m_SFXVolume;
        SetSFXVolume();
        SetSFXVolume();
        SetSFXVolume();

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
