using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player_MovementController : MonoBehaviour
{
    private Vector3 m_Direction;
    private float m_VerticalVelocity;

    [HideInInspector] public bool m_OnGround;

    private CharacterController m_CharacterController;

    private void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();
    }
    public void MovementUpdate(Vector2 inputAxis, Camera camera, float lerpRotationPct)
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

            if (m_Direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(m_Direction), lerpRotationPct);
                transform.eulerAngles = new Vector3(0, transform.rotation.y, 0);
            }
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
        m_Direction += forward * dashDirection.y;
        m_Direction += right * dashDirection.x;
        m_Direction.Normalize();
    }
    public void SetDashDirection(Camera camera)
    {
        Vector3 forward = camera.transform.forward;
        m_Direction = forward;
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
        Vector3 movement = new Vector3(m_Direction.x * velocity * Time.deltaTime, m_Direction.y, m_Direction.z * velocity * Time.deltaTime);
        m_CharacterController.Move(movement);
    }
}
