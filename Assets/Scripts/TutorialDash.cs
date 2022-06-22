using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDash : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private bool dash = true;
    private void OnEnable()
    {
        GameManager.GetManager().GetInputManager().OnStartDashing += Hide;
        GameManager.GetManager().GetInputManager().OnRotatingClockwise += Hide2;
        GameManager.GetManager().GetInputManager().OnRotatingCounterClockwise += Hide2;
    }
    private void OnDisable()
    {
        GameManager.GetManager().GetInputManager().OnStartDashing -= Hide;
        GameManager.GetManager().GetInputManager().OnRotatingClockwise -= Hide2;
        GameManager.GetManager().GetInputManager().OnRotatingCounterClockwise -= Hide2;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Time.timeScale = 0.2f;
            canvas.SetActive(true);
        }
    }
    private void Hide()
    {
        if (dash)
        {
            Time.timeScale = 1f;
            canvas.SetActive(false);
            gameObject.SetActive(false);
        }
    }
    private void Hide2()
    {
        if (!dash)
        {
            Time.timeScale = 1f;
            canvas.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
