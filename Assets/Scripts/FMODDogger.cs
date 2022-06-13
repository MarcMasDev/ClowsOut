using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODDogger : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter hitSound;
    [SerializeField] private StudioEventEmitter footstepSound;
    public void FootStep()
    {
        //footstepSound.Play();
    }
    public void Hit()
    {
        hitSound.Play();
    }
}
