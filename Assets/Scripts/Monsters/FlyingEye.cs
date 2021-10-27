using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : Monster
{
    public AudioClip skill = null;
    public static float static_maxHp = 20f;
    public static float staticDmg = 10f;
    public static float staticDef = 0f;
    private int evasion_count = 0;

    protected override void Start()
    {
        base.Start();
        hp = 20f;
        maxHp = hp;
        damage = 10f;
        defense = 0f;
    }
    protected void Update()
    {
        evasion_count = Random.Range(0, 3);
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
            SoundManager.soundManager.SFXplayer("FlyingEyeAtk", clip);
            target.Hurt(damage, target.defense);
    }

    protected override void Active_Skill()
    {

    }

    protected override void Passive_Skill()
    {        
        // 33%확률로 공격회피
        Hero target = curRoom.GetComponentInChildren<Hero>();
        SoundManager.soundManager.SFXplayer("Evasion", skill);
        Hurt(damage * 0);
    }
}
