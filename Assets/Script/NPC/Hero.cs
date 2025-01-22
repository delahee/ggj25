using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{

    public float speed = 3.0f;
    float pushForce = 1.0f;
    float maxFOV = float.MaxValue;

    public Enemy enemy;
    public int hp = 10;

    public float atkMaxDuration = 2.0f;
    public float atkT = 0f;

    private void Update()
    {

        if (enemy == null)
        {
            SeekEnemy();
        }

        if (enemy == null) {

            GateOfHell door = FindObjectOfType<GateOfHell>();
            if (door == null) return;

            if (Vector3.Distance(transform.position, door.transform.position) > 10) { 
                transform.position = Vector3.MoveTowards(transform.position, door.transform.position, speed * Time.deltaTime);
            }

            return;
        }

        float dist = Vector3.Distance(transform.position, enemy.transform.position);
        if (dist > 1)
        {
            Vector3 move = Vector3.MoveTowards(transform.position, enemy.transform.position, speed * Time.deltaTime);
            transform.position = move;
        }
        else
        {
            AttackSequence();
        }
    }

    void SeekEnemy()
    {
        float minDist = maxFOV;
        foreach (Enemy e in FindObjectsOfType<Enemy>())
        {
            var dist = Vector3.Distance(e.transform.position, transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                enemy = e;
            }
        }
    }

    void AttackSequence()
    {
        atkT -= Time.deltaTime;
        if (atkT < 0.0f)
            Attack(enemy);
    }

    void Attack(Enemy o)
    {
        Vector3 dir = o.transform.position - transform.position;
        dir.y = 0;
        o.transform.position += dir.normalized * pushForce;
        o.OnHit();

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
