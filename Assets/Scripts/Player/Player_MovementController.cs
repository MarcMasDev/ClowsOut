using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player_MovementController : MonoBehaviour
{
    private Player_Blackboard m_Blackboard;
    private Vector3 m_Direction;
    private Vector3 m_DashDirection;
    private float m_VerticalVelocity;
    private Vector3 m_Redirection;
    private float m_InitialCenterY;
    private float m_AddedOffsetCenterY = 0.045f;

    [HideInInspector] public bool m_OnGround;

    private CharacterController m_CharacterController;

    private void Awake()
    {
        m_Blackboard = GetComponent<Player_Blackboard>();
        m_CharacterController = GetComponent<CharacterController>();
    }
    private void Start()
    {
        m_InitialCenterY = m_CharacterController.center.y;
        Vector3 l_forward = GameManager.GetManager().GetCameraManager().m_Camera.transform.forward;
        l_forward.y = 0;
        transform.forward = l_forward;
    }
    public void MovementUpdate(Vector2 inputAxis, Camera camera)
    {
        if (inputAxis != Vector2.zero)
        {
            Vector3 forward = camera.transform.forward;
            Vector3 right = camera.transform.right;
            forward.y = 0.0f;
            right.y = 0.0f;
            forward.Normalize();
            right.Normalize();

            Vector2 movementAxis = inputAxis;
            m_Direction += forward * movementAxis.y;
            m_Direction += right * movementAxis.x;
            m_Direction.Normalize();

            if (OnSlope())
            {
                m_CharacterController.center = new Vector3(m_CharacterController.center.x, 
                    m_InitialCenterY + m_AddedOffsetCenterY, m_CharacterController.center.z);
                m_VerticalVelocity += m_Blackboard.m_SlopeForce;
            }
            else{
                m_CharacterController.center = new Vector3(m_CharacterController.center.x,
                    m_InitialCenterY, m_CharacterController.center.z);
            }
        }
    }
    public void JumpMovementUpdate(Vector2 inputAxis, Camera camera)
    {
        if (inputAxis != Vector2.zero)
        {
            Vector3 forward = camera.transform.forward;
            Vector3 right = camera.transform.right;
            forward.y = 0.0f;
            right.y = 0.0f;
            forward.Normalize();
            right.Normalize();

            Vector2 movementAxis = inputAxis;
            m_Redirection += forward * movementAxis.y;
            m_Redirection += right * movementAxis.x;
            m_Redirection.Normalize();
        }
    }
    public void SetDashDirection(Camera camera,Vector2 dashDirection)
    {
        Vector3 forward = camera.transform.forward;
        Vector3 right = camera.transform.right;
        forward.y = 0.0f;
        right.y = 0.0f;
        forward.Normalize();
        right.Normalize();
        m_DashDirection += forward * dashDirection.y;
        m_DashDirection += right * dashDirection.x;
        m_DashDirection.Normalize();
    }
    public void SetDashDirection(Camera camera)
    {
        Vector3 forward = camera.transform.forward;
        m_DashDirection = forward;
    }
    public void ResetDashDirection()
    {
        m_DashDirection = Vector3.zero;
    }
    public void GravityUpdate()
    {
        m_OnGround = false;
        CollisionFlags collisionFlags = m_CharacterController.collisionFlags;
        if ((collisionFlags & CollisionFlags.Below) != 0 && m_VerticalVelocity < 0.0f)
        {
            m_VerticalVelocity = 0;
            m_OnGround = true;
        }
        else if ((collisionFlags & CollisionFlags.Above) != 0 && m_VerticalVelocity > 0.0f)
        {
            m_VerticalVelocity = 0;
        }
        m_VerticalVelocity += Physics.gravity.y * Time.deltaTime;
        m_Direction = new Vector3(m_Direction.x, m_VerticalVelocity * Time.deltaTime, m_Direction.z);
    }
    public void SetMovement(float velocity)
    {
        Vector3 movement = new Vector3((m_Direction.x * velocity + m_DashDirection.x * velocity + m_Redirection.x * m_Blackboard.m_AirSpeed) * Time.deltaTime,
            m_Direction.y, (m_Direction.z * velocity + m_DashDirection.z * velocity + m_Redirection.z * m_Blackboard.m_AirSpeed) * Time.deltaTime);
        m_CharacterController.Move(movement);
        m_Redirection = Vector3.zero;
        m_Direction = Vector2.zero;
    }
    public bool OnGround()
    {
        return Physics.Raycast(m_Blackboard.m_Center.transform.position, m_Blackboard.m_Feet.transform.position - m_Blackboard.m_Center.transform.position,
            Vector3.Distance(m_Blackboard.m_Center.transform.position, m_Blackboard.m_Feet.transform.position), m_Blackboard.m_GroundLayerMask);
    }
    public bool OnWall()
    {
        return Physics.Raycast(m_Blackboard.m_CenterW.transform.position, m_Blackboard.m_Hand.transform.position - m_Blackboard.m_CenterW.transform.position,
            Vector3.Distance(m_Blackboard.m_CenterW.transform.position, m_Blackboard.m_Hand.transform.position), m_Blackboard.m_GroundLayerMask);
    }
    private bool OnSlope()
    {
        RaycastHit l_Hit;
        if(Physics.Raycast(m_Blackboard.m_Center.transform.position, m_Blackboard.m_Feet.transform.position - m_Blackboard.m_Center.transform.position, 
            out l_Hit, Vector3.Distance(m_Blackboard.m_Feet.transform.position, m_Blackboard.m_Center.transform.position)))
        {
            if (l_Hit.normal != Vector3.up)
            {
                return true;
            }
        }
        return false;
    }
}
