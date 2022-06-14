using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RoomInfo
{
    public int m_MinDeadEnemies;
    public GameObject bulletToUnlock;
    public Animator door;
    public EnemiesDieCounter m_Enemies;
}
public class DoorManager : MonoBehaviour
{
    [SerializeField] private RoomInfo[] rooms;
    private int currentRoom = 0;
    private Vector3 powerUpPos; //This is done in order to avoid public voids or actions
    private Transform playerPos;
    private bool toOpen = false;
    private void Start()
    {
        playerPos = GameManager.GetManager().GetPlayer().transform;
    }

    void Update()
    {
        if (currentRoom < rooms.Length)
        {
            if (!toOpen && rooms[currentRoom].m_Enemies.m_DeathEnemies >= rooms[currentRoom].m_MinDeadEnemies)
            {//open, when pass close
                toOpen = true;
                powerUpPos = GameManager.GetManager().GetLastEnemyDeathPos();
                Instantiate(rooms[currentRoom].bulletToUnlock, powerUpPos, Quaternion.identity);
                currentRoom = GameManager.GetManager().GetCurrentRoomIndex() + 1;
                //m_Animator.SetBool("Open", true);
                //else
                //{
                //    m_Animator.SetBool("Open", false);
                //}
            }
        }
        if (toOpen && Vector3.Distance(playerPos.position, powerUpPos) <= 1.3f)
        {
            OpenDoor();
        }
    }
    private void OpenDoor()
    {
        toOpen = false;
        rooms[currentRoom-1].door.SetBool("Open", true);
    }

}
