using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class FMODDolores : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter footstep;
    public void FootStep()
    {
        //Sound
        footstep.Play();
    }
    public void Shoot()
    {
        //Sound

    }
}
