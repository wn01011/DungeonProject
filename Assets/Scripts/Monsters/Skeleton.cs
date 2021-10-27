using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Monster
{
    public static float static_maxHp = 10f;
    public static float staticDmg = 1f;
    public static float staticDef = 0f;
    private float poisonDmg = 0.2f;

    protected override void Start()
    {
        base.Start();
        hp = 10f;
        maxHp = hp;
        damage = 1f;
        defense = 0f;
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
            target.Hurt(damage,target.defense);
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
    public void SetStaticHp()
    {
        maxHp = static_maxHp;
        hp = static_maxHp;
    }
    private IEnumerator poisonAttack(Hero _targetHero)
    {
        float duration = 10f;
        float poisonTickTime = 1.0f;
        while(!_targetHero.isDie || duration <= 0f)
        {
            duration -= poisonTickTime;
            yield return new WaitForSeconds(poisonTickTime);
            _targetHero.Hurt(poisonDmg,0);
        }
    }
}
