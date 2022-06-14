using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMenuMax : MonoBehaviour
{
    private ExplainShow explain;
    private Animator unlockAnim;
    [Header("Lock")]
    [SerializeField] private bool locked = true;
    [SerializeField] private GameObject[] lockShow;
    [SerializeField] private GameObject[] lockHide;
    [SerializeField] private int index;

    [Header("MAX")]
    [SerializeField] private bool maximum = false;
    [SerializeField] private GameObject[] visualToHide;
    [SerializeField] private GameObject[] visualToShow;
    [SerializeField] private string bulletName;
    private void Start()
    {
        explain = GetComponentInChildren<ExplainShow>();
        unlockAnim = GetComponent<Animator>();
    }
    public void Click()
    {

        if (!locked && maximum && CurrentBulletsEqual())
        {
            explain.Hide();
            for (int i = 0; i < visualToHide.Length; i++)
            {
                visualToHide[i].SetActive(false);
            }
        }
    }
    private bool CurrentBulletsEqual()
    {
        for (int i = 0; i < 3; i++)
        {
            if (GameManager.GetManager().GetLevelData().LoadDataPlayerBullets()[i].ToString().Equals(bulletName)) { return true; }
        }
        return false;
    }
    public void CheckMax(int idx)
    {
        if (maximum && !locked && bulletName.Equals(GameManager.GetManager().GetLevelData().LoadDataPlayerBullets()[idx].ToString()))
        {
            for (int i = 0; i < visualToShow.Length; i++)
            {
                visualToShow[i].SetActive(true);
            }
        }
    }
    public void InitLock(int unlockIndex)
    {
        if (index < unlockIndex)
        {
            locked = false;
        }
        VisualLockSetter();
    }
    public void ChekLock(int unlockIndex)
    {
        if (unlockIndex == GameManager.GetManager().GetCurrentRoomIndex() && locked)
        {
            Unlock();
        }
        else if (unlockIndex < GameManager.GetManager().GetCurrentRoomIndex())
        {
            locked = false;
        }
        VisualLockSetter();
    }
    private void VisualLockSetter()
    {
        for (int i = 0; i < lockHide.Length; i++)
        {
            lockHide[i].SetActive(!locked);
        }
        for (int i = 0; i < lockShow.Length; i++)
        {
            lockShow[i].SetActive(locked);
        }
        Click();
    }
    private void Unlock()
    {
        locked = false;
        if (unlockAnim)
            unlockAnim.SetBool("Unlock", true);
    }
}
