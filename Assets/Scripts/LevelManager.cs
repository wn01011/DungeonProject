using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private GameObject groundRoom = null;
    private GameObject basicRoom = null;
    private GameObject bossRoom = null;
    private GameObject entrance = null;
    
    private Queue<GameObject> roomQueue = new Queue<GameObject>();

    private float width = 0.0f;
    private float height = 0.0f;
    private float lastRoomYPos = 0.0f;
    private int roomCount = 9;

    enum roomType
    {
        basic,
        boss,
        trap
    }

    #region initialization
    private void Awake()
    {
        groundRoom = Resources.Load<GameObject>("Prefabs\\Room_F1");
        basicRoom = Resources.Load<GameObject>("Prefabs\\Room_basic");
        bossRoom = Resources.Load<GameObject>("Prefabs\\Room_boss");
        entrance = Resources.Load<GameObject>("Prefabs\\Entrance");
    }
    #endregion

    private void Start()
    {
        //Entrance
        Instantiate(entrance, Vector3.up * 1.8f, Quaternion.identity);

        #region roomQueue.Enqueue() ground -> basic -> boss
        //ground
        for (int i=0; i<3; ++i)
        {
            GameObject room = Instantiate(groundRoom, transform.position, Quaternion.identity);
            room.transform.parent = this.transform;
            roomQueue.Enqueue(room);
        }
        //basic
        for(int i=0; i<6; ++i)
        {
            GameObject room = Instantiate(basicRoom, transform.position, Quaternion.identity);
            room.transform.parent = this.transform;
            roomQueue.Enqueue(room);
        }
        //boss
        {
            GameObject room = Instantiate(bossRoom, transform.position, Quaternion.identity);
            room.transform.parent = this.transform;
            roomQueue.Enqueue(room);
        }
        #endregion

        #region roomQueue.Dequeue() ground -> basic -> boss
        //ground
        for (int i = 0; i < 3; ++i)
        {
            GameObject room = roomQueue.Peek();
            width = room.GetComponent<Collider>().bounds.size.x;
            room.transform.localPosition = new Vector3(-width + width * i, 0f, 0f);
            roomQueue.Dequeue();
        }
        //basic
        for (int i=0; i<6; ++i)
        {
            GameObject room = roomQueue.Peek();
            width = room.GetComponent<Collider>().bounds.size.x;
            height = room.GetComponent<Collider>().bounds.size.y;
            room.transform.localPosition = new Vector3(-width + width * (i % 3), -height * ((i/3) + 1), 0f);
            lastRoomYPos = room.transform.localPosition.y;
            roomQueue.Dequeue();
        }
        //boss
        {
            GameObject room = roomQueue.Peek();
            height = room.GetComponent<Collider>().bounds.size.y;
            room.transform.localPosition = new Vector3(0, lastRoomYPos - height + 0.9f, 0);
        }
        #endregion
    }
}
