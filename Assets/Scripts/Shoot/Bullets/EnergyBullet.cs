using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class EnergyBullet : Bullet
{


    public override void SetBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect)
    {
        Debug.Log("Set Bullet");
        base.SetBullet(position, normal, speed, damage, collisionMask, collisionWithEffect);
    }

    public override void OnCollisionWithEffect()
    {
        //StartCoroutine(DamageArea());
    }

    public override void OnCollisionWithoutEffect()
    {
        //StartCoroutine(DamageArea());
    }










}
