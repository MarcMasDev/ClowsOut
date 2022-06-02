using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDash : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    private void OnEnable()
    {
        GameManager.GetManager().GetInputManager().OnStartDashing += Hide;
    }
    private void OnDisable()
    {
        GameManager.GetManager().GetInputManager().OnStartDashing -= Hide;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            canvas.SetActive(true);
        }
    }
    private void Hide()
    {
        canvas.SetActive(false);
        gameObject.SetActive(false);
    }
}
