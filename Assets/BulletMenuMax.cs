using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMenuMax : MonoBehaviour
{
    [SerializeField] private GameObject[] visualToHide;
    [SerializeField] private GameObject[] visualToShow;
    private ExplainShow explain;
    [SerializeField] private int max = 1;
    [SerializeField] private string bulletName;
    private float currentAmount = 0;
    private bool isMax = false;
    private void Start()
    {
        explain = GetComponentInChildren<ExplainShow>();
    }
    public void Click()
    {
        if (isMax)
        {
            currentAmount++;
            if (currentAmount == max)
            {
                print("??");
                explain.Hide();
                for (int i = 0; i < visualToHide.Length; i++)
                {
                    visualToHide[i].SetActive(false);
                }
            }
            else if (currentAmount > max)
                currentAmount = max;
        }

    }
    public void CheckMax(int idx)
    {
        if (bulletName.Equals(GameManager.GetManager().GetLevelData().LoadDataPlayerBullets()[idx].ToString()))
        {
            for (int i = 0; i < visualToShow.Length; i++)
            {
                visualToShow[i].SetActive(true);
            }
            currentAmount--;
        }
        if (currentAmount <0)
        {
            currentAmount = 0;
        }
    }
    public void Max(bool isMax)
    {
        this.isMax = isMax;
    }
}
