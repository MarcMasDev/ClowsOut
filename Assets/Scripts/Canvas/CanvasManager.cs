using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    private bool m_Open;
    private bool m_Back;
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }
    private void Update()
    {
        if (m_Back)
        {
            GameCursor();
        }
        else if (m_Open)
        {

        }
    }
    private static void MenuCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private static void GameCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
