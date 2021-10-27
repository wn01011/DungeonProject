using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Monster
{
    public static float static_maxHp = 50f;
    public static float staticDmg = 5f;
    public static float staticDef = 0f;
    public bool bossDie = false;

    private int cooltime = 0;

    [SerializeField] private GameObject Tornado = null;
    [SerializeField] private GameObject DarkLightning = null;


    protected override void Start()
    {
        base.Start();
        hp = 50f;
        maxHp = hp;
        damage = 2f;
        defense = 0f;
    }

    protected void Update()
    {
        cooltime = Random.Range(0, 3);
        maxHp = static_maxHp;
        damage = staticDmg;
        defense = staticDef;
    }

    protected override void Dead()
    {
        base.Dead();
        bossDie = true;
    }

    protected override void Attack()
    {

        Hero target = curRoom.GetComponentInChildren<Hero>();
        if (target)
        {
            Debug.Log(cooltime);
            target.Hurt(damage,target.defense);
            Active_Skill();
        }
    }
    public void SetStaticHp()
    {
        maxHp = static_maxHp;
        hp = static_maxHp;
    }
    private IEnumerator Boss_SKill()
    {
        Hero target = curRoom.GetComponentInChildren<Hero>();
        // 공격시 확률적으로 토네이도,다크라이트닝 생성
        if (cooltime == 0)
        {
            animator.SetTrigger("Skill");
            GameObject bossskill_1 = Instantiate(Tornado, target.transform.position + (Vector3.up * 0.65f), Quaternion.identity);
            yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
            Destroy(bossskill_1);
        }
        else if (cooltime == 1)
        {
            animator.SetTrigger("Skill");
            GameObject bossskill_2 = Instantiate(DarkLightning, target.transform.position + (Vector3.up * 0.65f), Quaternion.identity);
            yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
            Destroy(bossskill_2);
        }
    }
    protected override void Active_Skill()
    {
        StartCoroutine(Boss_SKill());
    }
    protected override void Passive_Skill()
    {
        
    }


}