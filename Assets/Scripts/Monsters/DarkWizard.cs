using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkWizard : Monster
{
    public static float static_maxHp = 40f;
    public static float staticDmg = 5f;
    public static float staticDef = 1f;

    protected override void Start()
    {
        base.Start();
        hp = 40f;
        maxHp = hp;
        damage = 5f;
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
            SoundManager.soundManager.SFXplayer("DarkWizardAtk", clip);
            Passive_Skill();
        }
    }
    protected override void Active_Skill()
    {

    }

    protected override void Passive_Skill()
    {
        //적 공격시 마왕 MP회복
        FireBall fireball = GetComponent<FireBall>();
        fireball.MP_absorb();
    }
}
