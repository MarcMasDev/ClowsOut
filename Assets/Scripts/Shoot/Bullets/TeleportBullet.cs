using System.Collections;
using UnityEngine;

public class TeleportBullet : Bullet
{
    private float m_RequiredDistance = 0.5f;
    private IEnumerator m_Routine;
    ControlCoroutines m_Control;

    public override void SetBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect)
    {
        base.SetBullet(position, normal, speed, damage, collisionMask, collisionWithEffect);
    }

    public override void OnCollisionWithEffect() { }

    public override void OnCollisionWithoutEffect()
    {
        m_Routine = TeleportColision();
        //temporal
        m_Control = GameObject.FindObjectOfType<ControlCoroutines>();
        m_Control.StartingCoroutine(m_Routine);
    }

    IEnumerator TeleportColision()
    {
        Debug.Log("Teleporting");

        Vector3 l_Direction = (m_PointColision - m_Pos).normalized;
        Vector3 l_SafeDistance = l_Direction * m_RequiredDistance;

        Vector3 l_Desplacement = m_PointColision - l_SafeDistance;

        //temporal
        CharacterController l_CharacterController = GameObject.FindObjectOfType<CharacterController>();
        //
        Vector3 l_PlayerPos = l_CharacterController.transform.position;
        l_CharacterController.enabled = false;

        float l_Time = 0;
        float l_MaxTime = 1f;

        while (l_Time < l_MaxTime)
        {
            l_CharacterController.transform.position = Vector3.Lerp(l_PlayerPos, l_Desplacement, l_Time / l_MaxTime);
            l_Time += Time.deltaTime;

            yield return null;
        }
        l_CharacterController.enabled = true;
        m_Control.StopingCoroutine(m_Routine);
    }
}

