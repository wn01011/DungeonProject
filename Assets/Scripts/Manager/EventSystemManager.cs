using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventSystemManager : MonoBehaviour, IPointerDownHandler
{
    private void Start()
    {
        setMonsterPanelText = setMonsterPanel.GetComponentInChildren<Text>();
        hpControl = GetComponent<HPcontrol>();
        rooms = spawnManager.rooms;
        skeleton = Resources.Load<GameObject>("Prefabs\\Monsters\\SkeletonWarrior");
        deathbringer = Resources.Load<GameObject>("Prefabs\\Monsters\\DeathBringer");
        curRoomMonsterImg = setMonsterPanel.GetComponentsInChildren<Image>()[1];
        monsterImgs[0].sprite = skeletonSourceIamge;
        monsterImgs[1].sprite = deathBringerSourceIamge;
        monsterImgs[1].rectTransform.localScale = new Vector3(-1f, 1f, 1f);
        monsterImgs[1].GetComponentInChildren<Text>().rectTransform.localScale = new Vector3(-1f, 1f, 1f);
        for(int i=0; i< selectEffect.Length; ++i)
        {
            selectEffect[i] = monsterImgs[i].GetComponentsInChildren<RectTransform>()[1].gameObject;
            selectEffect[i].SetActive(false);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject curTarget = eventData.pointerCurrentRaycast.gameObject;
        if(curTarget.CompareTag("SetMonster"))
        {
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
                setMonsterPanelText.text += "\nyou have " + rooms[roomNum].GetComponentsInChildren<Monster>()[0].name;
                if(rooms[roomNum].GetComponentInChildren<DeathBringer>())
                {
                    curRoomMonsterImg.sprite = deathBringerSourceIamge;
                    curRoomMonsterImg.rectTransform.localScale = Vector3.one;
                }
                else if(rooms[roomNum].GetComponentInChildren<Skeleton>())
                {
                    curRoomMonsterImg.sprite = skeletonSourceIamge;
                    curRoomMonsterImg.rectTransform.localScale = new Vector3(-1f, 1f, 1f);
                }
            }
            else
            {
                curRoomMonsterImg.sprite = null;
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
        else
        {
            Debug.Log("OnPointerDown Occur\n" + eventData.pointerCurrentRaycast.gameObject.name);
        }
    }
    public void ChangeBtn()
    {
        if (!changeOn || !curRoomMonsterImg) return;
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
            spawnManager.ChangeMonsterSpawnMap(2 * roomNum, SpawnManager.monsterType.skeleton);
            GameObject monster = Instantiate(skeleton,transform.position, Quaternion.identity, rooms[roomNum].transform);
            monster.transform.localPosition = new Vector3(0.9f, -0.5f); ;
            hpControl.ExchangeBarCoroutine(roomNum, monster.GetComponent<Monster>());
            curRoomMonsterImg.sprite = skeletonSourceIamge;
            spawnManager.monsterList[roomNum] = monster;
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
            spawnManager.ChangeMonsterSpawnMap(2 * roomNum, SpawnManager.monsterType.deathbringer);
            GameObject monster = Instantiate(deathbringer, transform.position, Quaternion.identity, rooms[roomNum].transform);
            monster.transform.localPosition = new Vector3(0.9f, -0.5f);
            hpControl.ExchangeBarCoroutine(roomNum, monster.GetComponent<Monster>());
            curRoomMonsterImg.sprite = deathBringerSourceIamge;
            spawnManager.monsterList[roomNum] = monster;
        }
        spawnManager.monsterList[roomNum].transform.parent = rooms[roomNum].transform;
    }


    private GameObject[] rooms = null;
    private Text setMonsterPanelText = null;
    private Image curRoomMonsterImg = null;
    
    [SerializeField] private GameObject setMonsterPanel = null;
    [SerializeField] private GameObject[] roomColArray = new GameObject[9];
    [SerializeField] private Image[] monsterImgs = null;
    [SerializeField] private Sprite skeletonSourceIamge = null;
    [SerializeField] private Sprite deathBringerSourceIamge = null;

    [SerializeField] private GameManager gameManager = null;
    [SerializeField] private SpawnManager spawnManager = null;
    private HPcontrol hpControl = null;

    private GameObject[] selectEffect = new GameObject[2];
    [SerializeField] private bool changeOn = false;
    private int roomNum = 0;
    private GameObject skeleton = null;
    private GameObject deathbringer = null;
}
