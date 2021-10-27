using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditBehaviour : Hero
{
    protected override void Start()
    {
        base.Start();
        hp = 10f;
        maxHp = hp;
        damage = 2f;
    }
    protected override void Attack()
    {
        if(curRoom && curRoom.GetComponentInChildren<Monster>())
        {
            curRoom.GetComponentInChildren<Monster>().Hurt(damage);
            SoundManager.soundManager.SFXplayer("Slash_2", clip);
            Passive_Skill();
        }
    }
    protected override void Active_Skill()
    {

    }
    protected override void Passive_Skill()
    {
        //Splash Attack; 50% dmg
        foreach(Monster monster in curRoom.GetComponentsInChildren<Monster>())
        {
            monster.Hurt(damage * 0.5f);
        }
    }
}
