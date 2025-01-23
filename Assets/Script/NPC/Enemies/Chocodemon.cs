using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chocodemon : Enemy
{
    [Space]
    [Range(0.0f, 1.0f)] 
    public float subMobChance = .3f;
    [Min(1)]
    public int qMax = 3;
    public GameObject ownPrefab;

    public override void OnHit(int dmg)
    {
        base.OnHit(dmg);
        if (!dead)
        {
            if (Random.Range(0.0f, 1.0f) < subMobChance)
                SubMob();
        }
    }

    void SubMob()
    {
        for (int i = 0; i < Random.Range(1, qMax+1); i++)
        {
            var c = Random.insideUnitCircle * 3;
            Instantiate(ownPrefab).transform.position = transform.position + new Vector3(c.x, 0, c.y);
        }
    }
}
