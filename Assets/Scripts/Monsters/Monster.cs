using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Monster : MonoBehaviour
{
    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        monsterRb = GetComponentInChildren<Rigidbody2D>();
        monsterSr = GetComponentInChildren<SpriteRenderer>();
        tempColor = monsterSr.color;
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

    public virtual void Hurt(float _damage)
    {
        if (hp - _damage <= 0)
        {
            hp = 0;
            Dead();
        }
        else
        {
            if (!isHurtColor)
                StartCoroutine(HurtAlphaChange());
            if (isHurtColor)
            {
                StopCoroutine(HurtAlphaChange());
                monsterSr.color = tempColor;
                StartCoroutine(HurtAlphaChange());
            }
            hp -= _damage;
        }
    }
    IEnumerator HurtAlphaChange()
    {
        isHurtColor = true;
        int negativeSwitch = 1;
        for (int i = 0; i < 3f; ++i)
        {
            if (negativeSwitch == 1)
            {
                // ¹Ù²ð Color°ª
                monsterSr.color = Color.red;
            }
            else
            {
                monsterSr.color = tempColor;
            }
            negativeSwitch *= -1;
            yield return new WaitForSeconds(0.1f);
        }
        monsterSr.color = tempColor;
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
            if(!curRoom.GetComponentInChildren<Hero>())
            {
                yield return new WaitForEndOfFrame();
                continue;
            }
            else if (curRoom.GetComponentsInChildren<Hero>().Length != 0 && !isDie)
            {
                animator.SetTrigger("Attack");
                float attackClipLength = animator.runtimeAnimatorController.animationClips[0].length;
                yield return new WaitForSeconds(attackClipLength * 0.5f);
                if(!isDie)
                    Attack();
                yield return new WaitForSeconds(attackClipLength * 0.5f);
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public void ReStart()
    {
        StartCoroutine(DieWait());
        StopAllCoroutines();
        SetHp();
        isDie = false;
        animator.SetTrigger("ReStart");
        monsterSr.color = Color.white;
        StartCoroutine(RoomCheck());
        StartCoroutine(RoomHeroCheck());
    }

    protected Animator animator;
    protected Rigidbody2D monsterRb;
    protected SpriteRenderer monsterSr;

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
    private bool isHurtColor = false;

    private Color tempColor = Color.white;
}

