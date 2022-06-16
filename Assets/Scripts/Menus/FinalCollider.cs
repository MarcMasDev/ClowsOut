using UnityEngine;

public class FinalCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print("Hola2");
        if (other.CompareTag("Player"))
        {
            print("Hola");
            GameManager.GetManager().GetCanvasManager().ShowWinMenu();
        
        }
    }
}
