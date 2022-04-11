using UnityEngine;

public class Player_Blackboard : MonoBehaviour
{
    [Header("Movement")]
    public Camera m_Camera;
    public float m_LerpRotationPct = 0.1f;
    public float m_WalkVelocity = 3;
    public float m_RunVelocity;
}
