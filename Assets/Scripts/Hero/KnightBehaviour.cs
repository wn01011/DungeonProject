using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KnightBehaviour : Hero
{
    private int attackCount = 0;
    [SerializeField] private Image skillCooldown = null;
    protected override void Start()
    {
        base.Start();
        hp = 10f;
        maxHp = hp;
        damage = 2f;
        defense = 1f;
    }

    protected override void Attack()
    {
        if(curRoom && curRoom.GetComponentInChildren<Monster>())
        {
            curRoom.GetComponentInChildren<Monster>().Hurt(damage);
            attackCount++;
            skillCooldown.fillAmount = attackCount * 0.25f;
            SoundManager.soundManager.SFXplayer("Slash_2", clip);
            Passive_Skill();
        }
    }

    protected override void Active_Skill()
    {
        
    }

    protected override void Passive_Skill()
    {
        //4번째 공격마다 더블어택!
        if(attackCount %4 == 0 && curRoom.GetComponentInChildren<Monster>())
        {
            attackCount = 0;
            curRoom.GetComponentInChildren<Monster>().Hurt(damage);
        }
    }
}
