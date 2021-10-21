using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public enum roomType
    {
        basic,
        boss,
        fire,
        water,
        wind,
        thunder
    }
    private void RoomTypeCheck()
    {
        if (gameObject.name == "Room_F12(Clone)")
        {
            myType = roomType.basic;
        }
        else if (gameObject.name == "Room_boss(Clone)")
        {
            myType = roomType.boss;
        }
        else if (gameObject.name == "Room_Fire(Clone)")
        {
            myType = roomType.fire;
        }
        else if (gameObject.name == "Room_Water(Clone)")
        {
            myType = roomType.water;
        }
        else if (gameObject.name == "Room_Thunder(Clone)")
        {
            myType = roomType.thunder;
        }
        else if (gameObject.name == "Room_Wind(Clone)")
        {
            myType = roomType.wind;
        }

    }
    
    

    private void Start()
    {
        RoomTypeCheck();
        StartCoroutine(RoomLockCheck());
    }
    private IEnumerator RoomLockCheck()
    {
        while(true)
        {
            yield return new WaitUntil(() => GetComponentInChildren<Hero>());
            Hero[] heros = GetComponentsInChildren<Hero>();
            if(heros.Length >= 2)
            {
                for(int i =0; i< heroCapacity; ++i)
                {
                    heros[i].roomLock = false;
                }
                for(int i=heroCapacity; i<heros.Length; ++i)
                {
                    heros[i].roomLock = true;
                }
            }
            else if(heros.Length == 1)
            {
                heros[0].roomLock = false;
            }
            
            if(!GetComponentInChildren<Monster>())
            {
                for(int i=0; i<heros.Length; ++i)
                {
                    heros[i].roomLock = false;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    

    public bool roomLock = false;
    public int heroCapacity = 2;
    public roomType myType = roomType.basic;
}
