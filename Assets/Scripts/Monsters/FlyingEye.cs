using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : Monster
{
    public static float static_maxHp = 40f;
    public static float staticDmg = 5f;
    public static float staticDef = 2f;
    public int evasion_count = 0;

    protected override void Start()
    {
        base.Start();
        hp = 40f;
        maxHp = hp;
        damage = 5f;
        defense = 2f;
    }
    protected void Update()
    {
        evasion_count = Random.Range(0, 4);
        maxHp = static_maxHp;
        damage = staticDmg;
        defense = staticDef;
    }
    protected override void Attack()
    {
        Hero target = curRoom.GetComponentInChildren<Hero>();
        if (target && evasion_count ==0)
        {
            Passive_Skill();
        }
        else
            target.Hurt(damage, target.defense);
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
        // 20%확률로 공격회피
        target.Hurt(damage * 0, target.defense);
    }
}
