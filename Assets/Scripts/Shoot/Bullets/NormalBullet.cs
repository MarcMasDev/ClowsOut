using UnityEngine;

public class NormalBullet : Bullet
{
    HealthSystem m_HealthSystem;

    public override void SetBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect)
    {
        Debug.Log("Normal Bullet");
        base.SetBullet(position, normal, speed, damage, collisionMask, collisionWithEffect);
    }

    public override void OnCollisionWithEffect()
    {
        m_HealthSystem = m_CollidedObject.GetComponent<HealthSystem>();
        m_HealthSystem.TakeDamage(m_DamageBullet);

        Debug.Log("Restado da�o a "+m_HealthSystem);
    }

    public override void OnCollisionWithoutEffect()
    {
        Debug.Log("Colision sin quitar da�o");
    }
}
