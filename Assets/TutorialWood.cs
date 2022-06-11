using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWood : MonoBehaviour, IRestart
{
    private bool woodDeleted = false;
    [SerializeField] private GameObject parent;

    public void AddRestartElement()
    {
        GameManager.GetManager().GetRestartManager().addRestartElement(this);
    }

    public void Restart()
    {
        woodDeleted = false;
        parent.SetActive(true);
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Explosion"))
        {
            StartCoroutine(Delaydestroy());
        }
    }
    private void Start()
    {
        AddRestartElement();
    }
    private IEnumerator Delaydestroy()
    {
        yield return new WaitForSeconds(0.2f);
        parent.SetActive(false);
    }
}
