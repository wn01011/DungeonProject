using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private GameObject groundRoom = null;
    private GameObject bossRoom = null;
    private GameObject entrance = null;
    private GameObject stair = null;
    private GameObject door = null;
    private Queue<GameObject> roomQueue = new Queue<GameObject>();

    private float width = 0.0f;
    private float height = 0.0f;
    private float lastRoomYPos = 0.0f;
    private int roomCount = 0;

    enum roomType
    {
        basic,
        boss,
        trap
    }
    // 0 : blank
    // 1 : skeleton
    private roomType[] roomMap = new roomType[]
    {
        roomType.basic, roomType.basic, roomType.basic,
        roomType.basic, roomType.basic, roomType.basic,
        roomType.basic, roomType.basic, roomType.basic,
        roomType.boss
    };
    
    #region initialization
    private void Awake()
    {
        roomCount = roomMap.Length;
        stair = Resources.Load<GameObject>("Prefabs\\Stair");
        groundRoom = Resources.Load<GameObject>("Prefabs\\Room_F1");
        bossRoom = Resources.Load<GameObject>("Prefabs\\Room_boss");
        entrance = Resources.Load<GameObject>("Prefabs\\Entrance");
        door = Resources.Load<GameObject>("Prefabs\\Door");
    }
    #endregion

    private GameObject RoomSelection(roomType _roomType)
    {
        if(_roomType == roomType.basic)
        {
            return groundRoom;
        }
        else if(_roomType== roomType.boss)
        {
            return bossRoom;
        }
        else if(_roomType == roomType.trap)
        {
            return null;
        }
        else
            return null;
    }
    private void Start()
    {
        //Entrance
        Instantiate(entrance, Vector3.up * 1.8f, Quaternion.identity);

        #region roomQueue.Enqueue()
       
        for (int i = 0; i < roomCount; ++i)
        {
            GameObject room = Instantiate(RoomSelection(roomMap[i]),transform.position, Quaternion.identity);
            room.transform.parent = this.transform;
            roomQueue.Enqueue(room);
        }
        #endregion

        #region roomQueue.Dequeue()
        
        for (int i=0; i<roomCount-1; ++i)
        {
            GameObject room = roomQueue.Peek();
            width = room.GetComponent<Collider>().bounds.size.x;
            height = room.GetComponent<Collider>().bounds.size.y;
            room.transform.localPosition = new Vector3(-width + width * (i % 3), -height * ((i/3) ), 0f);
            if((i%3)==0)
            {
                GameObject myDoor = Instantiate(door, room.transform.localPosition + Vector3.down * 0.3f + Vector3.right * width * Random.Range(0,3), Quaternion.identity);
            }
            lastRoomYPos = room.transform.localPosition.y;
            roomQueue.Dequeue();
        }
        //boss
        {
            GameObject room = roomQueue.Peek();
            height = lastRoomYPos - room.GetComponent<Collider>().bounds.size.y;
            room.transform.localPosition = new Vector3(0f, height + 0.9f, 0f);
            roomQueue.Dequeue();
        }
        
        #endregion
    GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
    for(int i=0; i<rooms.Length; ++i)
    {
        Debug.Log(rooms[i].name);
    }   
    }
}
