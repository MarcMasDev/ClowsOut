using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplainShow : MonoBehaviour
{
    private Animator a;
    private void Start()
    {
        a = GetComponent<Animator>();
    }
    public void Show()
    {
        a.SetBool("Show", true);
    }
    public void Hide()
    {
        a.SetBool("Show", false);
    }
}
