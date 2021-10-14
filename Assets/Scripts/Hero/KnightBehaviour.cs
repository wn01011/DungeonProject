using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightBehaviour : Hero
{
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        curRoom.GetComponentInChildren<Monster>().Hurt();
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
