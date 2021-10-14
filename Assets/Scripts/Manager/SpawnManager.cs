using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private GameObject skeleton = null;
    private GameObject deathbringer = null;
    private GameObject knight = null;
    public GameObject boss = null;

    private int monsterCount = 0;

    enum monsterType
    {
        blank,
        skeleton,
        deathbringer,
        boss
    }

    private monsterType[] spawn = new monsterType[]
    {
        monsterType.blank, monsterType.deathbringer, monsterType.skeleton,
        monsterType.skeleton, monsterType.blank, monsterType.skeleton,
        monsterType.blank, monsterType.deathbringer, monsterType.skeleton,
        monsterType.boss
    };
    
    private void Awake()
    {
        monsterCount = spawn.Length;
        skeleton = Resources.Load<GameObject>("Prefabs\\Monsters\\SkeletonWarrior");
        deathbringer = Resources.Load<GameObject>("Prefabs\\Monsters\\DeathBringer");
        knight = Resources.Load<GameObject>("Prefabs\\Heroes\\Knight");
        boss = Resources.Load<GameObject>("Prefabs\\Monsters\\Boss");
    }

    private GameObject MonsterSelection(monsterType _monsterType)
    {
        switch (_monsterType)
        {
            case monsterType.blank:
                return null;
            case monsterType.skeleton:
                return skeleton;
            case monsterType.deathbringer:
                return deathbringer;
            case monsterType.boss:
                return boss;
            default:
                return null;
        }

    }

    void Start()
    {

        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
        skeleton.GetComponent<SpriteRenderer>().flipX = true;

        GameObject myKight = Instantiate(knight, Vector3.zero, Quaternion.identity);
        myKight.transform.parent = this.transform;

        for (int i = 0; i < monsterCount-1; ++i)
        {
            if(MonsterSelection(spawn[i]))
            {
                GameObject monster = Instantiate(MonsterSelection(spawn[i]), transform.position, Quaternion.identity);
                monster.transform.parent = rooms[i].transform;
               
                monster.transform.localPosition = new Vector3(0.75f,-0.74f);
            }
        }
        { 
            GameObject myBoss = Instantiate(boss, transform.position, Quaternion.identity);
            myBoss.transform.parent = rooms[rooms.Length-1].transform;
            myBoss.transform.localPosition = new Vector3(1.29f, -1.68f);
        }
    }

    public void Restart()
    {
        StopAllCoroutines();
        for (int i = 0; i < GetComponentsInChildren<Transform>().Length - 2; ++i)
            Destroy(GetComponentsInChildren<Transform>()[i].gameObject);
        Start();
    }
}
