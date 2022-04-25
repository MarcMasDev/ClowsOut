using UnityEngine;

public class NormalBullet : Bullet
{
    HealthSystem m_EnemyHealthSystem;
    //public NormalBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect) : base(position, normal, speed, damage, collisionMask, collisionWithEffect)
    //{//nothing to do yet. Takes values of constructor parent (base)
    //}

    //public override Bullet InstantiateBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect)
    //{
    //    return null// base.InstantiateBullet(position, normal, speed, damage, collisionMask, collisionWithEffect);
    //}

    public override void SetBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect)
    {
        base.SetBullet(position, normal, speed, damage, collisionMask, collisionWithEffect);
    }

    public override void OnCollisionWithEffect()
    {
        m_EnemyHealthSystem = m_CollidedObject.GetComponent<HealthSystem>();
        m_EnemyHealthSystem.TakeDamage(m_DamageBullet);
    }


    public override void OnCollisionWithoutEffect()
    {
    }


}
