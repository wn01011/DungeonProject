using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Monster
{
    public static float static_maxHp = 30f;
    public static float staticDmg = 3f;
    public static float staticDef = 2f;
    private int critical_hit = 0;

    protected override void Start()
    {
        base.Start();
        hp = 30f;
        maxHp = hp;
        damage = 3f;
        defense = 2f;
    }
    protected void Update()
    {
        critical_hit = Random.Range(0, 4);
        maxHp = static_maxHp;
        damage = staticDmg;
        defense = staticDef;
    }
    protected override void Attack()
    {
        Hero target = curRoom.GetComponentInChildren<Hero>();
        if (target)
        {
            target.Hurt(damage,target.defense);
            Passive_Skill();
        }
    }
    public void SetStaticHp()
    {
        maxHp = static_maxHp;
        hp = static_maxHp;
    }
    protected override void Active_Skill()
    {

    }

    protected override void Passive_Skill()
    {
        Hero target = curRoom.GetComponentInChildren<Hero>();
        //공격시 20% 확률로 데미지2배
        if (critical_hit == 0)
        {
            animator.SetTrigger("Skill");
            target.Hurt(damage * 2, target.defense);
        }
    }
}
