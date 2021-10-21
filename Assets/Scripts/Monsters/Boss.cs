using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Boss : Monster
{
    public bool bossDie = false;
    protected override void Start()
    {
        base.Start();
        hp = 5f;
        maxHp = hp;
        damage = 1f;
    }
    protected override void Dead()
    {
        base.Dead();
        bossDie = true;
    }
    

    protected override void Active_Skill()
    {
        throw new System.NotImplementedException();
    }

    protected override void Passive_Skill()
    {
        throw new System.NotImplementedException();
    }
}

