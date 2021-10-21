using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringer : Monster
{
    protected override void Start()
    {
        base.Start();
        hp = 10f;
        maxHp = hp;
        damage = 1f;
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
