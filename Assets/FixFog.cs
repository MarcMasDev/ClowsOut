using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixFog : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.02f);
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
