using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMenuMax : MonoBehaviour
{
    private ExplainShow explain;

    [Header("Lock")]
    [SerializeField] private bool locked = true;
    [SerializeField] private GameObject[] lockShow;
    [SerializeField] private GameObject[] lockHide;

    [Header("MAX")]
    [SerializeField] private bool maximum = false;
    [SerializeField] private GameObject[] visualToHide;
    [SerializeField] private GameObject[] visualToShow;
    [SerializeField] private string bulletName;
    private void Start()
    {
        explain = GetComponentInChildren<ExplainShow>();
    }
    public void Click()
    {
        if (!locked && maximum)
        {
            explain.Hide();
            for (int i = 0; i < visualToHide.Length; i++)
            {
                visualToHide[i].SetActive(false);
            }
        }
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

    public void Unlock()
    {
        locked = true;
    }

    public void ChekLock()
    {
        for (int i = 0; i < visualToHide.Length; i++)
        {
            lockHide[i].SetActive(!locked);
            lockShow[i].SetActive(locked);
        }
    }
}
