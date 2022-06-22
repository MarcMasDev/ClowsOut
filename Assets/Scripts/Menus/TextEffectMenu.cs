using TMPro;
using UnityEngine;

public class TextEffectMenu : MonoBehaviour
{
    public TMP_Text text;
    public Color Pointer;
    public Color Default = Color.white;
    private bool Poiting;
    public bool m_Bubble = false;
    public float m_Multiplayer = 1.7f;
    public void PointerColor()
    {
        text.color = Pointer;
        Poiting = true;
    }
    public void DefaultColor()
    {
        text.color = Default;
        Poiting = false;
    }

    void Update()
    {
        if (!Poiting)
            return;
     
        text.ForceMeshUpdate();
        var textInfo = text.textInfo;
        for (int i = 0; i < textInfo.characterCount; ++i)
        {
            var charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible)
            {
                continue;
            }

            var vertexs = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            int idx = charInfo.vertexIndex;
            if (m_Bubble)
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
        return new Vector2(Mathf.Sin(time * m_Multiplayer), Mathf.Cos(time * 0.9f));
    }

    Vector3 Wobble(float time)
    {
        return new Vector3(0, Mathf.Sin(Time.time * 2f) * 2f, 1);
    }

}
