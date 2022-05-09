using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Player"))
            return;
                //   GameManager.GetManager().GetLevelData().load();
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex  +1);
        }
        else
        {
            GameManager.GetManager().GetLevelData().SaveTotalTime();
            SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);

        }
           

    }
}
