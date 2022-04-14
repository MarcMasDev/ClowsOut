using System.Collections;
using UnityEngine;

public class StickyBullet : Bullet
{
    private float m_TimeToExplosion;
    private float m_ExplosionArea;

    IEnumerator routine;
    public StickyBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect, float timeExplosion,float explosionArea) : base(position, normal, speed, damage, collisionMask, collisionWithEffect)
    {
        m_TimeToExplosion = timeExplosion;
        m_ExplosionArea = explosionArea;
    }

    public override void OnCollisionWithEffect()
    {
        routine = DamageArea();
        ControlCoroutines l_Control = GameObject.FindObjectOfType<ControlCoroutines>();
        l_Control.StartingCoroutine(routine);
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
    }
}
