using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] private float time = 8;
    private float currentTime;

    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private RectTransform rect;

    private Transform target;
    private Transform player;

    private IEnumerator CountDown = null;
    private Action unRegister = null;

    private Quaternion tRot = Quaternion.identity;
    private Vector3 tpos = Vector3.zero;
    [SerializeField]private float fadeSpeed = 4;
    public void Init(Transform target, Transform player, Action unRegister)
    {
        print("INIT");
        Destroy(gameObject, 10);
        this.target = target;
        this.player = player;
        this.unRegister = unRegister;

        RestartTimer();

    }
    public void RestartTimer()
    {
        currentTime = time;
        StopCoroutine(Timer());
        StartCoroutine(Timer());
        StartCoroutine(RotateToTarget());
    }
    IEnumerator RotateToTarget()
    {
        while(enabled)
        {
            if (target)
            {
                tpos = target.position;
                tRot = target.rotation;
            }
            Vector3 dir = target.position-player.position;

            tRot = Quaternion.LookRotation(dir);
            tRot.z = -tRot.y;
            tRot.x = 0; tRot.y = 0;

            Vector3 upDir = new Vector3(0, 0, player.eulerAngles.y);
            rect.localRotation = tRot * Quaternion.Euler(upDir);
            yield return null;
        }
    }
    private IEnumerator Timer()
    {
        while(canvasGroup.alpha<1)
        {
            canvasGroup.alpha += fadeSpeed * Time.deltaTime;
            yield return null;
        }
        while(currentTime > 0)
        {
            currentTime--;
            yield return new WaitForSeconds(1);
        }
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= fadeSpeed/2 * Time.deltaTime;
            yield return null;
        }
        unRegister();
        Destroy(gameObject);
    }
}
