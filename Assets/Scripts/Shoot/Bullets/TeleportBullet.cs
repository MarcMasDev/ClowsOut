using System.Collections;
using UnityEngine;

public class TeleportBullet : Bullet
{
    private float m_RequiredDistance = 1f;
    GameObject m_PlayerMesh;
    GameObject m_TrailTeleport;
    public override void SetBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect)
    {
        base.SetBullet(position, normal, speed, damage, collisionMask, collisionWithEffect);
    }

    public override void SetTeleport(GameObject playerMesh, GameObject trailTeleport)
    {
        m_PlayerMesh = playerMesh;
        m_TrailTeleport = trailTeleport;
    }

    public override void OnCollisionWithEffect() { }

    public override void OnCollisionWithoutEffect()
    {
        StartCoroutine(TeleportColision());
    }

    IEnumerator TeleportColision()
    {
        Debug.Log("Teleporting");
        //temporal
        CharacterController l_CharacterController = GameObject.FindObjectOfType<Player_ShootSystem>().GetComponent<CharacterController>();
        Vector3 l_PlayerPos = l_CharacterController.transform.position;

        Vector3 l_Direction = (m_PointColision - l_PlayerPos).normalized;
        Vector3 l_SafeDistance = l_Direction * m_RequiredDistance;
        Vector3 l_SafePos = m_PointColision - l_SafeDistance;
       
        //Vector3 l_PlayerPos = l_CharacterController.transform.position;
        l_CharacterController.enabled = false;

        float l_Time = 0;
        float l_MaxTime = 1f;

        m_PlayerMesh.SetActive(false);
        m_TrailTeleport.SetActive(true);
        
        while (l_Time < l_MaxTime)
        {
            Debug.DrawLine(l_PlayerPos, l_SafePos);
            l_CharacterController.transform.position = Vector3.Lerp(l_PlayerPos, l_SafePos, l_Time / l_MaxTime);
            l_Time += Time.deltaTime;
            yield return null;
        }
        m_PlayerMesh.SetActive(true);
        m_TrailTeleport.SetActive(false);
        m_TrailTeleport.GetComponent<TrailRenderer>().Clear();
        l_CharacterController.enabled = true;
        Destroy(gameObject);
    }
}

