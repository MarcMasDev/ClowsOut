using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKFootBehaviour : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform m_FootTransformR;
    [SerializeField] private Transform m_FootTransformL;
    private Transform[] m_AllFootTransforms;
    [SerializeField] private Transform m_FootTargetTransformR;
    [SerializeField] private Transform m_FootTargetTransformL;
    private Transform[] m_AllFootTargetTransforms;
    [SerializeField] private TwoBoneIKConstraint m_FootIKR;
    [SerializeField] private TwoBoneIKConstraint m_FootIKL;
    private TwoBoneIKConstraint[] m_AllFootIKs;
    [SerializeField] Animator m_Animator;
    //[SerializeField] CharacterController m_CharacterController;
    [Header("Foot")]
    [SerializeField] private LayerMask m_GroundLayerMask;
    [SerializeField] private float m_MaxHitDistance;
    [SerializeField] private float m_AddedHeight;
    [SerializeField] private float m_DefaultYOffSet;
    [Header("Rotation")]
    [SerializeField] private float m_MaxBodyRotationX;
    [SerializeField] private float m_MaxBodyRotationZ;
    [SerializeField] private float m_MaxRotationStep;
    //[Header("Height")]
    //[SerializeField, Range(-0.5f, 2)] private float m_UpperFootYLimit = 0.3f;
    //[SerializeField, Range(-2, 0.5f)] private float m_LowerFootYLimit = -0.1f;
    //[SerializeField] private float m_AddedCapsuleColliderHeight = 0.05f;
    //[SerializeField] private float m_MaxCapsuleColliderHeight;


    private bool[] m_AllGroundHits;
    private Vector3[] m_AllHitNormals;
    private float m_AngleAboutX;
    private float m_AngleAboutZ;
    private float[] m_AllFootWeights;
    private Vector3 m_AveHitNormal;
    //private int[] m_CheckLocalTargetY;
    //private float m_InitialCapsuleColliderHeight;
    void Start()
    {
        m_AllFootTransforms = new Transform[2];
        m_AllFootTransforms[0] = m_FootTransformR;
        m_AllFootTransforms[1] = m_FootTransformL;

        m_AllFootTargetTransforms = new Transform[2];
        m_AllFootTargetTransforms[0] = m_FootTargetTransformR;
        m_AllFootTargetTransforms[1] = m_FootTargetTransformL;

        m_AllFootIKs = new TwoBoneIKConstraint[2];
        m_AllFootIKs[0] = m_FootIKR;
        m_AllFootIKs[1] = m_FootIKL;

        m_AllGroundHits = new bool[3];
        m_AllHitNormals = new Vector3[2];
        m_AllFootWeights = new float[2];
        //m_CheckLocalTargetY = new int[2];

        //m_InitialCapsuleColliderHeight = m_CharacterController.center.y;
    }

    void FixedUpdate()
    {
        RotateCharacterFeet();
        //RotateCharacterBody();
    }

    private void CheckGroundBelow(out Vector3 hitPoint, out bool gotGroundSpherecastHit, out Vector3 hitNormal,
        out float currentHitDistance, Transform objectTransform, int checkForLayerMask, float maxHitDistance, float addedHeight)
    {
        RaycastHit hit;
        Vector3 startSpherecast = objectTransform.position + new Vector3(0f, addedHeight, 0f);

        if (checkForLayerMask == -1)
        {
            Debug.LogError("Layer does not exist!");
            gotGroundSpherecastHit = false;
            currentHitDistance = 0f;
            hitNormal = Vector3.up;
            hitPoint = objectTransform.position;
        }
        else
        {
            if (Physics.SphereCast(startSpherecast, 0.2f, Vector3.down, out hit, maxHitDistance, checkForLayerMask, QueryTriggerInteraction.UseGlobal))
            {
                currentHitDistance = hit.distance - addedHeight;
                gotGroundSpherecastHit = true;
                hitNormal = hit.normal;
                hitPoint = hit.point;
            }
            else
            {
                gotGroundSpherecastHit = false;
                currentHitDistance = 0f;
                hitNormal = Vector3.up;
                hitPoint = objectTransform.position;
            }
        }
    }
    Vector3 ProjectOnContactPlane(Vector3 vector, Vector3 hitNormal)
    {
        return vector - hitNormal * Vector3.Dot(vector, hitNormal);
    }

    private void ProjectedAxisAngles(out float angleAboutX, out float angleAboutZ, Transform footTargetTransform, Vector3 hitNormal)
    {
        Vector3 l_XAxisProjected = ProjectOnContactPlane(footTargetTransform.forward, hitNormal).normalized;
        Vector3 l_ZAxisProjected = ProjectOnContactPlane(footTargetTransform.right, hitNormal).normalized;

        angleAboutX = Vector3.SignedAngle(footTargetTransform.forward, l_XAxisProjected, footTargetTransform.right);
        angleAboutZ = Vector3.SignedAngle(footTargetTransform.right, l_ZAxisProjected, footTargetTransform.forward);
    }

    private void RotateCharacterFeet()
    {
        m_AllFootWeights[0] = m_Animator.GetFloat("R Foot Weight");
        m_AllFootWeights[1] = m_Animator.GetFloat("L Foot Weight");

        for (int i = 0; i < m_AllFootIKs.Length; i++)
        {
            m_AllFootIKs[i].weight = m_AllFootWeights[i];

            CheckGroundBelow(out Vector3 l_Hitpoint, out m_AllGroundHits[i], out Vector3 l_HitNormal, out _,
                m_AllFootTransforms[i], m_GroundLayerMask, m_MaxHitDistance, m_AddedHeight);
            m_AllHitNormals[i] = l_HitNormal;

            if (m_AllGroundHits[i] == true)
            {
                float l_YOffset = m_DefaultYOffSet;

                ProjectedAxisAngles(out m_AngleAboutX, out m_AngleAboutZ, m_AllFootTransforms[i], m_AllHitNormals[i]);

                m_AllFootTargetTransforms[i].position = new Vector3(m_AllFootTransforms[i].position.x, l_Hitpoint.y + l_YOffset, m_AllFootTransforms[i].position.z);

                m_AllFootTargetTransforms[i].rotation = m_AllFootTransforms[i].rotation;
                m_AllFootTargetTransforms[i].localEulerAngles = new Vector3(m_AllFootTargetTransforms[i].localEulerAngles.x + m_AngleAboutX,
                    m_AllFootTargetTransforms[i].localEulerAngles.y, m_AllFootTargetTransforms[i].localEulerAngles.z + m_AngleAboutZ);
            }
            else
            {
                m_AllFootTargetTransforms[i].position = m_AllFootTransforms[i].position;

                m_AllFootTargetTransforms[i].rotation = m_AllFootTransforms[i].rotation;
            }
        }
    }
    private void RotateCharacterBody()
    {
        float l_AveHitNormalX = 0f;
        float l_AveHitNormalY = 0f;
        float l_AveHitNormalZ = 0f;

        for(int i = 0; i < 2; i++)
        {
            l_AveHitNormalX += m_AllHitNormals[i].x;
            l_AveHitNormalY += m_AllHitNormals[i].y;
            l_AveHitNormalZ += m_AllHitNormals[i].z;
        }
        m_AveHitNormal = new Vector3(l_AveHitNormalX / 2, l_AveHitNormalY / 2, l_AveHitNormalZ / 2);

        ProjectedAxisAngles(out m_AngleAboutX, out m_AngleAboutZ, transform, m_AveHitNormal);


        float l_CharacterXRotation = transform.eulerAngles.x;
        float l_CharacterZRotation = transform.eulerAngles.z;

        if (l_CharacterXRotation > 180)
        {
            l_CharacterXRotation -= 360;
        }
        if (l_CharacterZRotation > 180)
        {
            l_CharacterZRotation -= 360;
        }

        if (l_CharacterXRotation + m_AngleAboutX < -m_MaxBodyRotationX)
        {
            m_AngleAboutX = m_MaxBodyRotationX + l_CharacterXRotation;
        }
        else if (l_CharacterXRotation + m_AngleAboutX > m_MaxBodyRotationX)
        {
            m_AngleAboutX = m_MaxBodyRotationX - l_CharacterXRotation;
        }

        if (l_CharacterZRotation + m_AngleAboutZ < -m_MaxBodyRotationZ)
        {
            m_AngleAboutZ = m_MaxBodyRotationZ + l_CharacterZRotation;
        }
        else if (l_CharacterZRotation + m_AngleAboutZ > m_MaxBodyRotationZ)
        {
            m_AngleAboutZ = m_MaxBodyRotationZ - l_CharacterZRotation;
        }

        float l_BodyEulerX = Mathf.MoveTowardsAngle(0, m_AngleAboutX, m_MaxRotationStep);
        float l_BodyEulerZ = Mathf.MoveTowardsAngle(0, m_AngleAboutZ, m_MaxRotationStep);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x + l_BodyEulerX, transform.eulerAngles.y, transform.eulerAngles.z + l_BodyEulerZ);
    }
}
