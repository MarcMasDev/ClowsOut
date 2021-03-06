using UnityEngine;

public class InteractFont : MonoBehaviour
{
    private void Update()
    {
        Vector3 l_LookPos = transform.position - GameManager.GetManager().GetCameraManager().m_Camera.transform.position;
        var l_Rotation = Quaternion.LookRotation(l_LookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, l_Rotation, 0.5f);
    }
}
