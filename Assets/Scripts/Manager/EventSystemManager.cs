using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventSystemManager : MonoBehaviour, IPointerDownHandler
{
    private void Start()
    {
        #region Initialization
        setMonsterPanelText = setMonsterPanel.GetComponentInChildren<Text>();
        hpControl = GetComponent<HPcontrol>();
        rooms = spawnManager.rooms;
        skeleton = Resources.Load<GameObject>("Prefabs\\Monsters\\SkeletonWarrior");
        deathbringer = Resources.Load<GameObject>("Prefabs\\Monsters\\DeathBringer");
        
        monsterImgs[0].sprite = skeletonSourceIamge;
        monsterImgs[1].sprite = deathBringerSourceIamge;
        monsterImgs[1].rectTransform.localScale = new Vector3(-1f, 1f, 1f);
        monsterImgs[1].GetComponentInChildren<Text>().rectTransform.localScale = new Vector3(-1f, 1f, 1f);
        #endregion
    }

    #region 몬스터 바꾸기 기능
    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject curTarget = eventData.pointerCurrentRaycast.gameObject;
        if(curTarget.CompareTag("SetMonster"))
        {
            SelectEffectOff();
            for (int i = 0; i < roomColArray.Length; ++i)
            {
                if (roomColArray[i] == curTarget)
                {
                    roomNum = i;
                    break;
                }
            }
            setMonsterPanelText.text = rooms[roomNum].name;
            if(rooms[roomNum].GetComponentInChildren<Monster>())
            { 
                Monster[] myMonsters = rooms[roomNum].GetComponentsInChildren<Monster>();
                for(int i=0; i< myMonsters.Length; ++i)
                {
                    if(myMonsters[i].GetComponent<Skeleton>())
                    {
                        curRoomMonsterImg[i].sprite = skeletonSourceIamge;
                        curRoomMonsterImg[i].rectTransform.localScale = new Vector3(-1f, 1f, 1f);
                    }
                    else if(myMonsters[i].GetComponent<DeathBringer>())
                    {
                        curRoomMonsterImg[i].sprite = deathBringerSourceIamge;
                        curRoomMonsterImg[i].rectTransform.localScale = new Vector3(1f, 1f, 1f);
                    }
                }
            }
            else
            {
                for(int i=0; i< curRoomMonsterImg.Length; ++i)
                {
                    curRoomMonsterImg[i].sprite = null;
                }
            }
            
            setMonsterPanel.SetActive(true);
            
        }
        else if(curTarget.CompareTag("MonsterImg"))
        {
            int num = 0;
            if(curTarget == monsterImgs[0].gameObject)
            {
                num = 0;
                selectEffect[1].SetActive(false);
            }
            else if(curTarget == monsterImgs[1].gameObject)
            {
                num = 1;
                selectEffect[0].SetActive(false);
            }
            selectEffect[num].SetActive(!selectEffect[num].activeSelf);

            if (selectEffect[0].activeSelf == true || selectEffect[1].activeSelf == true)
            {
                changeOn = true;
            }
            else changeOn = false;
        }
        else if(curTarget.CompareTag("CurRoomMonsterImg"))
        {
            int num = 0;
            if(curTarget == curRoomMonsterImg[0].gameObject)
            {
                num = 2;
                selectEffect[3].SetActive(false);
            }
            else if(curTarget == curRoomMonsterImg[1].gameObject)
            {
                num = 3;
                selectEffect[2].SetActive(false);
            }
            else
            {
                Debug.Log("바꿀몬스터가 없거나 죽었습니다.");
                setMonsterPanel.SetActive(false);
                return;
            }
            selectEffect[num].SetActive(!selectEffect[num].activeSelf);
        }
        else
        {
            Debug.Log("OnPointerDown Occur\n" + eventData.pointerCurrentRaycast.gameObject.name);
        }
    }
    public void ChangeBtn()
    {
        if (!changeOn || (!curRoomMonsterImg[0] && !curRoomMonsterImg[1])) return;

        int curRoomNum = 0;
        int monsterNum = 0;
        if(selectEffect[0].activeSelf == true)
        {
            if(gameManager.goods.bone >= 100)
            {
                gameManager.goods.bone -= 100;
            }
            else
            {
                Debug.Log(100 - gameManager.goods.bone + " Bone 이 모자랍니다.");
                return;
            }
            if (selectEffect[2].activeSelf == true)
            {
                monsterNum = 2 * roomNum;
                spawnManager.ChangeMonsterSpawnMap(monsterNum, SpawnManager.monsterType.skeleton);
                GameObject monster = Instantiate(skeleton, transform.position, Quaternion.identity, rooms[roomNum].transform);
                monster.transform.localPosition = new Vector3(0.9f, -0.5f); ;
                hpControl.ExchangeBarCoroutine(monsterNum, monster.GetComponent<Monster>());
                curRoomMonsterImg[curRoomNum].sprite = skeletonSourceIamge;
                spawnManager.monsterList[monsterNum] = monster;
            }
            else if (selectEffect[3].activeSelf == true)
            {
                monsterNum = 2 * roomNum + 1;
                spawnManager.ChangeMonsterSpawnMap(monsterNum, SpawnManager.monsterType.skeleton);
                GameObject monster = Instantiate(skeleton, transform.position, Quaternion.identity, rooms[roomNum].transform);
                monster.transform.localPosition = new Vector3(1.2f, -0.5f); ;
                hpControl.ExchangeBarCoroutine(monsterNum, monster.GetComponent<Monster>());
                curRoomMonsterImg[curRoomNum].sprite = skeletonSourceIamge;
                spawnManager.monsterList[monsterNum] = monster;
            }
            
        }
        else if(selectEffect[1].activeSelf == true)
        {
            if (gameManager.goods.bone >= 200)
            {
                gameManager.goods.bone -= 200;
            }
            else
            {
                Debug.Log(200 - gameManager.goods.bone + " Bone 이 모자랍니다.");
                return;
            }
            if (selectEffect[2].activeSelf == true)
            {
                monsterNum = 2 * roomNum;
                spawnManager.ChangeMonsterSpawnMap(monsterNum, SpawnManager.monsterType.deathbringer);
                GameObject monster = Instantiate(deathbringer, transform.position, Quaternion.identity, rooms[roomNum].transform);
                monster.transform.localPosition = new Vector3(0.9f, -0.5f); ;
                hpControl.ExchangeBarCoroutine(monsterNum, monster.GetComponent<Monster>());
                curRoomMonsterImg[curRoomNum].sprite = deathBringerSourceIamge;
                spawnManager.monsterList[monsterNum] = monster;
            }
            else if (selectEffect[3].activeSelf == true)
            {
                monsterNum = 2 * roomNum + 1;
                spawnManager.ChangeMonsterSpawnMap(monsterNum, SpawnManager.monsterType.deathbringer);
                GameObject monster = Instantiate(deathbringer, transform.position, Quaternion.identity, rooms[roomNum].transform);
                monster.transform.localPosition = new Vector3(1.2f, -0.5f); ;
                hpControl.ExchangeBarCoroutine(monsterNum, monster.GetComponent<Monster>());
                curRoomMonsterImg[curRoomNum].sprite = deathBringerSourceIamge;
                spawnManager.monsterList[monsterNum] = monster;
            }
        }
        spawnManager.monsterList[monsterNum].transform.parent = rooms[roomNum].transform;
    }
    private void SelectEffectOff()
    {
        for(int i=0; i< selectEffect.Length; ++i)
        {
            selectEffect[i].SetActive(false);
        }
    }
    #endregion

    private GameObject[] rooms = null;
    private Text setMonsterPanelText = null;
    
    [SerializeField] private GameObject setMonsterPanel = null;
    [SerializeField] private GameObject[] roomColArray = new GameObject[9];
    [SerializeField] private GameObject[] selectEffect = new GameObject[4];
    [SerializeField] private Image[] monsterImgs = null;
    [SerializeField] private Image[] curRoomMonsterImg = null;
    [SerializeField] private Sprite skeletonSourceIamge = null;
    [SerializeField] private Sprite deathBringerSourceIamge = null;

    [SerializeField] private GameManager gameManager = null;
    [SerializeField] private SpawnManager spawnManager = null;
    private HPcontrol hpControl = null;

    [SerializeField] private bool changeOn = false;
    private int roomNum = 0;
    private GameObject skeleton = null;
    private GameObject deathbringer = null;
}
