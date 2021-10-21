using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Monster : MonoBehaviour
{
    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
        
        StartCoroutine(RoomCheck());
        StartCoroutine(RoomHeroCheck());
    }

    protected virtual void Attack()
    {
        Hero target = curRoom.GetComponentInChildren<Hero>();
        if (target)
        {
            target.Hurt(damage);
        }
    }
    abstract protected void Active_Skill();
    abstract protected void Passive_Skill();

    public void SetHp() => hp = maxHp;
    public float GetHp()
    {
        return hp;
    }
    
    public float GetMaxHp()
    {
        return maxHp;
    }

    public virtual void Hurt(float _dmg)
    {
        if(hp - _dmg <= 0)
        {
            hp = 0f;
            Dead();
        }
        else
        {
            animator.SetTrigger("Hurt");
            hp -= _dmg;
        }
    }
    protected virtual void Dead()
    {
        isDie = true;
        animator.SetTrigger("Die");
        transform.parent = spawnManager.transform;
        StartCoroutine(DieWait());
    }
    private IEnumerator DieWait()
    {
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        transform.position = waitPos;
    }
    private IEnumerator RoomCheck()
    {
        
        while (true)
        {
            ray.direction = Vector3.forward;
            ray.origin = transform.position + Vector3.back * 100f;
            if (Physics.Raycast(ray, out raycastHit) && !isDie)
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
    protected virtual IEnumerator RoomHeroCheck()
    {
        yield return new WaitUntil(() => curRoom != null);
        while (true)
        {
            if (curRoom.GetComponentsInChildren<Hero>().Length != 0 && !isDie)
            {
                animator.SetTrigger("Attack");
                float attackClipLength = animator.runtimeAnimatorController.animationClips[0].length;
                yield return new WaitForSeconds(attackClipLength * 0.5f);
                if(!isDie)
                    Attack();
                yield return new WaitForSeconds(attackClipLength * 0.5f);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void ReStart()
    {
        StartCoroutine(DieWait());
        StopAllCoroutines();
        SetHp();
        isDie = false;
        animator.SetTrigger("ReStart");
        StartCoroutine(RoomCheck());
        StartCoroutine(RoomHeroCheck());
    }

    protected Animator animator;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;

    [SerializeField] protected float hp =0f;
    protected float maxHp = 0f;
    [SerializeField] protected int attributes = 0;
    [SerializeField] protected float damage =0f;
    [SerializeField] protected SpawnManager spawnManager = null;
    protected Transform pos;

    private Vector3 waitPos = Vector3.right * 100f;

    public GameObject curRoom = null;
    private Ray ray = new Ray();
    private RaycastHit raycastHit = new RaycastHit();

    public bool isDie = false;
}

