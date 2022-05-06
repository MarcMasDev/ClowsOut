using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttractorBullet : Bullet
{
    private float m_AttractorArea;
    private float m_AttractingTime;
    private float m_RequireAttractorDistance;
    List<GameObject> m_Enemies = new List<GameObject>();
    List<Vector3> m_InitialPos = new List<Vector3>();
    List<Vector3> m_FinalPos = new List<Vector3>();
    private SphereCollider m_Collider;

    public override void SetBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect)
    {
        Debug.Log("Set Bullet");
        base.SetBullet(position, normal, speed, damage, collisionMask, collisionWithEffect);
    }

    public override void SetAttractor(float attractorArea, float attractingTime, float attractingDistance)
    {
        m_Collider = GetComponent<SphereCollider>();
        m_Collider.enabled = false;
        m_AttractorArea = attractorArea;
        m_AttractingTime = attractingTime;
        m_RequireAttractorDistance = attractingDistance;
    }

    public override void OnCollisionWithEffect()
    {
        m_Collider.enabled = true;
    }

    public override void OnCollisionWithoutEffect()
    {
        m_Collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_CollisionWithEffect == (m_CollisionWithEffect | (1 << other.gameObject.layer)))
        {
            m_Enemies.Add(other.gameObject);
            
            other.gameObject.GetComponent<BlackboardEnemies>().ActivateAttractorEffect(
                transform.TransformPoint(m_Collider.center));
        }
          

        /*
        if (m_Enemies.Count == 0)
            return;
        */
        for (int i = 0; i < m_Enemies.Count; i++)
        {
            //Vector3 l_Direction = (m_PointColision - m_Enemies[i].transform.position).normalized;
            //m_Enemies[i].GetComponent<BlackboardEnemies>().ActivateAttractorEffect(m_Collider.center);
           // m_Enemies[i].GetComponent<BlackboardEnemies>().m_Pause = true;
           //m_Enemies[i].GetComponent<NavMeshAgent>().enabled = false;
           // m_Enemies[i].GetComponent<BlackboardEnemies>().m_Rigibody.isKinematic = false;
           //m_Enemies[i].GetComponent<BlackboardEnemies>().m_Rigibody.AddForce(l_Direction *2, ForceMode.Force);
           //m_Enemies[i].GetComponent<BlackboardEnemies>().m_Rigibody.velocity = l_Direction*2;

            //(m_Enemies[i].transform.position - m_Pos).normalized;
            //Vector3 l_SafeDistance = l_Direction * m_RequireAttractorDistance;
            //Vector3 l_Desplacement = m_PointColision - l_SafeDistance;

            //m_InitialPos.Add(m_Enemies[i].transform.position);
            //m_FinalPos.Add(l_Desplacement);ç

        }
        //StartCoroutine(DamageArea());
    }

    private void OnTriggerStay(Collider other)
    {
        for (int i = 0; i < m_Enemies.Count; i++)
        {
            

            if (Vector3.Distance(m_Enemies[i].transform.position, transform.position) < 1f)
            {
               // m_Enemies[i].GetComponent<BlackboardEnemies>().m_Pause = false;
            }
        }
    }

    //IEnumerator DamageArea()
    //{
    //    Collider[] l_InArea = Physics.OverlapSphere(transform.position, m_AttractorArea, m_CollisionWithEffect);
    //    List<Collider> l_Inside = l_InArea.ToList();
    //    for (int i = 0; i < l_InArea.Length; i++)
    //    {
    //        print(l_Inside.Count);
    //        if (l_Inside[i].GetComponent<CharacterController>())
    //            l_Inside.RemoveAt(i);
    //        print(l_Inside.Count);
    //        l_InArea[i].GetComponent<BlackboardEnemies>().m_Pause = true;
    //    }
    //    List<Vector3> l_InitialPos = new List<Vector3>();
    //    List<Vector3> l_FinalPos = new List<Vector3>();

    //    for (int i = 0; i < l_InArea.Length; i++)
    //    {
    //        l_InArea[i].GetComponent<NavMeshAgent>().enabled = false;
    //        Vector3 l_Direction = (l_InArea[i].transform.position - m_Pos).normalized;
    //        Vector3 l_SafeDistance = l_Direction * m_RequireAttractorDistance;

    //        Vector3 l_Desplacement = m_Pos; +l_SafeDistance;

    //        l_InitialPos.Add(l_InArea[i].transform.position);
    //        l_FinalPos.Add(l_Desplacement);
    //    }

    //    for (int i = 0; i < m_Enemies.Count; i++)
    //    {
    //        nos a punto = AB punto - nos
    //        Vector3 l_Direction = (m_PointColision - m_Enemies[i].transform.position).normalized;      //(l_InArea[i].transform.position - m_Pos).normalized;
    //        Rigidbody entity = m_Enemies[i].GetComponent<Rigidbody>();
    //        CapsuleCollider entityCol = m_Enemies[i].GetComponent<CapsuleCollider>();

    //        entityCol.enabled = true;
    //        entity.isKinematic = false;
    //        entity.velocity = l_Direction * 10;
    //    }

    //    float l_Time = 0;
    //    while (l_Time <= m_AttractingTime)
    //    {
    //        print("while");
    //        for (int i = 0; i < m_Enemies.Count; i++)
    //        {
    //            m_Enemies[i].transform.position = Vector3.Lerp(m_InitialPos[i], m_FinalPos[i], l_Time / m_AttractingTime);
    //        }
    //        l_Time += Time.deltaTime;
    //        yield return null;
    //    }
    //    yield return new WaitForSeconds(5);
    //    l_InArea[i].GetComponent<NavMeshAgent>().enabled = true;
    //    l_InArea[i].GetComponent<CharacterController>().enabled = true;
    //    Destroy(gameObject);
    //}
}
