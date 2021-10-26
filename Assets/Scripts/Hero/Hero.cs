using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Hero : MonoBehaviour
{

    protected virtual void Start()
    {
        #region Initialization

        animator = GetComponentInChildren<Animator>();
        heroRb = GetComponentInChildren<Rigidbody2D>();
        heroSr = GetComponentInChildren<SpriteRenderer>();
        tempColor = heroSr.color;

        entranceTr = GameObject.FindGameObjectWithTag("Entrance").GetComponent<Transform>();
        doors = GameObject.FindGameObjectsWithTag("Door");
        rooms = GameObject.FindGameObjectsWithTag("Room");
        spawnManager = FindObjectOfType<SpawnManager>();
        gameManager = FindObjectOfType<GameManager>();
        uiManager = FindObjectOfType<UIManager>();
        bossRoomTr = rooms[rooms.Length-1].transform;
        height = rooms[0].GetComponent<Collider>().bounds.size.y;
        offset = new Vector3(-6f, -0.4f, 0f);

        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 10;
       
        #endregion

        StartCoroutine(heroMoveAlgorithm());
        StartCoroutine(RoomCheck());
        StartCoroutine(roomMonsterCheck());
    }

    abstract protected void Attack();

    abstract protected void Active_Skill();

    abstract protected void Passive_Skill();

    #region Get & SetHp

    public void SetHp()
    {
        hp = maxHp;
    }

    public float GetHp()
    {
        return hp;
    }

    public float GetMaxHp()
    {
        return maxHp;
    }

    #endregion

    public void Hurt(float _damage, float _defense)
    {
        float trueDamage = _damage - _defense;
        if(trueDamage < 0)
        {
            trueDamage = 0;
        }
        
        if (hp - trueDamage <= 0)
        {
            hp = 0;
            Dead();
        }
        else if(trueDamage != 0)
        {
            if(!isHurtColor)
                StartCoroutine(HurtAlphaChange());
            if(isHurtColor)
            {
                StopCoroutine(HurtAlphaChange());
                heroSr.color = tempColor;
                StartCoroutine(HurtAlphaChange());
            }
            hp -= trueDamage;       
        }
    }

    IEnumerator HurtAlphaChange()
    {
        isHurtColor = true;
        int negativeSwitch = 1; 
        for(int i = 0; i < 3f; ++i)
        {
            if(negativeSwitch == 1)
            {
                heroSr.color = Color.red;
            }
            else
            {
                heroSr.color = tempColor;
            }
            negativeSwitch *= -1;
            yield return new WaitForSeconds(0.1f);
        }
        heroSr.color = tempColor;
    }
    protected void Dead()
    {
        if (isDie) return;
        gameManager.goods.bone += 10;
        isMove = false;
        transform.position = waitPos;
        transform.parent = spawnManager.transform;
        --spawnManager.curHeroCount;
        uiManager.WaveTextUpdate();

        animator.SetTrigger("Die");
        isDie = true;
        StartCoroutine(DieWait());
    }

    protected IEnumerator DieWait()
    {
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        moveStart = false;
        isAttack = false;
        roomLock = false;
        curRoom = null;
    }
    
    public void ReStart()
    {
        #region restart initialization

        maxHp = 10 + GameManager.wave * 5;
        damage = GameManager.wave;
        defense = GameManager.wave;
        
        SetHp();
        isDie = false;
        moveStart = false;
        isMove = true;
        isAttack = false;
        roomLock = false;
        curRoom = null;
        animator.SetTrigger("ReStart");
        transform.position = waitPos;
        heroSr.color = Color.white;

        #endregion

        StopAllCoroutines();
        StartCoroutine(heroMoveAlgorithm());
        StartCoroutine(RoomCheck());
        StartCoroutine(roomMonsterCheck());
    }

    #region Hero Move

    #region MoveToward Functions
    private void MoveToEntrance()
    {
        transform.position = Vector3.MoveTowards(transform.position, entranceTr.position + Vector3.up * offset.y, moveSpeed * Time.deltaTime);
    }
    private void MoveToDoor(int _floor)
    {
        transform.position = Vector3.MoveTowards(transform.position, doors[_floor].transform.position + Vector3.up * -0.4f, moveSpeed * Time.deltaTime);
    }
    private void MoveToRoomPos(Vector3 _roomPos)
    {
        transform.position = Vector3.MoveTowards(transform.position, _roomPos + new Vector3(-1f, -0.7f, 0f), moveSpeed * Time.deltaTime);
    }
    #endregion

    private IEnumerator heroMoveAlgorithm()
    {
        yield return new WaitUntil(() => moveStart);
        
        transform.position = spawnPos;
        //처음 소환 후 입구까지 이동
        while (transform && transform.position != entranceTr.position + Vector3.up * offset.y)
        {
            yield return new WaitUntil(() => !isAttack);
            MoveToEntrance();
            yield return new WaitForSeconds(0.01f);
        }
        //이동이 끝나면 다음줄로 소환
        transform.position = ResetPosition();

        //각 줄마다 있는 door로 이동이 끝날 때마다 다음 줄로 이동
        for (int i = 0; i < doors.Length; ++i)
        {
            while (transform.position != doors[i].transform.position + Vector3.up * -0.4f)
            {
                if(!moveStart)
                {
                    break;
                }
                if (curRoom)
                {
                    if (curRoom && curRoom.GetComponentsInChildren<Monster>().Length == 0)
                        animator.SetBool("Idle", false);
                    while (curRoom && curRoom.GetComponentsInChildren<Monster>().Length != 0 && !roomLock)
                    {
                        isMove = false;
                        animator.SetBool("Idle", true);
                        MoveToRoomPos(curRoom.transform.position);
                        yield return new WaitForSeconds(0.02f);
                    }
                }
                yield return new WaitUntil(() => !isAttack);
                isMove = true;
                animator.SetBool("Idle", false);
                MoveToDoor(i);
                yield return new WaitForSeconds(0.01f);
            }
            //이번 줄의 door에 도착했으므로 다음 줄로 이동
            transform.position = ResetPosition();
        }
        if (!isDie)
        {
            transform.position = bossRoomTr.position + new Vector3(-2f, -1.2f + offset.y, 0f);
            while(transform.position != rooms[rooms.Length-1].transform.position + new Vector3(-1f, -1.55f, 0f))
            {
                animator.SetBool("Idle", true);
                transform.position = Vector3.MoveTowards(transform.position, bossRoomTr.position + new Vector3(-1f, -1.55f, 0f), moveSpeed * Time.deltaTime);
                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitUntil(() => !isAttack);
            isMove = false;
        }
        else
        {
            transform.position = waitPos;
        }
    }

    #endregion

    //현재 있는 방을 해당 히어로 위치에 raycast를 쏴서 판별함
    private IEnumerator RoomCheck()
    {
        yield return new WaitUntil(() => transform.position.x >= 0);
        while (true)
        {
            ray.direction = Vector3.forward;
            ray.origin = transform.position + Vector3.back * 100f;
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.CompareTag("Room") || raycastHit.collider.CompareTag("BossRoom"))
                {
                    curRoom = raycastHit.collider.gameObject;
                    this.transform.parent = curRoom.transform;
                }
                else
                {
                    curRoom = null;
                }
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    //위에서 방을 찾았다면 해당 방에서 몬스터가 얼마나 있는지 판별함(몬스터는 방의 자식으로 존재)
    private IEnumerator roomMonsterCheck()
    {
        yield return new WaitUntil(() => curRoom != null);
        while (true)
        {
            if (curRoom && curRoom.GetComponentsInChildren<Monster>().Length != 0 && !isMove && !roomLock)
            {
                isAttack = true;
                animator.SetTrigger("Attack");
                float attackClipLength = animator.runtimeAnimatorController.animationClips[0].length;
                yield return new WaitForSeconds(attackClipLength * 0.5f);
                if(!isDie)
                    Attack();
                yield return new WaitForSeconds(attackClipLength * 0.5f);
                isAttack = false;
            }
            else
            {
                isAttack = false;
                animator.SetBool("Idle", true);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    //히어로가 한줄을 모두 이동했을때 다시 소환될 위치 계산
    private Vector3 ResetPosition()
    {
        return new Vector3(-6f, transform.position.y - height, 0f);
    }


    #region values

    protected Animator animator = null;
    protected Rigidbody2D heroRb = null;
    protected SpriteRenderer heroSr = null;

    [SerializeField] protected float hp = 0f;
    [SerializeField] protected float maxHp = 0f;
    [SerializeField] protected float damage = 0f;
                        public float defense = 0f;

    protected Transform pos;
    private Transform entranceTr = null;
    private Transform bossRoomTr = null;

    private GameObject[] doors = null;
    private GameObject[] rooms = null;
    public GameObject curRoom = null;
    private float height = 0f;
    private float moveSpeed = 5f;
    public int attributes = 0;

    private GameManager gameManager = null;
    private SpawnManager spawnManager = null;
    private UIManager uiManager = null;

    private Ray ray = new Ray();
    private RaycastHit raycastHit = new RaycastHit();

    public bool moveStart = false;
    public bool isMove = true;
    public bool isAttack = false;
    public bool roomLock = false;
    public bool isDie = false;
    private bool isHurtColor = false;

    private Vector3 waitPos = new Vector3(100f, 0f, 0f);
    private Vector3 offset = Vector3.zero;
    private Vector3 spawnPos = new Vector3(-6f, 1.4f, 0f);

    private Color tempColor = Color.white;
    #endregion

}
