using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
    

public class OptionsMenu : MonoBehaviour
{
    public Dropdown m_ResolutionsDropdown;
    public Slider m_FrameRate;
    public TMP_Text m_FPStext;
    List<string> options = new List<string>();
    Resolution[] m_Resolutions;
    private void Start()
    {
        m_FrameRate.value = 60;
        SetFrameRate();

        m_Resolutions = Screen.resolutions;
        m_ResolutionsDropdown.ClearOptions();

        int currentIndex=0;
        for (int i = 0; i < m_Resolutions.Length; i++)
        {
            string resol = m_Resolutions[i].width + " x " + m_Resolutions[i].height;
            options.Add(resol);

            if (m_Resolutions[i].width == Screen.currentResolution.width && m_Resolutions[i].height == Screen.currentResolution.height)
            {
                currentIndex = i;
            }
        }

        m_ResolutionsDropdown.AddOptions(options);
        m_ResolutionsDropdown.value = currentIndex;
        m_ResolutionsDropdown.RefreshShownValue();
    }

    //fullscreen
    public void SetFullscreen(bool mode)
    {
        Screen.fullScreen = mode;
    }

    public void SetVSync(bool mode)
    {
        QualitySettings.vSyncCount = mode ? 1 : 0; 
    }

    public void SetResolution(int index)
    {
        Screen.SetResolution(m_Resolutions[index].width, m_Resolutions[index].height,Screen.fullScreen);
    }

    public void SetFrameRate()
    {
        Application.targetFrameRate = Mathf.RoundToInt(m_FrameRate.value);
        m_FPStext.text = Application.targetFrameRate + " FPS";
    }
}
