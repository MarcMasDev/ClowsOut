using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.GetManager().GetLevelData().SaveData();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
