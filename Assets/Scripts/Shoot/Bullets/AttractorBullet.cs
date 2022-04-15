using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractorBullet : Bullet
{
    IEnumerator routine;

    private float m_AttractorArea;
    private float m_AttractingTime;
    private float m_RequireAttractorDistance;

    public AttractorBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect, float attractorArea, float attractingTime, float attractingDistance)
        : base(position, normal, speed, damage, collisionMask, collisionWithEffect)
    {
        m_AttractorArea = attractorArea;
        m_AttractingTime = attractingTime;
        m_RequireAttractorDistance = attractingDistance;
    }

    public override void OnCollisionWithEffect()
    {
        routine = DamageArea();
        ControlCoroutines l_Control = GameObject.FindObjectOfType<ControlCoroutines>();
        l_Control.StartingCoroutine(routine);
    }

    public override void OnCollisionWithoutEffect()
    {
        routine = DamageArea();
        ControlCoroutines l_Control = GameObject.FindObjectOfType<ControlCoroutines>();
        l_Control.StartingCoroutine(routine);
    }

    IEnumerator DamageArea()
    {
        Collider[] l_InArea = Physics.OverlapSphere(m_Pos, m_AttractorArea, m_CollisionWithEffect);
        List<Vector3> l_InitialPos = new List<Vector3>();
        List<Vector3> l_FinalPos = new List<Vector3>();

        for (int i = 0; i < l_InArea.Length; i++)
        {
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
    }
}
