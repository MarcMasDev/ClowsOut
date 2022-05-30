using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tp_PlayFXOnSpawn : MonoBehaviour
{
    [SerializeField] private Animator fx;

    public void PlayAnim()
    {
        fx.gameObject.SetActive(true);
        fx.SetBool("TP", true);
    }
}
