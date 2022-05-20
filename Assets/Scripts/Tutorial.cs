using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject toShow;
    [SerializeField] private float tutorialIndex = 0;
    private void Start()
    {
        toShow.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Time.timeScale = 0.1f;
            if (toShow)
                toShow.SetActive(true);
        }
    }

    private void Update()
    {
        switch (tutorialIndex)
        {
            case 0:
                //if (Input.GetMouseButtonDown(0))
                //{
                //    //Time.timeScale = 1f;
                //    if (toShow)
                //        toShow.SetActive(false);
                //}
                break;
            default:

                break;
        }
    }
}
