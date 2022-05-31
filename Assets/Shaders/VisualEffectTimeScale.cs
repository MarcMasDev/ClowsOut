using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
[ExecuteInEditMode]
public class VisualEffectTimeScale : MonoBehaviour
{
    [Range(0.0f, 10.0f)] public float SimulationTimeScale = 1.0f;

    private VisualEffect Graph;

    private void OnValidate()
    {
        Graph = gameObject.GetComponent<VisualEffect>();
    }

    private void Update()
    {
        if (!Graph)
        {
            Graph = gameObject.GetComponent<VisualEffect>();
        }
        Graph.playRate = SimulationTimeScale;
    }
}
