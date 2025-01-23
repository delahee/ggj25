using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angel : Enemy
{
    [Space]
    public EyeProj prefab;
    public float ShootInterval = .8f;
    public float t = .8f;

    protected override void Update()
    {
        base.Update();
        t -= Time.deltaTime;
        if ( t <= 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        t = ShootInterval;

        if (door != null)
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
