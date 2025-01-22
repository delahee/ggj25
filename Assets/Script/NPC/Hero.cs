using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{

    public float speed = 3.0f;
    public float pushForce = 1.0f;


    public Heroes data;
    public int hp = 10;
    Enemy targetEnemy;

    public float atkMaxDuration = 2.0f;
    public float atkT = 0f;

    Vector3 patrolPos;
    float patrolT = 0.0f;
    float patrolInterval = 2.0f;


    void Init(Heroes data)
    {
        this.data = data;
        hp = data.hp;
    }

    private void Awake()
    {
        Init(data);
    }

    private void Update()
    {

        if (targetEnemy == null)
        {
            SeekEnemy();
        }

        if (targetEnemy == null) {

            GateOfHell door = FindObjectOfType<GateOfHell>();
            if (door == null) return;

            if (Vector3.Distance(transform.position, door.transform.position) > 10) { 
                transform.position = Vector3.MoveTowards(transform.position, door.transform.position, speed * Time.deltaTime);
            }
            else
            {
                PatrolSequence();
            }

            return;
        }

        float dist = Vector3.Distance(transform.position, targetEnemy.transform.position);
        if (dist > 1)
        {
            Vector3 move = Vector3.MoveTowards(transform.position, targetEnemy.transform.position, speed * Time.deltaTime);
            transform.position = move;
        }
        else
        {
            AttackSequence();
        }
    }

    void RequestPatrolPos()
    {
        var gate = GateOfHell.instance;
        if (gate == null) return;


        if (Vector3.Distance(transform.position, gate.transform.position) > gate.radius / 2)
        {
            Vector3 c = Random.insideUnitCircle * gate.radius / 2;
            patrolPos = gate.transform.position + new Vector3(c.x, 0, c.y);
        }
        else
        {
            Vector3 c = Random.insideUnitCircle;
            patrolPos = transform.position + new Vector3(c.x, 0, c.y);
        }

    }
    void PatrolSequence()
    {
        if (Vector3.Distance(transform.position, patrolPos) > .1f) 
            transform.position = Vector3.MoveTowards(transform.position, patrolPos, speed * Time.deltaTime);
        else 
            patrolT -= Time.deltaTime;
        
        if (patrolT <= 0)
        {
            patrolT = patrolInterval;
            RequestPatrolPos();
        }
    }

    void SeekEnemy()
    {
        float minDist = GateOfHell.instance.radius;
        foreach (Enemy e in FindObjectsOfType<Enemy>())
        {
            var dist = Vector3.Distance(e.transform.position, transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                targetEnemy = e;
            }
        }
    }

    void AttackSequence()
    {
        atkT -= Time.deltaTime;
        if (atkT < 0.0f)
            Attack(targetEnemy);
    }

    void Attack(Enemy o)
    {
        Vector3 dir = o.transform.position - transform.position;
        dir.y = 0;
        o.transform.position += dir.normalized * pushForce;
        o.OnHit(data.AtkDmgBasis);

        atkT = Random.Range(0f, atkMaxDuration);
    }

    void OnHit()
    {
        hp--;
        if (hp < 0)
        {
            OnDie();
        }
    }

    void OnDie()
    {
        Destroy(gameObject);
    }
}
