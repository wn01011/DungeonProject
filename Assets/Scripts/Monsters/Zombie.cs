using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Monster
{
    public static float static_maxHp = 20f;
    public static float staticDmg = 2f;
    public static float staticDef = 1f;

    protected override void Start()
    {
        base.Start();
        hp = 20f;
        maxHp = hp;
        damage = 2f;
        defense = 1f;
    }
    protected void Update()
    {
        maxHp = static_maxHp;
        damage = staticDmg;
        defense = staticDef;
    }
    protected override void Attack()
    {
        Hero target = curRoom.GetComponentInChildren<Hero>();
        if (target)
        {
            target.Hurt(damage, target.defense);
            //Passive_Skill();
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
        //Àç»ý·Â
        if (hp < maxHp)
            hp += Time.deltaTime * 0.2f;
    }
}
