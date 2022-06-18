using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.Rendering;

public class OptionsMenu : MonoBehaviour
{
    public TMP_Dropdown m_ResolutionsDropdown;
    public TMP_Dropdown m_QualityDropdown;
    public Options m_OptionsData;
    public Slider m_FrameRate;
    public TMP_Text m_TextFrame;
    public GameObject m_HudText;
    public Toggle m_FullScreen;
    public Toggle m_VSync;
    public Toggle m_Muted;
    List<string> options = new List<string>();
    Resolution[] m_Resolutions = new Resolution[0];

    private VCA m_MasterVCA;
    private VCA m_MusicVCA;
    private VCA m_SFXVCA;
    public Slider m_MasterSlider;
    public Slider m_MusicSlider;
    public Slider m_SFXSlider;
    public Slider m_HudOpacity;

    int m_IndexResolut;
    private CanvasGroup m_CanvasGroup;
    private TMP_Text m_Text;
    private GameObject m_StartRebindObject;
    private GameObject m_WaitingForInput;
    private int m_Index;
    bool m_ThereIsPauseMenu;
    private InputActionRebindingExtensions.RebindingOperation m_rebindingOperation;

    public RenderPipelineAsset[] m_QualityLevels;
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

        m_ThereIsPauseMenu = GameManager.GetManager().GetPauseMenu() != null;
        if (m_ThereIsPauseMenu)
        {
            m_HudText.SetActive(false);
        }
    }

    #region Rebind

    public void StartRebinding(GetRebindInput input)
    {
        m_StartRebindObject = input.m_Button;
        m_WaitingForInput = input.m_WaitInput;
        m_Text = input.m_Text;
        m_StartRebindObject.SetActive(false);
        m_WaitingForInput.SetActive(true);

        input.m_Input.action.Disable();
        m_rebindingOperation = input.m_Input.action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete(input.m_Input)).Start();
    }

    private void RebindComplete(InputActionReference reference)
    {
        int l_BindingIndex = reference.action.GetBindingIndexForControl(reference.action.controls[m_Index]);
        m_Text.text = InputControlPath.ToHumanReadableString(reference.action.bindings[l_BindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        m_rebindingOperation.Dispose();
        m_StartRebindObject.SetActive(true);
        m_WaitingForInput.SetActive(false);
    }
    #endregion
    #region SetVolumes
    public void SetMasterVolume()
    {
        m_OptionsData.m_MasterVolume = m_MasterSlider.value;
        m_MasterVCA.setVolume(m_MasterSlider.value);
        m_Muted.isOn = m_OptionsData.m_GameMuted = false;
    }
    public void SetMusicVolume()
    {
        m_OptionsData.m_MusicVolume = m_MusicSlider.value;
        m_MusicVCA.setVolume(m_MusicSlider.value);
        m_Muted.isOn = m_OptionsData.m_GameMuted = false;

    }
    public void SetSFXVolume()
    {
        m_OptionsData.m_SFXVolume = m_SFXSlider.value;
        m_SFXVCA.setVolume(m_SFXSlider.value);
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
    public void SetOpacity(float opacity)
    {
        m_OptionsData.m_HudOpacity = opacity;
    }

    public void ChangeQualityLevel(int i)
    {
        m_OptionsData.m_QualityLevelIndex = i;
        QualitySettings.SetQualityLevel(m_OptionsData.m_QualityLevelIndex);
        QualitySettings.renderPipeline = m_QualityLevels[m_OptionsData.m_QualityLevelIndex];
    }

    public void SetFrameRate()
    {
        m_OptionsData.m_FPS = Mathf.RoundToInt(m_FrameRate.value);
        m_TextFrame.text = m_OptionsData.m_FPS.ToString();
        Application.targetFrameRate = Mathf.RoundToInt(m_OptionsData.m_FPS);
    }
    public void LoadDataSO()
    {
        if (GameManager.GetManager().GetLevelData().m_GameStarted == false)
        {
            m_HudOpacity.interactable = false;
        }
        else
        {
            m_HudOpacity.interactable = true;
            SetOpacity(m_OptionsData.m_HudOpacity);
        }

        m_TextFrame.text = m_OptionsData.m_FPS.ToString();
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

            if (m_Resolutions[i].width == Screen.currentResolution.width && m_Resolutions[i].height == Screen.currentResolution.height)
            {
                m_OptionsData.m_IndexResolution = i;
            }
            m_IndexResolut = m_OptionsData.m_IndexResolution;
        }

        m_ResolutionsDropdown.AddOptions(options);
        m_ResolutionsDropdown.value = m_IndexResolut;
        m_ResolutionsDropdown.RefreshShownValue();

        ChangeQualityLevel(m_OptionsData.m_QualityLevelIndex);
        m_QualityDropdown.value = m_OptionsData.m_QualityLevelIndex;
    }

    public void CloseOptions()
    {
        if (m_ThereIsPauseMenu)
        {
            GameManager.GetManager().GetPauseMenu().CloseOptions();
        }
        else
        {
            m_CanvasGroup.alpha = 0;
            m_CanvasGroup.interactable = false;
            m_CanvasGroup.blocksRaycasts = false;
        }
    }
    public void OpenOptions()
    {
        if (m_ThereIsPauseMenu)
        {
            GameManager.GetManager().GetPauseMenu().OpenOptions();
        }
        else
        {
            m_CanvasGroup.alpha = 1;
            m_CanvasGroup.interactable = true;
            m_CanvasGroup.blocksRaycasts = true;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }
}
