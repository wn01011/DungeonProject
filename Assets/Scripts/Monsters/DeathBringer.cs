using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringer : Monster
{
    public static float static_maxHp = 30f;
    public static float staticDmg = 8f;
    public static float staticDef = 5f;

    private float cooltime = 0f;

    [SerializeField] private GameObject DeathBlow = null;

    protected override void Start()
    {
        base.Start();
        hp = 30f;
        maxHp = hp;
        damage = 8f;
        defense = 5f;
    }
    protected void Update()
    {
        cooltime += Time.deltaTime * 1f;
        maxHp = static_maxHp;
        damage = staticDmg;
        defense = staticDef;
    }
    protected override void Attack()
    {
        Hero target = curRoom.GetComponentInChildren<Hero>();
        if (target && cooltime > 3f)
        {
            cooltime = 0f;
            Active_Skill();
        }
        else
        {
            target.Hurt(damage, target.defense);
            Passive_Skill();
        }
    }
    private IEnumerator Skill()
    {
        Hero target = curRoom.GetComponentInChildren<Hero>();
        //3초에 한번씩 스킬발동
        animator.SetTrigger("Skill");
        GameObject skill = Instantiate(DeathBlow, target.transform.position + (Vector3.up * 0.9f), Quaternion.identity);
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        Destroy(skill);

    }

    protected override void Active_Skill()
    {
        StartCoroutine(Skill());
    }

    protected override void Passive_Skill()
    {
        //공격시 영혼수확
        gameManager.goods.soulgem += 5;
    }
}
