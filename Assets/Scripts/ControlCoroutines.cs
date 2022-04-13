using System.Collections;
using UnityEngine;

public class ControlCoroutines : MonoBehaviour
{
    /// <summary>
    /// Useful for non-monobehaviour classes and it has IEnumerators.
    /// </summary>
    /// <param name="routine"></param>
    public void StartingCoroutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }
}
