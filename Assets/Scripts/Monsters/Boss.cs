using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Monster
{
    public AudioClip skill_1 = null;
    public AudioClip skill_2 = null;
    public static float static_maxHp = 50f;
    public static float staticDmg = 2f;
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
        cooltime = Random.Range(0, 4);
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
        if (target && cooltime ==0)
        {
            target.Hurt(damage*2f,target.defense);
            StartCoroutine(BossSkill_1());
        }
        else if (target && cooltime == 1)
        {
            target.Hurt(damage*3f, target.defense);
            StartCoroutine(BossSkill_2());
        }
        else
        {
            target.Hurt(damage, target.defense);
            SoundManager.soundManager.SFXplayer("BossAtk", clip);
        }
    }
    private IEnumerator BossSkill_1()
    {
        // 공격시 확률적으로 토네이도 생성
        Hero target = curRoom.GetComponentInChildren<Hero>();
        animator.SetTrigger("Skill");
        GameObject bossskill_1 = Instantiate(Tornado, target.transform.position + (Vector3.up * 0.65f), Quaternion.identity);
        SoundManager.soundManager.SFXplayer("Tornado", skill_1);
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        Destroy(bossskill_1);
     }

    private IEnumerator BossSkill_2()
    {
        // 공격시 확률적으로 다크라이트닝 생성
        Hero target = curRoom.GetComponentInChildren<Hero>();
        animator.SetTrigger("Skill");
        GameObject bossskill_2 = Instantiate(DarkLightning, target.transform.position + (Vector3.up * 0.65f), Quaternion.identity);
        SoundManager.soundManager.SFXplayer("DarkLightning", skill_2);
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        Destroy(bossskill_2);
    }
    protected override void Active_Skill()
    {

    }
    protected override void Passive_Skill()
    {
        
    }

}