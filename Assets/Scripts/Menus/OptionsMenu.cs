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

    private FMOD.Studio.VCA m_MasterVCA;
    private FMOD.Studio.VCA m_MusicVCA;
    private FMOD.Studio.VCA m_SFXVCA;
    public Slider m_MasterSlider;
    public Slider m_MusicSlider;
    public Slider m_SFXSlider;
    public TMP_Text m_Master;
    public TMP_Text m_Music;
    public TMP_Text m_SFX;


    int m_IndexResolut;
    public CanvasGroup m_CanvasGroup;
    
    private void Awake()
    {
        print(FMODUnity.RuntimeManager.GetVCA("vca:/Master"));
        m_CanvasGroup = GetComponent<CanvasGroup>();
        GameManager.GetManager().SetOptions(this);
    }

    public void Start()
    {
        m_MasterVCA = FMODUnity.RuntimeManager.GetVCA(m_OptionsData.m_PathMaster);
        m_MusicVCA = FMODUnity.RuntimeManager.GetVCA(m_OptionsData.m_PathMusic);
        m_SFXVCA = FMODUnity.RuntimeManager.GetVCA(m_OptionsData.m_PathSFX);
        LoadDataSO();
    }

    #region SetVolumes
    public void SetMasterVolume(float volume)
    {
        m_OptionsData.m_MasterVolume = (int)volume;
        m_Master.text = m_OptionsData.m_MasterVolume.ToString();
        m_MasterVCA.setVolume(m_OptionsData.m_MasterVolume);
    }
    public void SetMusicVolume(float volume)
    {
        m_OptionsData.m_MusicVolume = (int)volume;
        m_Music.text = m_OptionsData.m_MusicVolume.ToString();
        m_MusicVCA.setVolume(m_OptionsData.m_MusicVolume);
    }
    public void SetSFXVolume(float volume)
    {
        m_OptionsData.m_SFXVolume = (int)volume;
        m_SFX.text = m_OptionsData.m_SFXVolume.ToString();
        m_SFXVCA.setVolume(m_OptionsData.m_SFXVolume);
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

        SetSFXVolume(m_OptionsData.m_MasterVolume);
        SetSFXVolume(m_OptionsData.m_MusicVolume);
        SetSFXVolume(m_OptionsData.m_SFXVolume);

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
