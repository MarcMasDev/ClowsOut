using System.Collections;
using UnityEngine;

public class TeleportBullet : Bullet
{
    private float m_RequiredDistance=0.5f;
    private IEnumerator routine;

    public TeleportBullet(Vector3 position, Vector3 normal, float speed, float damage, LayerMask collisionMask, LayerMask collisionWithEffect) : base(position, normal, speed, damage, collisionMask, collisionWithEffect)
    {
        //nothing to do yet. Takes values of constructor parent (base)
    }

    public override void OnCollisionWithEffect()
    { }

    public override void OnCollisionWithoutEffect()
    {
        routine = TeleportColision();
        //temporal
        ControlCoroutines control = GameObject.FindObjectOfType<ControlCoroutines>();
        control.StartingCoroutine(routine);
    }
  
    IEnumerator TeleportColision()
    {
        Debug.Log("Teleporting");

        Vector3 l_Direction = (m_PointColision-m_Pos).normalized;
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
    }

    private void TeleportWithEnemy()
    { 
    
    }
}

