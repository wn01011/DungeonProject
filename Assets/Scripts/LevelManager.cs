using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private GameObject basicRoom = null;
    private GameObject bossRoom = null;
    private GameObject fireRoom = null;
    private GameObject waterRoom = null;
    private GameObject windRoom = null;
    private GameObject thunderRoom = null;
    private GameObject earthRoom = null;
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
        fire,
        water,
        earth,
        wind,
        thunder
    }

    private roomType[] roomMap = new roomType[]
    {
        roomType.earth, roomType.fire, roomType.basic,
        roomType.thunder, roomType.water, roomType.basic,
        roomType.wind, roomType.basic, roomType.basic,
        roomType.boss
    };
    
    #region initialization
    private void Awake()
    {
        roomCount = roomMap.Length;
        stair = Resources.Load<GameObject>("Prefabs\\Stair");
        basicRoom = Resources.Load<GameObject>("Prefabs\\Room_F1");
        bossRoom = Resources.Load<GameObject>("Prefabs\\Room_boss");
        fireRoom = Resources.Load<GameObject>("Prefabs\\Room_Fire");
        waterRoom = Resources.Load<GameObject>("Prefabs\\Room_Water");
        windRoom = Resources.Load<GameObject>("Prefabs\\Room_Wind");
        earthRoom = Resources.Load<GameObject>("Prefabs\\Room_Earth");
        thunderRoom = Resources.Load<GameObject>("Prefabs\\Room_Thunder");
        entrance = Resources.Load<GameObject>("Prefabs\\Entrance");
        door = Resources.Load<GameObject>("Prefabs\\Door");
    }
    #endregion

    private GameObject RoomSelection(roomType _roomType)
    {
        switch (_roomType)
        {
            case roomType.basic:
                return basicRoom;
            case roomType.boss:
                return bossRoom;
            case roomType.fire:
                return fireRoom;
            case roomType.water:
                return waterRoom;
            case roomType.earth:
                return earthRoom;
            case roomType.wind:
                return windRoom;
            case roomType.thunder:
                return thunderRoom;
            default:
                return null;
        } 
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
       
    }
}
