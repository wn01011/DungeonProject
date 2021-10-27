using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public enum monsterType
    {
        blank,
        skeleton,
        zombie,
        goblin,
        flyingeye,
        darkwizard,
        deathbringer,
        boss
    }
    enum heroType
    {
        knight,
        bandit
    }

    //monster spawn Mapping
    //room(,)    room(,)    room(,)
    //room(,)    room(,)    room(,)
    //room(,)    room(,)    room(,)
    //boss
    public monsterType[] spawn = new monsterType[]
    {
        monsterType.skeleton, monsterType.skeleton,             monsterType.skeleton, monsterType.skeleton,             monsterType.skeleton, monsterType.skeleton,
        monsterType.skeleton, monsterType.skeleton,             monsterType.skeleton, monsterType.skeleton,             monsterType.skeleton, monsterType.skeleton,
        monsterType.skeleton, monsterType.skeleton,             monsterType.skeleton, monsterType.skeleton,             monsterType.skeleton, monsterType.skeleton,
        monsterType.boss
    };
    //hero spawn Mapping
    private heroType[] heroSpawn = new heroType[]
    {
        heroType.knight, heroType.bandit, heroType.knight,
        heroType.bandit
    };

    private void Awake()
    {
        #region Initialization
        monsterCount = spawn.Length;
        heroCount = heroSpawn.Length;
        skeleton = Resources.Load<GameObject>("Prefabs\\Monsters\\SkeletonWarrior");
        zombie = Resources.Load<GameObject>("Prefabs\\Monsters\\Zombie");
        goblin = Resources.Load<GameObject>("Prefabs\\Monsters\\Goblin");
        flyingEye = Resources.Load<GameObject>("Prefabs\\Monsters\\FlyingEye");
        darkWizard = Resources.Load<GameObject>("Prefabs\\Monsters\\DarkWizard");
        deathbringer = Resources.Load<GameObject>("Prefabs\\Monsters\\DeathBringer");
        knight = Resources.Load<GameObject>("Prefabs\\Heroes\\Knight");
        bandit = Resources.Load<GameObject>("Prefabs\\Heroes\\Bandit");
        boss = Resources.Load<GameObject>("Prefabs\\Monsters\\Boss");
        #endregion
    }

    void Start()
    {
        rooms = GameObject.FindGameObjectsWithTag("Room");
        skeleton.GetComponent<SpriteRenderer>().flipX = true;
        bandit.GetComponent<SpriteRenderer>().flipX = true;

        SpawnHero();
        SpawnMonster();
        MonsterPositioning();
        StartCoroutine(heroWave());
    }
    public void SpawnMapping()
    {
        //GameManager에서 PlayerPrefs에서 가져온 spawnMappingArray를 동기화 시켜줌
        for (int i = 0; i < spawn.Length; ++i)
        {
            spawn[i] = (monsterType)gameManager.spawnMappingArray[i];
        }
    }
    private GameObject MonsterSelection(monsterType _monsterType)
    {
        switch (_monsterType)
        {
            case monsterType.blank:
                return null;
            case monsterType.skeleton:
                return skeleton;
            case monsterType.zombie:
                return zombie;
            case monsterType.goblin:
                return goblin;
            case monsterType.flyingeye:
                return flyingEye;
            case monsterType.darkwizard:
                return darkWizard;
            case monsterType.deathbringer:
                return deathbringer;
            case monsterType.boss:
                return boss;
            default:
                return null;
        }
    }
    private GameObject HeroSelection(heroType _heroType)
    {
        switch (_heroType)
        {
            case heroType.knight:
                return knight;
            case heroType.bandit:
                return bandit;
            default:
                return null;
        }
    }

    private void SpawnHero()
    {
        curHeroCount = heroCount;
        maxHeroCount = heroCount;
        uiManager.WaveTextUpdate();

        for(int i=0; i<heroCount; ++i)
        {
            if(HeroSelection(heroSpawn[i]))
            {
                GameObject hero = Instantiate(HeroSelection(heroSpawn[i]), Vector3.right * 100, Quaternion.identity, transform);
                heroQueue.Enqueue(hero);
            }
        }
    }
    private IEnumerator heroWave()
    {
        while(heroQueue.Count != 0)
        {
            heroQueue.Peek().transform.parent = this.transform;
            heroQueue.Peek().transform.localPosition = new Vector3(-6f, 1.4f, 0f);
            heroQueue.Peek().GetComponentInChildren<Hero>().moveStart = true;
            heroQueue.Peek().GetComponentInChildren<Animator>().Play("Run");
            heroQueue.Dequeue();
            yield return new WaitForSeconds(1f);
        }
    }
    private void SpawnMonster()
    {
        for (int i = 0; i < monsterCount-1; ++i)
        {
            if (MonsterSelection(spawn[i]))
            {
                GameObject monster = Instantiate(MonsterSelection(spawn[i]), transform.position, Quaternion.identity);
                monsterList.Add(monster);
            }
        }
        {
            GameObject myBoss = Instantiate(boss, transform.position, Quaternion.identity);
            monsterList.Add(myBoss);
        }
    }
    private void MonsterPositioning()
    {
        int blankJumpCount = 0;
        for(int i =0; i<rooms.Length-1; ++i)
        {
            if (spawn[2 * i] == monsterType.blank)
            {
                blankJumpCount++;
                continue;
            }
            else
            {
                monsterList[2 * i-blankJumpCount].transform.parent = rooms[i].transform;
                monsterList[2 * i-blankJumpCount].transform.localPosition = new Vector3(0.8f, -0.65f);
                monsterList[2 * (i - blankJumpCount) + 1].transform.parent = rooms[i].transform;
                monsterList[2 * (i - blankJumpCount) + 1].transform.localPosition = new Vector3(1.2f, -0.65f);
            }
        }
        //bossPositioning
        {
            monsterList[monsterList.Count-1].transform.parent = rooms[rooms.Length - 1].transform;
            monsterList[monsterList.Count-1].transform.localPosition = bossPos + (Vector3.up*0.2f);
        }
    }
    public void Restart()
    {
        StartCoroutine(RestartCoroutine());
    }
    private IEnumerator RestartCoroutine()
    {
        if(gameManager.waveUp)
        {
            for(int i=0; i< GameManager.wave; ++i)
            {
                GameObject newHero = Instantiate(HeroSelection(heroSpawn[i]), Vector3.right * 100f, Quaternion.identity, transform);
                hpControl.NewHpBarMaker(newHero);
            }
            gameManager.waveUp = false;
        }
        yield return new WaitForSeconds(0.5f);

        Hero[] heros = FindObjectsOfType<Hero>();
        curHeroCount = heros.Length;
        maxHeroCount = heros.Length;
        uiManager.WaveTextUpdate();

        foreach (GameObject monster in monsterList)
        {
            monster.GetComponent<Monster>().ReStart();
        }
        monsterList[monsterList.Count - 1].transform.localPosition = bossPos;
        for (int i = 0; i < heros.Length; ++i)
        {
            heros[i].transform.parent = this.transform;
            heros[i].transform.position = waitPos;
            heros[i].ReStart();
            heroQueue.Enqueue(heros[i].gameObject);
        }
        MonsterPositioning();

        //던전 유지보수 시간 동안 다음 웨이브 진행 멈춤
        isMaintenance = true;
        while(isMaintenance)
        {
            isMaintenance = uiManager.isMaintenance;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitUntil(() => !isMaintenance);
        

        StartCoroutine(heroWave());
    }
    public void ChangeMonsterSpawnMap(int _aryNum, monsterType _target)
    {
        spawn[_aryNum] = _target;
    }

   

    private GameObject skeleton = null;
    private GameObject zombie = null;
    private GameObject goblin = null;
    private GameObject flyingEye = null;
    private GameObject darkWizard = null;
    private GameObject deathbringer = null;

    private GameObject knight = null;
    private GameObject bandit = null;

    private GameObject boss = null;

    private int monsterCount = 0;
    private int heroCount = 0;
    public int curHeroCount = 0;
    public int maxHeroCount = 0;

    public  List<GameObject> monsterList = new List<GameObject>();
    private Queue<GameObject> heroQueue = new Queue<GameObject>();

    private Vector3 waitPos = new Vector3(100f, 0f, 0f);
    private Vector3 bossPos = new Vector3(1.29f, -1.68f);
    
    public GameObject[] rooms = null;
    public bool isMaintenance = false;

    [SerializeField] private GameManager gameManager = null;
    [SerializeField] private UIManager uiManager = null;
    [SerializeField] private HPcontrol hpControl = null;
}
