using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMachine : MonoBehaviour, IInteractable
{
    public GameObject m_InteractFont;
    private void Start()
    {
        m_InteractFont.SetActive(false);
    }
    public virtual void Interact()
    {
        GameManager.GetManager().GetCanvasManager().ShowBulletMenu();
    }

    public virtual void StartPointing()
    {
        m_InteractFont.SetActive(true);
    }

    public virtual void StopPointing()
    {
        m_InteractFont.SetActive(false);
    }
}
