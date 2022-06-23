using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    [SerializeField] private TMP_Text texts;
    [SerializeField] private GameObject canvases;
    [SerializeField] private float time = 80;
    private float currenTime = 80;
    private bool lastRoom = false;
    private void Start()
    {
        playerPos = GameManager.GetManager().GetPlayer().transform;
        music = FindObjectOfType<FMOD_Music>();
        currenTime = time;
    }

    void Update()
    {
        //print("currentRoom "+currentRoom);
        if (currentRoom < rooms.Length)
        {
            Debug.Log("Room Check " + toOpen);
            Debug.Log("Room Check " + rooms[currentRoom].m_Enemies.m_DeathEnemies + " " + rooms[currentRoom].m_MinDeadEnemies);
            if (!toOpen && rooms[currentRoom].m_Enemies.m_DeathEnemies >= rooms[currentRoom].m_MinDeadEnemies)
            {//open, when pass close
               
                    music.EndMusic();
                
                toOpen = true;
                Debug.Log("Room Check " + rooms[currentRoom].bulletToUnlock);
                if (rooms[currentRoom].bulletToUnlock)
                {
                    powerUpPos = GameManager.GetManager().GetLastEnemyDeathPos();
                    Instantiate(rooms[currentRoom].bulletToUnlock, powerUpPos, rooms[currentRoom].bulletToUnlock.transform.rotation);
                    print("currentRoom instancio ");
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

        if (lastRoom)
        {
            if (currenTime <= 0)
            {
                lastRoom = false;
                texts.text = "?";
            }
            else
            {
                texts.text = Mathf.RoundToInt(currenTime).ToString();
                currenTime -= Time.deltaTime;
            }
        }
    }
    private void OpenDoor()
    {
        print("openmusic");
        toOpen = false;
        rooms[currentRoom-1].door.SetBool("Open", true);
        rooms[currentRoom - 1].emitter?.Play();

        if (currentRoom >= 5)
        {
            StartCoroutine(OpenFinal());
        }
    }
    private IEnumerator OpenFinal()
    {
        lastRoom = true;
        yield return new WaitForSeconds(time);

        powerUpPos = GameManager.GetManager().GetLastEnemyDeathPos();
        Instantiate(rooms[currentRoom].bulletToUnlock, powerUpPos, rooms[currentRoom].bulletToUnlock.transform.rotation);
        print("currentRoom instancio ");
        currentRoom = GameManager.GetManager().GetCurrentRoomIndex() + 1;
        OpenDoor();
        toOpen = false;
        rooms[5].door.SetBool("Open", true);
        rooms[5].emitter?.Play();

    }
}
