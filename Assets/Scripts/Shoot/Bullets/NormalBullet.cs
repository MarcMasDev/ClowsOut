using UnityEngine;

public class NormalBullet : Bullet
{
    HealthSystem m_EnemyHealthSystem;
    public NormalBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect) : base(position, normal, speed, damage, collisionMask, collisionWithEffect)
    {
    }

    public override void OnCollisionWithEffect()
    {
        m_EnemyHealthSystem = m_CollidedObject.GetComponent<HealthSystem>();
        m_EnemyHealthSystem.TakeDamage(m_DamageBullet);

        Debug.Log("Restado da�o a "+m_EnemyHealthSystem);
    }


    public override void OnCollisionWithoutEffect()
    {
        Debug.Log("Colision sin quitar da�o");
    
    }


}
