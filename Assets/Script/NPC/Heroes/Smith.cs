using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Smith : Hero
{
    [Header("Smith")]
    public Turret prefab;
    public float timeToCreateATurret = 3.0f;
    Vector3 turretPos;
    public float t = 0.0f;

    void RequestNextTurretPos()
    {
        if (GateOfHell.instance)
        {
            var c = Random.insideUnitCircle;
            turretPos = GateOfHell.instance.transform.position + new Vector3(c.x, 0, Mathf.Abs(c.y) + 2);
            return;
        }

        foreach (var e in FindObjectsOfType<Enemy>()) {
            if (e.dead) continue;
            turretPos = e.transform.position;
            return;
        }
        RequestPatrolPos();
        turretPos = patrolPos;
    }

    private void Update()
    {
        if (t >= 0.0f)
        {
            t -= Time.deltaTime;
            if (t <= 0.0f)
            {
                t = timeToCreateATurret;
                SpawnTurret();
                RequestNextTurretPos();
            }
        }

        if (Vector3.Distance(transform.position, turretPos) > .1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, turretPos, speed * Time.deltaTime);

        }

    }

    void SpawnTurret()
    {
        Turret t = Instantiate(prefab, transform.position, transform.rotation);
        t.dmg = data.AtkDmgBasis;
    }
}
