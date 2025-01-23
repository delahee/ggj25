using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virtue : Enemy
{
    List<Enemy> toDebuffOnDeath = new();     // For debuffing on death

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (!toDebuffOnDeath.Contains(enemy))
                toDebuffOnDeath.Add(enemy);
            enemy.BuffSpeed(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (toDebuffOnDeath.Contains(enemy))
                toDebuffOnDeath.Remove(enemy);
            enemy.DebuffSpeed(this);
        }
    }

    [ContextMenu("DIIIIE")]
    protected override void OnDie()
    {
        base.OnDie();
        foreach (Enemy enemy in toDebuffOnDeath)
        {
            enemy.DebuffSpeed(this);
        }

    }

}
