using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveObjectOnRestart : MonoBehaviour, IRestart
{
    public void AddRestartElement()
    {
        GameManager.GetManager().GetRestartManager().addRestartElement(this);
    }

    public void Restart()
    {
        gameObject.SetActive(true);
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        AddRestartElement();
    }

}
