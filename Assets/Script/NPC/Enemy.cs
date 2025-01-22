using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy : MonoBehaviour
{
    public int speed = 3;

    public GateOfHell door;
    public int hp = 10;

    public float atkMaxDuration = 2.0f;
    public float atkT = 0f;

    private void Start()
    {
        if (door == null)
        {
            FindDoor();
        }
        if (door == null)
        {
            enabled = false;
            Debug.LogWarning("Door not found");
        }
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, door.transform.position) > 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, door.transform.position, speed * Time.deltaTime);
        }
        else
        {
            AttackSequence();
        }
    }

    void FindDoor()
    {
        door = FindObjectOfType<GateOfHell>();
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
        door.OnHit();
    }

    public void OnHit()
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
