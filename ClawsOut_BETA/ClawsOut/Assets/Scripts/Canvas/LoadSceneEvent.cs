using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneEvent : MonoBehaviour
{
    void RestartLevel()
    {
        //GameManager.GetManager().GetRestartManager().Restart();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
