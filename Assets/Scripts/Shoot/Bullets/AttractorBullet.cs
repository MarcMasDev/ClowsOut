using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttractorBullet : Bullet
{
    private float m_AttractorArea;
    private float m_AttractingTime;
    private float m_RequireAttractorDistance;

    public override void SetBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect)
    {
        Debug.Log("Set Bullet");
        base.SetBullet(position, normal, speed, damage, collisionMask, collisionWithEffect);
    }

    public override void SetAttractor(float attractorArea, float attractingTime, float attractingDistance)
    {
        Debug.Log("Set Attractor Bullet");
        m_AttractorArea = attractorArea;
        m_AttractingTime = attractingTime;
        m_RequireAttractorDistance = attractingDistance;
    }

    public override void OnCollisionWithEffect()
    {
        StartCoroutine(DamageArea());
    }

    public override void OnCollisionWithoutEffect()
    {
        StartCoroutine(DamageArea());
    }

    IEnumerator DamageArea()
    {
        Collider[] l_InArea = Physics.OverlapSphere(transform.position, m_AttractorArea, m_CollisionWithEffect);
        for (int i = 0; i < l_InArea.Length; i++)
        {
            print(l_InArea[i].name);
        }
        //List<Vector3> l_InitialPos = new List<Vector3>();
        //List<Vector3> l_FinalPos = new List<Vector3>();

        ////for (int i = 0; i < l_InArea.Length; i++)
        ////{
        ////    l_InArea[i].GetComponent<NavMeshAgent>().enabled = false;
        ////    Vector3 l_Direction = (l_InArea[i].transform.position - m_Pos).normalized;
        ////    Vector3 l_SafeDistance = l_Direction * m_RequireAttractorDistance;

        ////    Vector3 l_Desplacement = m_Pos; +l_SafeDistance;

        ////    l_InitialPos.Add(l_InArea[i].transform.position);
        ////    l_FinalPos.Add(l_Desplacement);
        ////}

        for (int i = 0; i < l_InArea.Length; i++)
        {

            Vector3 l_Direction = (l_InArea[i].transform.position - m_Pos).normalized;
            Rigidbody entity = l_InArea[i].GetComponent<Rigidbody>();
            CapsuleCollider entityCol = l_InArea[i].GetComponent<CapsuleCollider>();
            //l_InArea[i].GetComponent<NavMeshAgent>().enabled = false;
            l_InArea[i].GetComponent<CharacterController>().enabled = false;
            entityCol.enabled = true;
            entity.isKinematic = false;
            entity.velocity=l_Direction*10;
        }

        //float l_Time = 0;
        //while (l_Time <= m_AttractingTime)
        //{
        //    for (int i = 0; i < l_InArea.Length; i++)
        //    {
        //        //l_InArea[i].transform.position = Vector3.Slerp(l_InitialPos[i], l_FinalPos[i], l_Time / m_AttractingTime);

        //    }
        //    l_Time += Time.deltaTime;
        //    yield return null;
        //}
        yield return new WaitForSeconds(5);
        //l_InArea[i].GetComponent<NavMeshAgent>().enabled = true;
        //l_InArea[i].GetComponent<CharacterController>().enabled = true;
        Destroy(gameObject);
    }
}
