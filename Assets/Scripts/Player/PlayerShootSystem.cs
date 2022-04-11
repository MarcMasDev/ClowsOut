using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootSystem : MonoBehaviour
{
    [Header("Ammunition Capacity")]
    [Range(0, 6)] public int m_AmmunitionCapacity;
    [Header("Rate Of Fire")]
    [Range(0, 5.0f)] public float m_RateOfFire;
    [Header("Recoil")]
    [Range(0, 15.0f)] public float m_VerticalMaximumRecoil;
    [Range(0, 15.0f)] public float m_VerticalMinimumRecoil;
    [Range(0, 3.0f)] public float m_HorizontalMaximumRecoil;
    [Range(0, -3.0f)] public float m_HorizontalMinimumRecoil;
    [Header("Dispersion")]
    [Range(0, 30.0f)] public float m_PerShotDispersion;
    [Range(0, 30.0f)] public float m_CamRotationDispersion;
    [Range(0, 30.0f)] public float m_MaxDispersion;
    [Range(0, 30.0f)] public float m_MinDispersion;
    [Range(0, 30.0f)] public float m_MovementDispersion;

    public float m_Dispersion;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
