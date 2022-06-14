using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DI_System : MonoBehaviour
{
    [SerializeField] private DamageIndicator indicatorPrefab;
    [SerializeField] private RectTransform holder = null;

    private Dictionary<Transform, DamageIndicator> Indicators = new Dictionary<Transform, DamageIndicator>();

    public static Action<Transform> CreateIndicator = delegate { };
    [SerializeField] private float fadeSpeed = 2;
    [SerializeField] private float fadeOpacity = 0.5f;
    [SerializeField] private float maxWaitTime = 0.1f;
    [SerializeField] private CanvasGroup hitDamageCanvasGroup;

    [SerializeField] private float fadeLowHealth = 0.5f;
    [SerializeField] private float lowHealthImageStart1 = 0.5f;
    [SerializeField] private float lowHealthImageStart2 = 0.5f;
    [SerializeField] private CanvasGroup mainHudHealthCanvasGroup;
    [SerializeField] private CanvasGroup mainHudLowHealthCanvasGroup;

    private HealthSystem p_Health;
    private void Start()
    {
        p_Health = GameManager.GetManager().GetPlayer().GetComponent<HealthSystem>();
        p_Health.m_OnHit += HitIndicator;
    }
    private void OnEnable()
    {
        CreateIndicator += Create;
    }
    private void OnDisable()
    {
        CreateIndicator -= Create;
    }
    void Create(Transform target)
    {
        if (Indicators.ContainsKey(target))
        {
            print("TimerRestart of" + target.transform.name);
            Indicators[target].RestartTimer();
            return;
        }
        print("Timer Add of" + target.transform.name);
        DamageIndicator newIndicator = Instantiate(indicatorPrefab, holder);
        newIndicator.Init(target, GameManager.GetManager().GetPlayer().transform, new Action( () => { Indicators.Remove(target); }));

        Indicators.Add(target, newIndicator);

        //if (!p_Health)
        //{
        //    p_Health = GameManager.GetManager().GetPlayer().GetComponent<HealthSystem>();
        //}
        //if (p_Health.GetCurrentLife < p_Health.m_MaxLife * 0.25f)
        //{
        //    StartCoroutine(BloodMainHUD(mainHudLowHealthCanvasGroup, true));
        //}
        //else if (p_Health.GetCurrentLife < p_Health.m_MaxLife*0.5f)
        //{
        //    StartCoroutine(BloodMainHUD(mainHudHealthCanvasGroup));
        //}
    }
    private void HitIndicator(float f)
    {
        StopCoroutine(BloodFX());
        StartCoroutine(BloodFX());
    }
    private IEnumerator BloodFX()
    {
        while (hitDamageCanvasGroup.alpha < fadeOpacity)
        {
            hitDamageCanvasGroup.alpha += fadeSpeed*4 * Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(maxWaitTime);
        while (hitDamageCanvasGroup.alpha >= 0)
        {
            hitDamageCanvasGroup.alpha -= fadeSpeed / 2 * Time.deltaTime;
            yield return null;
        }
        hitDamageCanvasGroup.alpha = 0;
    }

    private IEnumerator BloodMainHUD(CanvasGroup c, bool midLvl = false)
    {
        if (midLvl)
        {
            while (hitDamageCanvasGroup.alpha >= 0)
            {
                hitDamageCanvasGroup.alpha -= fadeSpeed / 2 * Time.deltaTime;
                yield return null;
            }

        }
        while (c.alpha < fadeOpacity)
        {
           c.alpha += fadeSpeed  * Time.deltaTime;
            yield return null;
        }
    }
}
