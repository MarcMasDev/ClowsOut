using UnityEngine;

[CreateAssetMenu(fileName = "OptionsData", menuName = "Menu/SO_OptionsData", order = 1)]
public class Options : ScriptableObject
{
    //var with default values.
    public int m_FPS=60;
    public bool m_Fullscreen = true;
    public bool m_Vysnc = false; //false = 0; true=1
    public int m_IndexResolution=16;

    public string m_PathMaster="vca:/Master";
    public string m_PathMusic = "vca:/Music";
    public string m_PathSFX = "vca:/SFX";
    public bool m_GameMuted = false;

   
    [Range(0.0001f, 1f)] public float m_MasterVolume;
    [Range(0.0001f, 1f)] public float m_MusicVolume;
    [Range(0.0001f, 1f)] public float m_SFXVolume;
}
