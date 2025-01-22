using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;



public class Enemy : MonoBehaviour, IHit
{
    public Enemies data;
    [Space]
    public int speed = 3;
    public int hp;

    public float atkMaxDuration = 2.0f;
    public float atkT = 0f;


    bool dead = false;

    public void Init(Enemies data)
    {
        this.data = data;
        hp = data.hp;
    }

    private void Awake()
    {
        Init(data);
    }

    protected virtual void Update()
    {
        GateOfHell gate = GateOfHell.instance;
        if (gate == null) return;

        if (Vector3.Distance(transform.position, gate.transform.position) > 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, gate.transform.position, speed * Time.deltaTime);
        }
        else
        {
            AttackSequence();
        }
    }


    void AttackSequence()
    {
        atkT -= Time.deltaTime;
        if (atkT < 0.0f)
            Attack();
    }

    void Attack()
    {
        atkT = Random.Range(0f, atkMaxDuration);
        GateOfHell.instance?.OnHit(data.dmg);
    }

    public void OnHit(int dmg)
    {
        if (dead) return;
        hp-=dmg;
        if (hp < 0)
        {
            OnDie();
        }
    }

    void OnDie()
    {
        dead = true;
        Destroy(gameObject);
    }

}
