using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //UwU
    protected Vector3 m_PointColision;
    protected Vector3 m_Pos;
    protected float m_Speed;
    
    protected GameObject m_CollidedObject;
    protected RaycastHit m_RaycastHit;
    protected float m_DamageBullet;

    private Vector3 m_NextFramePos;
    protected Vector3 m_Normal;

    protected LayerMask m_CollisionMask;
    protected LayerMask m_CollisionWithEffect;

    [SerializeField] private PlayParticle impactFX;
    [SerializeField] private GameObject projectileVFX;
    [SerializeField] private GameObject bloodFX;

    private Transform shootingEntity;
    public virtual void SetBullet(Vector3 position, Vector3 normal, float speed,
        float damage, LayerMask collisionMask, LayerMask collisionWithEffect, Transform enemy_transform = null)
    {
        shootingEntity = enemy_transform;
        transform.position = position;
        m_Speed = speed;
        m_CollisionMask = collisionMask;
        m_CollisionWithEffect = collisionWithEffect;
        m_Normal = normal;
        m_DamageBullet = damage;
        transform.forward = normal;
    }
    private void OnEnable()
    {
        OnClickBulletMachine.DestroyBullets += DestroyBullet;
        GameManager.GetManager().GetShootSystemManager().OnDestroyAllBullets += DestroyBullet;
    }
    private void OnDisable()
    {
        OnClickBulletMachine.DestroyBullets -= DestroyBullet;
        GameManager.GetManager().GetShootSystemManager().OnDestroyAllBullets -= DestroyBullet;
    }
    public virtual void SetAttractor(float attractorArea, float attractingTime, float attractingDistance,GameObject Particles) {}
    public virtual void SetIce(int maxIterations, float timeIteration, float slowSpeed) { }
    public virtual void SetSticky(float timeExplosion) { }
    public virtual void SetTeleport(GameObject[] playerMesh, GameObject trailTeleport,float velocityPlayer,PlayParticle particles, float requireDistance) { }
    public virtual void SetEnegy(List<EnergyBullet> eBullets) { }

    public void Hit()
    {
        RaycastHit l_RayCastHit;
        float l_Time = Time.deltaTime;
        m_NextFramePos = transform.position + m_Normal * l_Time * m_Speed;
        //Debug.DrawLine(transform.position, m_NextFramePos * 10);
        if (Physics.Raycast(transform.position, m_Normal, out l_RayCastHit, Vector3.Distance(transform.position, m_NextFramePos), m_CollisionMask))
        {
            
            if (m_CollisionWithEffect == (m_CollisionWithEffect | (1 << l_RayCastHit.collider.gameObject.layer)))
            {
             
                m_PointColision = l_RayCastHit.point - m_Normal * 0.005f;
                m_CollidedObject = l_RayCastHit.collider.gameObject;
                m_RaycastHit = l_RayCastHit;
                transform.position = m_PointColision;
                if (l_RayCastHit.collider.gameObject == GameManager.GetManager().GetPlayer() && shootingEntity)
                {
                    SetHudIndicator();
                }
                if (l_RayCastHit.collider.CompareTag("Drone"))
                {

                }
                else
                {
                    Instantiate(bloodFX, m_PointColision, Quaternion.identity);
                }
                OnCollisionWithEffect();
            }
            else
            {
                m_PointColision = l_RayCastHit.point - m_Normal * 0.005f;
                m_CollidedObject = l_RayCastHit.collider.gameObject;
                m_RaycastHit = l_RayCastHit;
                transform.position = m_PointColision;
                OnCollisionWithoutEffect();
            }
            transform.position = m_PointColision;
            m_Speed = 0;
            m_NextFramePos = m_PointColision;

            impactFX.PlayParticles();
            projectileVFX.SetActive(false);
        }
    }

    public void Update()
    {
        Hit();
        transform.position = m_NextFramePos;
    }

    //TODO: Override effects in each child of bullet
    public virtual void OnCollisionWithEffect()
    {
        Debug.Log("Impact WITH Effect");
    }

    public virtual void OnCollisionWithoutEffect()
    {
        Debug.Log("Impact WITHOUT Effect");
    }
    private void SetHudIndicator()
    {
        DI_System.CreateIndicator(shootingEntity);
    }
    public void DestroyBullet()
    {
        if(gameObject)
            Destroy(gameObject);
    }
   
}
