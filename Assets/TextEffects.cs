using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextEffects : MonoBehaviour
{
    public TMP_Text text;
    public bool buble=false;
    public float multiplayer = 2.3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.ForceMeshUpdate();
        var textInfo = text.textInfo;
        for (int i = 0; i < textInfo.characterCount; ++i)
        {
            var charInfo = textInfo.characterInfo[i];

            if(!charInfo.isVisible)
            {
                continue;
            }

            var vertexs = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            int idx = charInfo.vertexIndex;
            if (buble)
            {
                //vertexs[charInfo.vertexIndex + j] = ;
                Vector3 change = Wobble(Time.time + i);
                vertexs[idx] += change;
            }
            else
            {
                Vector3 offset = Move(Time.time + i);
                vertexs[idx] += offset;
                vertexs[idx + 1] += offset;
                vertexs[idx + 2] += offset;
                vertexs[idx + 3] += offset;
            }


            
        }

        for (int k = 0; k < textInfo.meshInfo.Length; ++k)
        {
            var meshInfo = textInfo.meshInfo[k];
            meshInfo.mesh.vertices = meshInfo.vertices;
            text.UpdateGeometry(meshInfo.mesh, k);
        }
    }

    Vector2 Move(float time)
    {
        return new Vector2(Mathf.Sin(time * multiplayer), Mathf.Cos(time * 0.9f));
    }

    Vector3 Wobble(float time)
    {
        return new Vector3(0, Mathf.Sin(Time.time * 2f ) * 2f, 1);
    }
}
