using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiwiPizza : Enemy
{
    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
            enemy.BuffHP(this);
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
            enemy.DebuffHP(this);
    }

    protected override void OnDie()
    {
        base.OnDie();

    }
}
