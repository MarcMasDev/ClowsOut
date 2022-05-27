using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SkinnedMeshToMeshVFX : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    public VisualEffect VFXGraph;
    public float refreshRate = 0.02f;
    private float currentRefreshRate = 0;
    private void Start()
    {
        currentRefreshRate = refreshRate;
    }
    private void Update()
    {
        currentRefreshRate -= Time.deltaTime;
        if (currentRefreshRate <0)
        {
            currentRefreshRate = refreshRate;
            Mesh m = new Mesh();
            skinnedMesh.BakeMesh(m);
            VFXGraph.SetMesh("Mesh", m);
        }
    }
}
