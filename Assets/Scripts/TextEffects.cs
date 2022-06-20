using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.VFX;

public class TextEffects : MonoBehaviour
{
    public TMP_Text m_Text;
    public TMP_Text m_TextPercentatge;
    public bool m_Bubble=false;
    public float m_Multiplayer = 2.3f;
    public Animator m_Anim;
    public GameObject m_ShooterPoint;
    public GameObject m_Instance;
    public VisualEffect m_MuzzleFlashes;
    public GameObject m_Sound;
   

    public void StartNewScene()
    {
        m_Anim.SetTrigger("Shoot");
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        float t = 0f;
        yield return new WaitForSecondsRealtime(0.2f);
        m_Sound.SetActive(true);
        m_Instance.SetActive(true);
        m_Instance.transform.SetParent(null);
        m_MuzzleFlashes.gameObject.SetActive(true);
        m_MuzzleFlashes.Play();
        while (t <= 3f)
        {
            t += Time.deltaTime;
            m_Instance.transform.position += m_Anim.gameObject.transform.forward * 30f;
            yield return null;
        }
    }
    private void Awake()
    {
        GameManager.GetManager().GetSceneLoader().m_effects = this;
    }
    void Start()
    {
        m_TextPercentatge.text = "Loading progress: 0 %";
        //StartNewScene();
    }
    //void Update()
    //{
    //    m_TextPercentatge.ForceMeshUpdate();
    //    var textInfo = m_TextPercentatge.textInfo;
    //    for (int i = 0; i < textInfo.characterCount; ++i)
    //    {
    //        var charInfo = textInfo.characterInfo[i];

    //        if(!charInfo.isVisible)
    //        {
    //            continue;
    //        }

    //        var vertexs = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
    //        int idx = charInfo.vertexIndex;
    //        if (m_Bubble)
    //        {
    //            //vertexs[charInfo.vertexIndex + j] = ;
    //            Vector3 change = Wobble(Time.time + i);
    //            vertexs[idx] += change;
    //        }
    //        else
    //        {
    //            Vector3 offset = Move(Time.time + i);
    //            vertexs[idx] += offset;
    //            vertexs[idx + 1] += offset;
    //            vertexs[idx + 2] += offset;
    //            vertexs[idx + 3] += offset;
    //        }


            
    //    }

    //    for (int k = 0; k < textInfo.meshInfo.Length; ++k)
    //    {
    //        var meshInfo = textInfo.meshInfo[k];
    //        meshInfo.mesh.vertices = meshInfo.vertices;
    //        m_TextPercentatge.UpdateGeometry(meshInfo.mesh, k);
    //    }
    //}

    //Vector2 Move(float time)
    //{
    //    return new Vector2(Mathf.Sin(time * m_Multiplayer), Mathf.Cos(time * 0.9f));
    //}

    //Vector3 Wobble(float time)
    //{
    //    return new Vector3(0, Mathf.Sin(Time.time * 2f ) * 2f, 1);
    //}
}
