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
        zombie = Resources.Load<GameObject>("Prefabs\\Monsters\\Zombie");
        goblin = Resources.Load<GameObject>("Prefabs\\Monsters\\Goblin");
        flyingeye = Resources.Load<GameObject>("Prefabs\\Monsters\\FlyingEye");
        darkwizard = Resources.Load<GameObject>("Prefabs\\Monsters\\DarkWizard");
        deathbringer = Resources.Load<GameObject>("Prefabs\\Monsters\\DeathBringer");
        
        monsterImgs[0].sprite = skeletonSourceImage;
        monsterImgs[1].sprite = zombieSourceImage;
        monsterImgs[2].sprite = goblinSourceImage;
        monsterImgs[3].sprite = flyingeyeSourceImage;
        monsterImgs[4].sprite = darkwizardSourceImage;
        monsterImgs[5].sprite = deathBringerSourceImage;

        #endregion
    }

    #region 몬스터 바꾸기 기능

    //처음 패널을 열기위해 마우스클릭 = if(SetMonster),
    //원하는 몬스터를 패널의 왼쪽에서 선택 = else if(MonsterImg),
    //바꿀 몬스터를 패널의 오른쪽에서 선택 = else if(CurRoomMonsterImg)
    public void OnPointerDown(PointerEventData eventData)
    {
        //정비시간에만 작동하게 제한
        if (!uiManager.isMaintenance) return;

        GameObject curTarget = eventData.pointerCurrentRaycast.gameObject;

        //room의 오른쪽 collider를 누른다면 setMonsterPanel을 킨다.
        if (curTarget.CompareTag("SetMonster"))
        {
            SelectEffectOff();

            //현재 curTarget에 저장된 collider의 순서로 room의 순서를 알기 위해roomNum을 저장
            for (int i = 0; i < roomColArray.Length; ++i)
            {
                if (roomColArray[i] == curTarget)
                {
                    roomNum = i;
                    break;
                }
            }
            Debug.Log(curTarget.name);
            Debug.Log(roomNum);
            setMonsterPanelText.text = rooms[roomNum].name;

            //현재 방에 몬스터가 존재한다면 sprite를 해당 몬스터에 맞게 curRoomMonsterImg에 넣어준다.
            if (rooms[roomNum].GetComponentInChildren<Monster>())
            {
                Monster[] myMonsters = rooms[roomNum].GetComponentsInChildren<Monster>();
                for (int i = 0; i < myMonsters.Length; ++i)
                {
                    if (myMonsters[i].GetComponent<Skeleton>())
                    {
                        curRoomMonsterImg[i].sprite = skeletonSourceImage;
                        curRoomMonsterImg[i].rectTransform.localScale = new Vector3(-1f, 1f, 1f);
                    }
                    else if (myMonsters[i].GetComponent<DeathBringer>())
                    {
                        curRoomMonsterImg[i].sprite = deathBringerSourceImage;
                        curRoomMonsterImg[i].rectTransform.localScale = new Vector3(1f, 1f, 1f);
                    }
                }
            }
            //몬스터가 없다면 스프라이트를 null로 교체한다.
            else
            {
                for (int i = 0; i < curRoomMonsterImg.Length; ++i)
                {
                    curRoomMonsterImg[i].sprite = null;
                }
            }
            //모든 준비가 끝났으니 켜준다.
            setMonsterPanel.SetActive(!setMonsterPanel.activeSelf);

        }
        //패널이 켜졌을때 MonsterImg 또는 CurRoomMonsterImg를 누른다면 selectEffect를 껏다 키면서 선택되었음을 나타낸다.
        //한번에 여러개의 MonsterImg가 켜져서는 안된다. 마찬가지로 CurRoomMonster도 여러개가 켜질 수없다.
        else if (curTarget.CompareTag("MonsterImg"))
        {
            int num = 0;

            for (int i = 0; i < monsterImgs.Length; ++i)
            {
                if (curTarget == monsterImgs[i].gameObject)
                {
                    num = i;
                    monsterImgNum = i;
                    break;
                }
            }
            MonsterSelectEffectOff();
            selectEffect[num].SetActive(!selectEffect[num].activeSelf);

            //하나라도 selectEffect가 켜져있는 상태라면 바꿀 수 있는 상태 조건1충족 확인. (넣어줄 몬스터 선택 했는가)
            for (int i = 0; i < monsterImgs.Length; ++i)
            {
                if (selectEffect[i].activeSelf == true)
                {
                    monsterChangeOn = true;
                    break;
                }
                else monsterChangeOn = false;
            }
        }
        //위에선 바꾸고 싶은 몬스터의 종류를 골랐다면 이제 현재 방의 어떤 몬스터를 바꿀 것인지 클릭을 해서 정한다.
        else if (curTarget.CompareTag("CurRoomMonsterImg"))
        {
            int num = 0;

            for (int i = 0; i < curRoomMonsterImg.Length; ++i)
            {
                if (curTarget == curRoomMonsterImg[i].gameObject)
                {
                    num = i + monsterImgs.Length;
                    curRoomMonsterImgNum = i;
                    break;
                }
            }
            CurRoomMonsterSelectEffectOff();
            selectEffect[num].SetActive(!selectEffect[num].activeSelf);

            //몬스터 바꾸기 조건 2 충족 확인. (바뀔 몬스터 자리를 선택했는가)
            for(int i = monsterImgs.Length; i< selectEffect.Length; ++i)
            {
                if (selectEffect[i].activeSelf == true)
                {
                    curRoomMonsterChangeOn = true;
                    break;
                }
                else curRoomMonsterChangeOn = false;
            }
        }
        else
        {
            Debug.Log("OnPointerDown Occur\n" + eventData.pointerCurrentRaycast.gameObject.name);
        }
    }

    //바뀔수 있는 상태인지 체크 => 위의 OnPointerDown에서 구한 monsterImgNum과 curRoomMonsterImgNum으로 서로 바뀔 애들 확인했으므로
    //해당 아이들을 이용해 switch문 두개로 정의되어야할 변수들을 초기화 시켜줌.
    //마지막으로 실제로 바꿔주는 작업을 실행함
    public void ChangeBtn()
    {
        int monsterNum = 0;
        int price = 0;
        
        GameObject newMonster = null;
        SpawnManager.monsterType monsterType = SpawnManager.monsterType.blank;
        Vector3 newSpawnPos = Vector3.zero;
        Sprite curRoomImg = null;


        //바꿀 수 있는 상태인지 체크
        if (monsterChangeOn && curRoomMonsterChangeOn)
        {
            changeOn = true;
        }
        else changeOn = false;

        if (!changeOn) { Debug.Log("바꿀 수 없는 상태입니다."); return; }
        

        //몬스터의 종류에 따라 가격과 타입 프리뱁 소스이미지를 지정해준다.
        // 0 : skeleton, 1: deathBringer
        switch (monsterImgNum)
        {
            case 0:
                {
                    price = 100;
                    monsterType = SpawnManager.monsterType.skeleton;
                    newMonster = skeleton;
                    curRoomImg = skeletonSourceImage;
                }
                break;
            case 1:
                {
                    price = 200;
                    monsterType = SpawnManager.monsterType.zombie;
                    newMonster = zombie;
                    curRoomImg = zombieSourceImage;
                }
                break;
            case 2:
                {
                    price = 300;
                    monsterType = SpawnManager.monsterType.goblin;
                    newMonster = goblin;
                    curRoomImg = goblinSourceImage;
                }
                break;
            case 3:
                {
                    price = 400;
                    monsterType = SpawnManager.monsterType.flyingeye;
                    newMonster = flyingeye;
                    curRoomImg = flyingeyeSourceImage;
                }
                break;
            case 4:
                {
                    price = 500;
                    monsterType = SpawnManager.monsterType.darkwizard;
                    newMonster = darkwizard;
                    curRoomImg = darkwizardSourceImage;
                }
                break;
            case 5:
                {
                    price = 600;
                    monsterType = SpawnManager.monsterType.deathbringer;
                    newMonster = deathbringer;
                    curRoomImg = deathBringerSourceImage;
                }
                break;
            default:
                break;
        }

        //방의 첫번째 배치와 두번째 배치가 달라지기 때문에 monsterNum을 아래와 같이 지정해주고 후에 spawnManger의 ChangeMonsterSpawnMap() 함수를 통해 맵핑에 반영해준다.
        //이 monsterNum은 또 hpBar를 해당 몬스터에 다시 할당하는데 필요하다.
        // 0 : 방의 첫번째 자리, 1 : 방의 두번째 자리
        switch (curRoomMonsterImgNum)
        {
            case 0:
                {
                    monsterNum = 2 * roomNum;
                    newSpawnPos = new Vector3(0.8f, -0.65f);
                }
                break;
            case 1:
                {
                    monsterNum = 2 * roomNum + 1;
                    newSpawnPos = new Vector3(1.2f, -0.65f);
                }
                break;
            default:
                break;
        }

        //가격 체크
        if (gameManager.goods.bone >= price)
        {
            gameManager.goods.bone -= price;
        }
        else
        {
            Debug.Log(price - gameManager.goods.bone + " Bone 이 모자랍니다.");
            return;
        }
        //위에서 두개의 switch문에서 지정한 값들을 다 넣어준다.
        //spawnManager의 몬스터 맵핑을 바꾼 몬스터의 type에 맞게 다시 설정해준다.
        spawnManager.ChangeMonsterSpawnMap(monsterNum, monsterType);
        GameObject monster = Instantiate(newMonster, transform.position, Quaternion.identity, rooms[roomNum].transform);
        monster.transform.localPosition = newSpawnPos;
        hpControl.ExchangeBarCoroutine(monsterNum, monster.GetComponent<Monster>());
        curRoomMonsterImg[curRoomMonsterImgNum].sprite = curRoomImg;
        spawnManager.monsterList[monsterNum] = monster;
        spawnManager.monsterList[monsterNum].transform.parent = rooms[roomNum].transform;
    }

    #region SelectEffect Off Function
    //전체 이펙트 끄기, 왼쪽만 끄기, 오른쪽만 끄기
    private void SelectEffectOff()
    {
        for(int i=0; i< selectEffect.Length; ++i)
        {
            selectEffect[i].SetActive(false);
        }
    }
    private void MonsterSelectEffectOff()
    {
        for(int i=0; i<selectEffect.Length - curRoomMonsterImg.Length; ++i)
        {
            selectEffect[i].SetActive(false);
        }
    }
    private void CurRoomMonsterSelectEffectOff()
    {
        for(int i=monsterImgs.Length; i< selectEffect.Length; ++i)
        {
            selectEffect[i].SetActive(false);
        }
    }

    #endregion

    #endregion

    #region variables

    [Header("ETC")]
    [SerializeField] private GameObject[] roomColArray = new GameObject[9];
    [SerializeField] private GameObject setMonsterPanel = null;
    [SerializeField] private GameObject[] selectEffect = new GameObject[8];

    [Header("MonsterSourceImg")]
    [SerializeField] private Sprite skeletonSourceImage = null;
    [SerializeField] private Sprite zombieSourceImage = null;
    [SerializeField] private Sprite goblinSourceImage = null;
    [SerializeField] private Sprite flyingeyeSourceImage = null;
    [SerializeField] private Sprite darkwizardSourceImage = null;
    [SerializeField] private Sprite deathBringerSourceImage = null;
    
    [Header("MonsterImg")]
    [SerializeField] private Image[] monsterImgs = null;
    [SerializeField] private Image[] curRoomMonsterImg = null;

    [Header("Manager")]
    [SerializeField] private GameManager gameManager = null;
    [SerializeField] private SpawnManager spawnManager = null;
    [SerializeField] private UIManager uiManager = null;
    
    private GameObject[] rooms = null;
    private Text setMonsterPanelText = null;
    private HPcontrol hpControl = null;

    private bool changeOn = false;
    private bool monsterChangeOn = false;
    private bool curRoomMonsterChangeOn = false;
    private int monsterImgNum = 0;
    private int curRoomMonsterImgNum = 0;

    private int roomNum = 0;
    private GameObject skeleton = null;
    private GameObject zombie = null;
    private GameObject goblin = null;
    private GameObject flyingeye = null;
    private GameObject darkwizard = null;
    private GameObject deathbringer = null;

    #endregion

}
