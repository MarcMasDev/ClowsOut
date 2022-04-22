using System.Collections;
using UnityEngine;

public class StickyBullet : Bullet
{
    private float m_TimeToExplosion;
    private float m_ExplosionArea;
    ControlCoroutines m_Control;
    IEnumerator m_Routine;
    //public StickyBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect, float timeExplosion,float explosionArea) : base(position, normal, speed, damage, collisionMask, collisionWithEffect)
    //{
    //    m_TimeToExplosion = timeExplosion;
    //    m_ExplosionArea = explosionArea;
    //}

    public override void SetBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect)
    {
        base.SetBullet(position, normal, speed, damage, collisionMask, collisionWithEffect);
    }

    public override void SetSticky(float timeExplosion, float explosionArea)
    {
        Debug.Log("Set Sticky Bullet");
        base.SetSticky(timeExplosion, explosionArea);
        m_TimeToExplosion = timeExplosion;
        m_ExplosionArea = explosionArea;
    }


    public override void OnCollisionWithEffect()
    {
        m_Routine = DamageArea();
        m_Control = GameObject.FindObjectOfType<ControlCoroutines>();
        m_Control.StartingCoroutine(m_Routine);
        Debug.Log("WITH Sticky Effect");
    }

    public override void OnCollisionWithoutEffect()
    {
        Debug.Log("WITHOUT Sticky Effect");
    }

    IEnumerator DamageArea()
    {
        float l_Time = 0;
        while (l_Time <= m_TimeToExplosion)
        {
            l_Time += Time.deltaTime;
            yield return null;
        }
        //something to mark the enemy with sticky bomb.
        //--
        //looking nearly
        m_Pos = m_CollidedObject.transform.position;
        Collider[] l_InArea = Physics.OverlapSphere(m_Pos, m_ExplosionArea, m_CollisionWithEffect);
        for (int i = 0; i < l_InArea.Length; i++)
        {
            Debug.Log(l_InArea[i]);
            l_InArea[i].GetComponent<HealthSystem>().TakeDamage(m_DamageBullet);
        }

        m_Control.StopingCoroutine(m_Routine);
    }
}
