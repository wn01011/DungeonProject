using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Hero : MonoBehaviour
{

    protected virtual void Start()
    {
        #region Initialization
        GameObject[] Doors = GameObject.FindGameObjectsWithTag("Door");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        entranceTr = GameObject.FindGameObjectWithTag("Entrance").GetComponent<Transform>();
        doors = GameObject.FindGameObjectsWithTag("Door");
        rooms = GameObject.FindGameObjectsWithTag("Room");
        bossRoomTr = rooms[rooms.Length-1].transform;
        knightBehaviour = GetComponent<KnightBehaviour>();
        height = rooms[0].GetComponent<Collider>().bounds.size.y;
        offset = new Vector3(-6f, -0.4f, 0f);

        transform.position = entranceTr.position + offset;

        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 10;
        #endregion

        StartCoroutine(heroMoveAlgorithm());
        StartCoroutine(RoomCheck());
        StartCoroutine(roomMonsterCheck());
    }
    private void FixedUpdate()
    {
    }

    abstract protected void Attack();


    abstract protected void Active_Skill();


    abstract protected void Passive_Skill();


    protected void Hurt()
    {

    }

    protected void Dead()
    {

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
        while (transform && transform.position != entranceTr.position + Vector3.up * offset.y)
        {
            yield return new WaitUntil(() => !knightBehaviour.isAttack);
            MoveToEntrance();
            yield return new WaitForSeconds(0.01f);
        }
        transform.position = ResetPosition();

        for (int i = 0; i < doors.Length; ++i)
        {
            while (transform.position != doors[i].transform.position + Vector3.up * -0.4f)
            {
                if (knightBehaviour.curRoom)
                {
                    if (knightBehaviour.curRoom && knightBehaviour.curRoom.GetComponentsInChildren<Monster>().Length == 0)
                        animator.SetBool("Idle", false);
                    while (knightBehaviour.curRoom.GetComponentsInChildren<Monster>().Length != 0)
                    {
                        isMove = false;
                        animator.SetBool("Idle", true);
                        MoveToRoomPos(knightBehaviour.curRoom.transform.position);
                        yield return new WaitForSeconds(0.01f);
                    }
                }
                yield return new WaitUntil(() => !isAttack);
                isMove = true;
                
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
            if (curRoom.GetComponentsInChildren<Monster>().Length != 0 && !isMove)
            {
                isAttack = true;
                animator.SetTrigger("Attack");
                float attackClipLength = animator.runtimeAnimatorController.animationClips[0].length;
                yield return new WaitForSeconds(attackClipLength * 0.5f);
                Attack();
                Debug.Log("Attack!");
                yield return new WaitForSeconds(attackClipLength * 0.5f);
                isAttack = false;
            }
            else
            {
                animator.SetBool("IdleBlock", true);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    protected Animator animator;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;

    [SerializeField] protected int hp;
    [SerializeField] protected int attributes;
    [SerializeField] protected float attack;

    protected Transform pos;

    private Transform entranceTr = null;
    private GameObject[] doors = null;
    private GameObject[] rooms = null;
    private Transform bossRoomTr = null;
    private float height = 0f;
    private float moveSpeed = 5f;
    private Vector3 offset = Vector3.zero;
    private KnightBehaviour knightBehaviour = null;

    [SerializeField] protected bool isMove = true;

    private GameManager gameManager = null;
    public GameObject curRoom = null;
    private Ray ray = new Ray();
    private RaycastHit raycastHit = new RaycastHit();
    public bool isAttack = false;
}
