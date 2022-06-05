using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public Dropdown m_ResolutionsDropdown;
    public Options m_OptionsData;
   // public Slider m_FOV;
    public Slider m_FrameRate;
    public TMP_Text m_FPStext;
    public Toggle m_FullScreen;
    public Toggle m_VSync;
   // public TMP_Text m_FOVtext;
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
       // LoadData();
        //GameManager.GetManager().GetLevelData().SaveOptions(Mathf.RoundToInt(m_FOV.value), Mathf.RoundToInt(m_FrameRate.value), Screen.fullScreen, QualitySettings.vSyncCount, currentIndex);
    }
    //fullscreen
    public void SetFullscreen(bool mode)
    {
        m_OptionsData.m_Fullscreen = mode;
        Screen.fullScreen = m_OptionsData.m_Fullscreen;
    }

    public void SetVSync(bool mode)
    {
        m_OptionsData.m_Vysnc = mode;
        QualitySettings.vSyncCount = m_OptionsData.m_Vysnc ? 1 : 0;

        //QualitySettings.vSyncCount = mode ? 1 : 0;
        //GameManager.GetManager().GetLevelData().m_VYsnc = QualitySettings.vSyncCount;
        ////SaveData();
    }

    public void SetResolution(int index)
    {
        m_OptionsData.m_IndexResolution = index;
        Screen.SetResolution(m_Resolutions[index].width, m_Resolutions[index].height, Screen.fullScreen);
        //GameManager.GetManager().GetLevelData().m_ResolutionChanged = true;
        //Screen.SetResolution(m_Resolutions[index].width, m_Resolutions[index].height, Screen.fullScreen);
        //GameManager.GetManager().GetLevelData().m_ResolutionIndex = index;
        ////SaveData();
    }

    public void SetFrameRate()
    {
        m_OptionsData.m_FPS = Mathf.RoundToInt(m_FrameRate.value);
        m_FPStext.text = m_OptionsData.m_FPS + " FPS";
        Application.targetFrameRate = Mathf.RoundToInt(m_OptionsData.m_FPS);
        
        GameManager.GetManager().GetLevelData().m_FPS = Mathf.RoundToInt(m_FrameRate.value);
        ////SaveData();
    }

    //public void SetFOV()
    //{
    //    GameManager.GetManager().GetLevelData().m_FOV = Mathf.RoundToInt(m_FOV.value);
    //    m_FOVtext.text = GameManager.GetManager().GetLevelData().m_FOV + " FOV";
    //    //SaveData();
    //}


    //need to be deleted
    public void SaveData()
    {
       // GameManager.GetManager().GetLevelData().SaveOptions(/*Mathf.RoundToInt(m_FOV.value),*/ Mathf.RoundToInt(m_FrameRate.value), GameManager.GetManager().GetLevelData().m_Fullscreen, GameManager.GetManager().GetLevelData().m_VYsnc);
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

            //m_IndexResolut = m_OptionsData.m_IndexResolution;
            if (!GameManager.GetManager().GetLevelData().m_ResolutionChanged && (m_Resolutions[i].width == Screen.currentResolution.width && m_Resolutions[i].height == Screen.currentResolution.height))
            {
                m_OptionsData.m_IndexResolution= i;
            }
            //else
            //{
            //    m_IndexResolut = m_OptionsData.m_IndexResolution; //GameManager.GetManager().GetLevelData().m_ResolutionIndex;
            //}
            m_IndexResolut = m_OptionsData.m_IndexResolution;
        }

        m_ResolutionsDropdown.AddOptions(options);
        m_ResolutionsDropdown.value = m_IndexResolut;
        m_ResolutionsDropdown.RefreshShownValue();
    }

    public void LoadData()
    {
        m_FrameRate.value = GameManager.GetManager().GetLevelData().m_FPS != m_FrameRate.value ? GameManager.GetManager().GetLevelData().m_FPS : m_FrameRate.value;
        SetFrameRate();

        //m_FOV.value = GameManager.GetManager().GetLevelData().m_FOV != m_FOV.value ? GameManager.GetManager().GetLevelData().m_FOV : m_FOV.value;
        //SetFOV();
        m_FullScreen.isOn = GameManager.GetManager().GetLevelData().m_Fullscreen != m_FullScreen.isOn ? GameManager.GetManager().GetLevelData().m_Fullscreen : m_FullScreen.isOn;
        SetFullscreen(GameManager.GetManager().GetLevelData().m_Fullscreen);

       
    }

    private void Update()
    {
        //print(Application.targetFrameRate);
    }

    public void CloseOptions()
    {
        m_CanvasGroup.alpha = 0;
        m_CanvasGroup.interactable = false;
        m_CanvasGroup.blocksRaycasts = false;
        SaveData();
    }
    public void OpenOptions()
    {
        m_CanvasGroup.alpha = 1;
        m_CanvasGroup.interactable = true;
        m_CanvasGroup.blocksRaycasts = true;
    }
}
