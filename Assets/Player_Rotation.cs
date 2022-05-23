using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Rotation : MonoBehaviour
{
    public Transform m_Player;
    public float m_RotateRightDegrees;

    public void RotateRight()
    {
        m_Player.forward = Quaternion.Euler(0, m_RotateRightDegrees, 0) * m_Player.transform.forward;
    }
}
