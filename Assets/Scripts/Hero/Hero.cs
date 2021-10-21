using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Hero : MonoBehaviour
{

    protected virtual void Start()
    {
        #region Initialization

        animator = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();

        entranceTr = GameObject.FindGameObjectWithTag("Entrance").GetComponent<Transform>();
        doors = GameObject.FindGameObjectsWithTag("Door");
        rooms = GameObject.FindGameObjectsWithTag("Room");
        spawnManager = FindObjectOfType<SpawnManager>();
        gameManager = FindObjectOfType<GameManager>();
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

    public void Hurt(float _damage)
    {
        if (hp - _damage <= 0)
        {
            hp = 0;
            Dead();
        }
        else 
        {
            animator.SetTrigger("Hurt");
            hp -= _damage;       
        }
    }

    protected void Dead()
    {
        gameManager.goods.bone += 10;
        isMove = false;
        transform.position = waitPos;
        transform.parent = spawnManager.transform;
        
        animator.SetTrigger("Die");
        StartCoroutine(DieWait());
    }
    protected IEnumerator DieWait()
    {
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        isDie = true;
        moveStart = false;
        isAttack = false;
        roomLock = false;
        curRoom = null;
    }
    public void ReStart()
    {
        maxHp = 10 + GameManager.wave * 5;
        damage = GameManager.wave;
        SetHp();
        isDie = false;
        moveStart = false;
        isMove = true;
        isAttack = false;
        roomLock = false;
        curRoom = null;
        animator.SetTrigger("ReStart");
        transform.position = waitPos;
        StopAllCoroutines();
        StartCoroutine(heroMoveAlgorithm());
        StartCoroutine(RoomCheck());
        StartCoroutine(roomMonsterCheck());
    }
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
    private Vector3 ResetPosition()
    {
        return new Vector3(-6f, transform.position.y - height, 0f);
    }
    private IEnumerator heroMoveAlgorithm()
    {
        yield return new WaitUntil(() => moveStart);
        transform.position = spawnPos;
        while (transform && transform.position != entranceTr.position + Vector3.up * offset.y)
        {
            yield return new WaitUntil(() => !isAttack);
            MoveToEntrance();
            yield return new WaitForSeconds(0.01f);
        }
        transform.position = ResetPosition();

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
            transform.position = ResetPosition();
        }
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

    protected Animator animator = null;
    protected Rigidbody2D rb = null;
    protected SpriteRenderer sr = null;

    [SerializeField] protected float hp = 0f;
    [SerializeField] protected float maxHp = 0f;
    [SerializeField] protected float damage = 0f;

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

    private Ray ray = new Ray();
    private RaycastHit raycastHit = new RaycastHit();

    public bool moveStart = false;
    public bool isMove = true;
    public bool isAttack = false;
    public bool roomLock = false;
    public bool isDie = false;
    private Vector3 waitPos = new Vector3(100f, 0f, 0f);
    private Vector3 offset = Vector3.zero;
    private Vector3 spawnPos = new Vector3(-6f, 1.4f, 0f);
}
