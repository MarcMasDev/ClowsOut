using Cinemachine;
using System.Collections;
using UnityEngine;

public class BulletMachine : MonoBehaviour, IInteractable
{
    public GameObject m_InteractFont;
    public CinemachineVirtualCamera m_Camera;
    public CanvasGroup m_BulletMenuCanvasGroup;
    public BulletMenu m_BulletMenu;

    private void Start()
    {
        m_InteractFont.SetActive(false);
    }
    public virtual void Interact()
    {
        //GameManager.GetManager().GetCanvasManager().SetBulleMenutCanvasGroup(m_BulletMenuCanvasGroup, m_BulletMenu);
        //GameManager.GetManager().GetCameraManager().SetBulletMachineCamera(m_Camera);
        //GameManager.GetManager().GetCanvasManager().ShowBulletMenu();
        //m_BulletMenu.CheckUnlock();
        StartCoroutine(Delay());
      //  m_BulletMenu.SetUnlocked();
    }
    IEnumerator Delay()
    {
        GameManager.GetManager().GetCanvasManager().SetBulleMenutCanvasGroup(m_BulletMenuCanvasGroup, m_BulletMenu);
        GameManager.GetManager().GetCameraManager().SetBulletMachineCamera(m_Camera);
        yield return null;
        GameManager.GetManager().GetCanvasManager().ShowBulletMenu();
        m_BulletMenu.CheckUnlock();
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
