using UnityEngine.SceneManagement;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    public void  LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
