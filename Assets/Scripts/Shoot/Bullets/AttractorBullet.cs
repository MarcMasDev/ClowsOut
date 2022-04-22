using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttractorBullet : Bullet
{
    IEnumerator m_Routine;

    private float m_AttractorArea;
    private float m_AttractingTime;
    private float m_RequireAttractorDistance;
    ControlCoroutines m_Control;


    //public override Bullet InstantiateBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect)
    //{
    //    base.InstantiateBullet(position, normal, speed, damage, collisionMask, collisionWithEffect);

    //    //m_AttractorArea = attractorArea;
    //    //m_AttractingTime = attractingTime;
    //    //m_RequireAttractorDistance = attractingDistance;

    //}



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
        m_Routine = DamageArea();
        ControlCoroutines l_Control = GameObject.FindObjectOfType<ControlCoroutines>();
        l_Control.StartingCoroutine(m_Routine);
    }

    public override void OnCollisionWithoutEffect()
    {
        m_Routine = DamageArea();
        m_Control = GameObject.FindObjectOfType<ControlCoroutines>();
        m_Control.StartingCoroutine(m_Routine);
    }

    IEnumerator DamageArea()
    {
        Collider[] l_InArea = Physics.OverlapSphere(m_Pos, m_AttractorArea, m_CollisionWithEffect);
        List<Vector3> l_InitialPos = new List<Vector3>();
        List<Vector3> l_FinalPos = new List<Vector3>();

        for (int i = 0; i < l_InArea.Length; i++)
        {
            l_InArea[i].GetComponent<NavMeshAgent>().enabled = false;
            Vector3 l_Direction = (l_InArea[i].transform.position - m_Pos).normalized;
            Vector3 l_SafeDistance = l_Direction * m_RequireAttractorDistance;

            Vector3 l_Desplacement = m_Pos + l_SafeDistance;

            l_InitialPos.Add(l_InArea[i].transform.position);
            l_FinalPos.Add(l_Desplacement);
        }

        float l_Time = 0;
        while (l_Time <= m_AttractingTime)
        {
            for (int i = 0; i < l_InArea.Length; i++)
            {
                l_InArea[i].transform.position = Vector3.Slerp(l_InitialPos[i], l_FinalPos[i], l_Time / m_AttractingTime);
            }
            l_Time += Time.deltaTime;
            yield return null;
        }

        m_Control.StopingCoroutine(m_Routine);
    }

    //public AttractorBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect, float attractorArea, float attractingTime, float attractingDistance)
    //    : base(position, normal, speed, damage, collisionMask, collisionWithEffect)
    //{
    //    m_AttractorArea = attractorArea;
    //    m_AttractingTime = attractingTime;
    //    m_RequireAttractorDistance = attractingDistance;
    //}
}
