using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class FMODDolores : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter footstep;
    [SerializeField] private StudioEventEmitter shoot;
    public void FootStep()
    {
        //Sound
        footstep.Play();
    }
    public void Shoot()
    {
        //Sound
        shoot.Play();
    }
}
