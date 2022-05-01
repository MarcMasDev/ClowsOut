using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneEvent : MonoBehaviour
{
    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
