using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RoomInfo
{
    public int m_MinDeadEnemies;
    public GameObject bulletToUnlock;
    public Animator door;
    public StudioEventEmitter emitter;
    public EnemiesDieCounter m_Enemies;
}
public class DoorManager : MonoBehaviour
{
    [SerializeField] private RoomInfo[] rooms;
    private int currentRoom = 0;
    private Vector3 powerUpPos; //This is done in order to avoid public voids or actions
    private Transform playerPos;
    private bool toOpen = false;
    private FMOD_Music music;
    private void Start()
    {
        playerPos = GameManager.GetManager().GetPlayer().transform;
        music = FindObjectOfType<FMOD_Music>();
    }

    void Update()
    {
        if (currentRoom < rooms.Length)
        {
            if (!toOpen && rooms[currentRoom].m_Enemies.m_DeathEnemies >= rooms[currentRoom].m_MinDeadEnemies)
            {//open, when pass close
                music.EndMusic();
                toOpen = true;
                if (rooms[currentRoom].bulletToUnlock)
                {
                    powerUpPos = GameManager.GetManager().GetLastEnemyDeathPos();
                    Instantiate(rooms[currentRoom].bulletToUnlock, powerUpPos, rooms[currentRoom].bulletToUnlock.transform.rotation);
                    currentRoom = GameManager.GetManager().GetCurrentRoomIndex() + 1;
                }
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
        rooms[currentRoom - 1].emitter?.Play();
    }

}
