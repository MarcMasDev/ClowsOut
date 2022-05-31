using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public UnityEvent m_shock;
    private CinemachineImpulseListener m_NormalCam;
    private CinemachineImpulseListener m_AimCam;
    //private CinemachineImpulseListener m_DashCam;
    Transform m_player;
    [SerializeField]
    float m_MaxDistanceToShake=20f;
    private void Start()
    {
        //Debug.Log("cam "+   gameObject.name);//eñlfkewijoewifj
        //  Shake(1, 1);
        GameManager.GetManager().SetCameraShake(this);
        m_NormalCam = GameManager.GetManager().GetCameraManager().m_ThirdPersonCamera.GetComponent<CinemachineImpulseListener>();
        m_AimCam = GameManager.GetManager().GetCameraManager().m_AimCamera.GetComponent<CinemachineImpulseListener>();
        //m_DashCam = GameManager.GetManager().GetCameraManager().m_DashCamera.GetComponent<CinemachineImpulseListener>();
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
                defaultIntensity = 1 / l_dist;
            }
            else
            {
                defaultIntensity = 0f;
            }
            //cantidad x número de porcentaje / 100

            //print("mod default " + defaultIntensity);

        }
        m_NormalCam.m_Gain = defaultIntensity;
        m_AimCam.m_Gain = defaultIntensity;
        //m_DashCam.m_Gain = defaultIntensity;

        InvokeRepeating("ShockWaveEvent", 0,0 );
        //print("Camara shake, shake "+ time);
        //Se llama por el game manager des de las explosiones (la script donde se llama se llama: PlayParticle)
    }
    void ShockWaveEvent()
    {
        m_shock.Invoke();
    }
}
