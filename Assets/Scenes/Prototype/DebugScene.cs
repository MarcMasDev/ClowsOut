using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
     //   GameManager.GetManager().GetLevelData().load();
        if(SceneManager.GetActiveScene().buildIndex==0)
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex -1);

    }
}
