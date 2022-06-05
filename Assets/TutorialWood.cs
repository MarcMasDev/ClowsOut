using UnityEngine;

public class TutorialWood : MonoBehaviour, IRestart
{
    private bool woodDeleted = false;
    [SerializeField] private GameObject[] tutorialWoods;
    [SerializeField] private GameObject parent;

    public void AddRestartElement()
    {
        GameManager.GetManager().GetRestartManager().addRestartElement(this);
    }

    public void Restart()
    {
        woodDeleted = false;
        parent.SetActive(true);
        gameObject.SetActive(true);
        for (int i = 0; i < tutorialWoods.Length; i++)
        {
            tutorialWoods[i].SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Explosion"))
        {
            woodDeleted = true;
            Destroy(c.gameObject);
        }
    }
    private void Start()
    {
        AddRestartElement();
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
