using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public Dropdown m_ResolutionsDropdown;
   // public Slider m_FOV;
    public Slider m_FrameRate;
    public TMP_Text m_FPStext;
    public Toggle m_FullScreen;
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
        LoadData();
        //GameManager.GetManager().GetLevelData().SaveOptions(Mathf.RoundToInt(m_FOV.value), Mathf.RoundToInt(m_FrameRate.value), Screen.fullScreen, QualitySettings.vSyncCount, currentIndex);
    }
    //fullscreen
    public void SetFullscreen(bool mode)
    {
        Screen.fullScreen = mode;
        m_FullScreen.isOn = mode;
        GameManager.GetManager().GetLevelData().m_Fullscreen = mode;
        //SaveData();
    }

    public void SetVSync(bool mode)
    {
        QualitySettings.vSyncCount = mode ? 1 : 0;
        GameManager.GetManager().GetLevelData().m_VYsnc = QualitySettings.vSyncCount;
        //SaveData();
    }

    public void SetResolution(int index)
    {
        GameManager.GetManager().GetLevelData().m_ResolutionChanged = true;
        Screen.SetResolution(m_Resolutions[index].width, m_Resolutions[index].height, Screen.fullScreen);
        GameManager.GetManager().GetLevelData().m_ResolutionIndex = index;
        //SaveData();
    }

    public void SetFrameRate()
    {
        Application.targetFrameRate = Mathf.RoundToInt(m_FrameRate.value);
        m_FPStext.text = Application.targetFrameRate + " FPS";
        GameManager.GetManager().GetLevelData().m_FPS = Mathf.RoundToInt(m_FrameRate.value);
        //SaveData();
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
        GameManager.GetManager().GetLevelData().SaveOptions(/*Mathf.RoundToInt(m_FOV.value),*/ Mathf.RoundToInt(m_FrameRate.value), GameManager.GetManager().GetLevelData().m_Fullscreen, GameManager.GetManager().GetLevelData().m_VYsnc);
    }

    public void LoadData()
    {
        m_FrameRate.value = GameManager.GetManager().GetLevelData().m_FPS != m_FrameRate.value ? GameManager.GetManager().GetLevelData().m_FPS : m_FrameRate.value;
        SetFrameRate();

        //m_FOV.value = GameManager.GetManager().GetLevelData().m_FOV != m_FOV.value ? GameManager.GetManager().GetLevelData().m_FOV : m_FOV.value;
        //SetFOV();
        m_FullScreen.isOn = GameManager.GetManager().GetLevelData().m_Fullscreen != m_FullScreen.isOn ? GameManager.GetManager().GetLevelData().m_Fullscreen : m_FullScreen.isOn;
        SetFullscreen(GameManager.GetManager().GetLevelData().m_Fullscreen);

        m_Resolutions = Screen.resolutions;
        m_ResolutionsDropdown.ClearOptions();

        for (int i = 0; i < m_Resolutions.Length; i++)
        {
            string resol = m_Resolutions[i].width + " x " + m_Resolutions[i].height;
            options.Add(resol);

            if (!GameManager.GetManager().GetLevelData().m_ResolutionChanged && (m_Resolutions[i].width == Screen.currentResolution.width && m_Resolutions[i].height == Screen.currentResolution.height))
            {
                m_IndexResolut = i;
            }
            else
            {
                m_IndexResolut = GameManager.GetManager().GetLevelData().m_ResolutionIndex;
            }
        }

        m_ResolutionsDropdown.AddOptions(options);
        m_ResolutionsDropdown.value = m_IndexResolut;
        m_ResolutionsDropdown.RefreshShownValue();
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
