using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angel : Enemy
{
    [Space]
    [Header("Angel")]
    public EyeProj prefab;
    public float ShootInterval = .8f;
    public float t = .8f;
    public Vector3 targetPos;

    private void Start()
    {
        FindTarget();
        if (door == null) door = GateOfHell.instance;
        targetPos = Vector3.MoveTowards(door.transform.position, transform.position, door.radius);
    }

    protected override void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        t -= Time.deltaTime;
        if ( t <= 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        t = ShootInterval;

        if (target != null && !target.dead)
        {
            if (target.dead)
                FindTarget();
            else
            {
                EyeProj proj = Instantiate(prefab, transform.position, transform.rotation);
                proj.targetPos = target.transform.position;
                proj.dmg = dmg;
                proj.owner = this;
                return;
            }
        }


        else if (door != null)
        {
            EyeProj proj = Instantiate(prefab, transform.position, transform.rotation);
            proj.targetPos = door.transform.position;
            proj.dmg = dmg;
            proj.owner = this;
        }

        else
            door = GateOfHell.instance;
    }
}
