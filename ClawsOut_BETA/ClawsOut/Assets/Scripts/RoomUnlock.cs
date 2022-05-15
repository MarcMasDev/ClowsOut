using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomUnlock : MonoBehaviour
{
    public Animation m_Animation;
    private bool m_RoomCompleted;
    private void Start()
    {
    }
    void Update()
    {
        if (!m_RoomCompleted)
        {
            if (IsRoomComplete())
            {
                m_Animation.Play();
                m_RoomCompleted = true;
            }
        }
    }
    bool IsRoomComplete()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                return false;
            }
        }
        return true;
    }
}
