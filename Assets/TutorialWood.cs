using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWood : MonoBehaviour
{
    private bool woodDeleted = false;
    [SerializeField] private GameObject[] tutorialWoods;
    [SerializeField] private GameObject parent;
    private void OnTriggerEnter(Collider c)
    {
        print("LOL");
        if (c.CompareTag("Explosion"))
        {
            woodDeleted = true;
        }
    }
    private void Update()
    {
        if (woodDeleted)
        {
            for (int i = 0; i < tutorialWoods.Length; i++)
            {
                tutorialWoods[i].SetActive(false);
            }
            parent.SetActive(false);
        }
    }
}
