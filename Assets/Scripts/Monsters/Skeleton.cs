using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Monster
{
    private float poisonDmg = 0.2f;
    protected override void Start()
    {
        base.Start();
        hp = 10f;
        maxHp = hp;
        damage = 1f;
    }
    protected override void Attack()
    {
        Hero target = curRoom.GetComponentInChildren<Hero>();
        if (target)
        {
            target.Hurt(damage);
            Passive_Skill();
        }
    }
    protected override void Active_Skill()
    {
        throw new System.NotImplementedException();
    }

    protected override void Passive_Skill()
    {
        //µ¶ ÁlÄû±â
        Hero target = curRoom.GetComponentInChildren<Hero>();
        if (target)
            StartCoroutine(poisonAttack(target));
    }

    private IEnumerator poisonAttack(Hero _targetHero)
    {
        float duration = 20f;
        float poisonTickTime = 1.0f;
        while(!_targetHero.isDie || duration <= 0f)
        {
            duration -= poisonTickTime;
            yield return new WaitForSeconds(poisonTickTime);
            _targetHero.Hurt(0.2f);
        }
    }
}
