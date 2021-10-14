using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;

    [SerializeField] protected int hp;
    [SerializeField] protected int attributes;
    [SerializeField] protected float attack;

    protected Transform pos;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

    }

    protected void Attack()
    {

    }

    protected void Active_Skill()
    {

    }

    protected void Passive_Skill()
    {

    }

    public virtual void Hurt()
    {
        Destroy(gameObject);
    }

    protected void Dead()
    {

    }
}

