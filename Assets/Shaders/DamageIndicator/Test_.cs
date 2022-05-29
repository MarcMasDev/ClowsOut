using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_ : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(waitToStart());
        TestTransform(transform);
    }
    private void TestTransform(Transform t = null)
    {
        print(t);
    }
    private IEnumerator waitToStart()
    {
        print("WTF");
        yield return new WaitForSeconds(2);
        Init();
    }
    void Init()
    {
        print("DI_SystSet");
        DI_System.CreateIndicator(this.transform);
        StartCoroutine(st());
    }
    private IEnumerator st()
    {
        yield return new WaitForSeconds(3);
        DI_System.CreateIndicator(this.transform);
        StartCoroutine(st());
    }
}
