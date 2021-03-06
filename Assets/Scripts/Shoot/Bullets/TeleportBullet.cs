using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBullet : Bullet
{
    private float m_RequiredDistance = 1f;
    GameObject[] m_PlayerMesh;
    GameObject m_TrailTeleport;
    PlayParticle m_ParticleGameobject;
    float m_VelocityPlayer;
    private Vector3 normal_I;

    [SerializeField] private Tp_PlayFXOnSpawn explosion;
    [SerializeField] private float explYoffset = 0.5f;

    [SerializeField]
    SphereCollider m_Collider;
    [SerializeField]
    TeleportDamage m_teleportDamage;

    public override void SetBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect, Transform enemy)
    {
        base.SetBullet(position, normal, speed, damage, collisionMask, collisionWithEffect, enemy);
        normal_I = normal;
    }

    public override void SetTeleport(GameObject[] playerMesh, GameObject trailTeleport, float velocityPlayer, PlayParticle particle, float requireDistance)
    {
        m_PlayerMesh = playerMesh;
        m_TrailTeleport = trailTeleport;
        m_VelocityPlayer = velocityPlayer;
        m_ParticleGameobject = particle;
        m_RequiredDistance = requireDistance;
    }

    public override void OnCollisionWithEffect()
    {
        StartCoroutine(TeleportColision());
    }

    public override void OnCollisionWithoutEffect()
    {
        StartCoroutine(TeleportColision());
    }

    IEnumerator TeleportColision()
    {
        Debug.Log("Teleporting");
        //temporal
        GameManager.GetManager().GetPlayer().GetComponent<Player_Blackboard>().m_Teleported = false;
        CharacterController l_CharacterController = GameObject.FindObjectOfType<Player_ShootSystem>().GetComponent<CharacterController>();

        Vector3 l_PlayerPos = l_CharacterController.transform.position;

        RaycastHit l_Hit;
        if (Physics.Raycast(m_PointColision, Vector2.up, out l_Hit, 2, m_CollisionMask))
        {
            if (Physics.Raycast(m_PointColision, Vector2.down, out l_Hit, 2, m_CollisionMask))
            {
                m_PointColision += l_Hit.distance * Vector3.down;
                //Debug.Log("TP_2_hit");
            }
            else
            {
                m_PointColision += 2 * Vector3.down;
                //Debug.Log("TP_2");
            }
        }
        else
        {
            if (Physics.Raycast(m_PointColision, Vector2.down, out l_Hit, 1, m_CollisionMask))
            {
                m_PointColision += l_Hit.distance * Vector3.down;
                //Debug.Log("TP_1_hit");
            }
            else
            {
                m_PointColision += 0.875f * Vector3.down;
                //Debug.Log("TP_1");
            }
        }
        if (Physics.Raycast(m_PointColision, m_Normal, out l_Hit, 0.25f, m_CollisionMask))
        {
            m_PointColision -= 0.25f * m_Normal;
            //Debug.Log("TP_Normal");
        }
            //if (Physics.Raycast(m_PointColision + m_RaycastHit.normal * 0.1f, -m_RaycastHit.normal, out l_Hit, 0.5f, m_CollisionMask))
            //{
            //    m_PointColision += 0.25f * m_RaycastHit.normal;
            //}

        float l_MaxTime = Vector3.Distance(m_PointColision, l_PlayerPos) / m_VelocityPlayer;
        l_CharacterController.enabled = false;

        m_ParticleGameobject.gameObject.SetActive(true);
        m_ParticleGameobject.transform.forward = normal_I;
        m_ParticleGameobject.PlayParticles();

        foreach (GameObject go in m_PlayerMesh)
        {
            go.SetActive(false);
        }
        m_TrailTeleport.SetActive(true);
        float l_Time = 0;
        while (l_Time < l_MaxTime)
        {
            //Debug.DrawLine(l_PlayerPos, l_SafePos);
            l_CharacterController.transform.position = Vector3.Lerp(l_PlayerPos, m_PointColision, l_Time / l_MaxTime);
            l_Time += Time.deltaTime;
            Debug.DrawRay(m_PointColision, Vector3.up, Color.red);
            Debug.DrawRay(m_PointColision, Vector3.down, Color.red);
            Debug.DrawRay(m_PointColision, Vector3.right, Color.green);
            Debug.DrawRay(m_PointColision, Vector3.left, Color.green);
            yield return null;
        }
        //Debug.Break();
        explosion.PlayAnim();
        explosion.transform.SetParent(null);
        explosion.transform.position = GameManager.GetManager().GetPlayer().transform.position;
        m_teleportDamage.m_DamageBullet = m_DamageBullet;
        m_Collider.enabled = true;
        foreach (GameObject go in m_PlayerMesh)
        {
            go.SetActive(true);
        }
        m_TrailTeleport.SetActive(false);

        //m_TrailTeleport.GetComponent<TrailRenderer>().Clear();
        l_CharacterController.enabled = true;
        m_ParticleGameobject.gameObject.SetActive(false);
        GameManager.GetManager().GetPlayer().GetComponent<Player_Blackboard>().m_Teleported = true;
        Destroy(gameObject);
    }
}

