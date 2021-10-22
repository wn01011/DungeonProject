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

    #region ���� �ٲٱ� ���

    //ó�� �г��� �������� ���콺Ŭ�� = if(SetMonster),
    //���ϴ� ���͸� �г��� ���ʿ��� ���� = else if(MonsterImg),
    //�ٲ� ���͸� �г��� �����ʿ��� ���� = else if(CurRoomMonsterImg)
    public void OnPointerDown(PointerEventData eventData)
    {
        //����ð����� �۵��ϰ� ����
        if (!uiManager.isMaintenance) return;

        GameObject curTarget = eventData.pointerCurrentRaycast.gameObject;

        //room�� ������ collider�� �����ٸ� setMonsterPanel�� Ų��.
        if (curTarget.CompareTag("SetMonster"))
        {
            SelectEffectOff();

            //���� curTarget�� ����� collider�� ������ room�� ������ �˱� ����roomNum�� ����
            for (int i = 0; i < roomColArray.Length; ++i)
            {
                if (roomColArray[i] == curTarget)
                {
                    roomNum = i;
                    break;
                }
            }
            setMonsterPanelText.text = rooms[roomNum].name;

            //���� �濡 ���Ͱ� �����Ѵٸ� sprite�� �ش� ���Ϳ� �°� curRoomMonsterImg�� �־��ش�.
            if (rooms[roomNum].GetComponentInChildren<Monster>())
            {
                Monster[] myMonsters = rooms[roomNum].GetComponentsInChildren<Monster>();
                for (int i = 0; i < myMonsters.Length; ++i)
                {
                    if (myMonsters[i].GetComponent<Skeleton>())
                    {
                        curRoomMonsterImg[i].sprite = skeletonSourceIamge;
                        curRoomMonsterImg[i].rectTransform.localScale = new Vector3(-1f, 1f, 1f);
                    }
                    else if (myMonsters[i].GetComponent<DeathBringer>())
                    {
                        curRoomMonsterImg[i].sprite = deathBringerSourceIamge;
                        curRoomMonsterImg[i].rectTransform.localScale = new Vector3(1f, 1f, 1f);
                    }
                }
            }
            //���Ͱ� ���ٸ� ��������Ʈ�� null�� ��ü�Ѵ�.
            else
            {
                for (int i = 0; i < curRoomMonsterImg.Length; ++i)
                {
                    curRoomMonsterImg[i].sprite = null;
                }
            }
            //��� �غ� �������� ���ش�.
            setMonsterPanel.SetActive(!setMonsterPanel.activeSelf);

        }
        //�г��� �������� MonsterImg �Ǵ� CurRoomMonsterImg�� �����ٸ� selectEffect�� ���� Ű�鼭 ���õǾ����� ��Ÿ����.
        //�ѹ��� �������� MonsterImg�� �������� �ȵȴ�. ���������� CurRoomMonster�� �������� ���� ������.
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

            //�ϳ��� selectEffect�� �����ִ� ���¶�� �ٲ� �� �ִ� ���� ����1���� Ȯ��. (�־��� ���� ���� �ߴ°�)
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
        //������ �ٲٰ� ���� ������ ������ ����ٸ� ���� ���� ���� � ���͸� �ٲ� ������ Ŭ���� �ؼ� ���Ѵ�.
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

            //���� �ٲٱ� ���� 2 ���� Ȯ��. (�ٲ� ���� �ڸ��� �����ߴ°�)
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

    //�ٲ�� �ִ� �������� üũ => ���� OnPointerDown���� ���� monsterImgNum�� curRoomMonsterImgNum���� ���� �ٲ� �ֵ� Ȯ�������Ƿ�
    //�ش� ���̵��� �̿��� switch�� �ΰ��� ���ǵǾ���� �������� �ʱ�ȭ ������.
    //���������� ������ �ٲ��ִ� �۾��� ������
    public void ChangeBtn()
    {

        int monsterNum = 0;
        int price = 0;
        
        GameObject newMonster = null;
        SpawnManager.monsterType monsterType = SpawnManager.monsterType.blank;
        Vector3 newSpawnPos = Vector3.zero;
        Sprite curRoomImg = null;


        //�ٲ� �� �ִ� �������� üũ
        if (monsterChangeOn && curRoomMonsterChangeOn)
        {
            changeOn = true;
        }
        else changeOn = false;

        if (!changeOn) { Debug.Log("�ٲ� �� ���� �����Դϴ�."); return; }
        

        //������ ������ ���� ���ݰ� Ÿ�� ������ �ҽ��̹����� �������ش�.
        // 0 : skeleton, 1: deathBringer
        switch (monsterImgNum)
        {
            case 0:
                {
                    price = 100;
                    monsterType = SpawnManager.monsterType.skeleton;
                    newMonster = skeleton;
                    curRoomImg = skeletonSourceIamge;
                }
                break;
            case 1:
                {
                    price = 200;
                    monsterType = SpawnManager.monsterType.deathbringer;
                    newMonster = deathbringer;
                    curRoomImg = deathBringerSourceIamge;
                }
                break;
            default:
                break;
        }
        //���� ù��° ��ġ�� �ι�° ��ġ�� �޶����� ������ monsterNum�� �Ʒ��� ���� �������ְ� �Ŀ� spawnManger�� ChangeMonsterSpawnMap() �Լ��� ���� ���ο� �ݿ����ش�.
        //�� monsterNum�� �� hpBar�� �ش� ���Ϳ� �ٽ� �Ҵ��ϴµ� �ʿ��ϴ�.
        // 0 : ���� ù��° �ڸ�, 1 : ���� �ι�° �ڸ�
        switch (curRoomMonsterImgNum)
        {
            case 0:
                {
                    monsterNum = 2 * roomNum;
                    newSpawnPos = new Vector3(0.9f, -0.5f);
                }
                break;
            case 1:
                {
                    monsterNum = 2 * roomNum + 1;
                    newSpawnPos = new Vector3(1.2f, -0.5f);
                }
                break;
            default:
                break;
        }

        //���� üũ
        if (gameManager.goods.bone >= price)
        {
            gameManager.goods.bone -= price;
        }
        else
        {
            Debug.Log(price - gameManager.goods.bone + " Bone �� ���ڶ��ϴ�.");
            return;
        }
        //������ �ΰ��� switch������ ������ ������ �� �־��ش�.
        //spawnManager�� ���� ������ �ٲ� ������ type�� �°� �ٽ� �������ش�.
        spawnManager.ChangeMonsterSpawnMap(monsterNum, monsterType);
        GameObject monster = Instantiate(newMonster, transform.position, Quaternion.identity, rooms[roomNum].transform);
        monster.transform.localPosition = newSpawnPos;
        hpControl.ExchangeBarCoroutine(monsterNum, monster.GetComponent<Monster>());
        curRoomMonsterImg[curRoomMonsterImgNum].sprite = curRoomImg;
        spawnManager.monsterList[monsterNum] = monster;
        spawnManager.monsterList[monsterNum].transform.parent = rooms[roomNum].transform;
    }

    #region SelectEffect Off Function
    //��ü ����Ʈ ����, ���ʸ� ����, �����ʸ� ����
    private void SelectEffectOff()
    {
        for(int i=0; i< selectEffect.Length; ++i)
        {
            selectEffect[i].SetActive(false);
        }
    }
    private void MonsterSelectEffectOff()
    {
        for(int i=0; i<selectEffect.Length - monsterImgs.Length; ++i)
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
    [SerializeField] private UIManager uiManager = null;
    private HPcontrol hpControl = null;

    private bool changeOn = false;
    private bool monsterChangeOn = false;
    private bool curRoomMonsterChangeOn = false;
    private int monsterImgNum = 0;
    private int curRoomMonsterImgNum = 0;

    private int roomNum = 0;
    private GameObject skeleton = null;
    private GameObject deathbringer = null;
}
