using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public UnityEvent m_shock;
    public CinemachineImpulseListener m_NormalCam;
    public CinemachineImpulseListener m_AimCam;
    
    Transform m_player;
    [SerializeField]
    float m_MaxDistanceToShake=20f;
    [SerializeField]
    float m_MaxIntensity;
    private void Start()
    {
        //Debug.Log("cam "+   gameObject.name);//e?lfkewijoewifj
        //  Shake(1, 1);
        GameManager.GetManager().SetCameraShake(this);
        m_NormalCam = GameManager.GetManager().GetCameraManager().m_MediumCamera.GetComponent<CinemachineImpulseListener>();
        m_AimCam = GameManager.GetManager().GetCameraManager().m_AimCamera.GetComponent<CinemachineImpulseListener>();
        
        m_player = GameManager.GetManager().GetPlayer().transform;
    }
    public void Shake(float defaultIntensity, float time, Transform point = null)
    {
        if(point != null)
        {
            float l_dist = Vector3.Distance(point.position, m_player.position);
            //print("dist cam " + l_dist);
            if (l_dist < m_MaxDistanceToShake)
            {
                defaultIntensity = (m_MaxIntensity) - ((((l_dist) * m_MaxIntensity) / m_MaxDistanceToShake));
                //defaultIntensity = 1 / l_dist;
                //defaultIntensity= Mathf.Clamp(defaultIntensity,0, 0.7f);
            }
            else
            {
                defaultIntensity = 0f;
            }
            //cantidad x n?mero de porcentaje / 100

            //print("mod default " + defaultIntensity);

        }
        m_NormalCam.m_Gain = defaultIntensity;
        m_AimCam.m_Gain = defaultIntensity;

        ShockWaveEvent();
        //Invoke("ShockWaveEvent", -1f );
        //print("Camara shake, shake "+ time);
        //Se llama por el game manager des de las explosiones (la script donde se llama se llama: PlayParticle)
    }
    void ShockWaveEvent()
    {
        m_shock.Invoke();
    }
}
