using Cinemachine;
using UnityEngine;

public class BulletMachine : MonoBehaviour, IInteractable
{
    public GameObject m_InteractFont;
    public CinemachineVirtualCamera m_Camera;
    public CanvasGroup m_BulletMenuCanvasGroup;
    private BulletMenu m_BulletMenu;
    private void Awake()
    {
        m_BulletMenu = m_BulletMenuCanvasGroup.GetComponent<BulletMenu>();
    }
    private void Start()
    {
        m_InteractFont.SetActive(false);
    }
    public virtual void Interact()
    {
        GameManager.GetManager().GetCanvasManager().SetBulleMenutCanvasGroup(m_BulletMenuCanvasGroup, m_BulletMenu);
        GameManager.GetManager().GetCameraManager().SetBulletMachineCamera(m_Camera);
        GameManager.GetManager().GetCanvasManager().ShowBulletMenu();
        m_BulletMenu.CheckUnlock();
      //  m_BulletMenu.SetUnlocked();
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
