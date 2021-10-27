using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkWizard : Monster
{
    public static float static_maxHp = 50f;
    public static float staticDmg = 6f;
    public static float staticDef = 3f;
    [SerializeField] private FireBall fireBall = null;

    protected override void Start()
    {
        base.Start();
        hp = 50f;
        maxHp = hp;
        damage = 6f;
        defense = 3f;
        fireBall = FindObjectOfType<FireBall>();
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
        //적 공격시 마왕 MP회복
        fireBall.MP_absorb();
    }
}
