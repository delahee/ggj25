using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chocodemon : Enemy
{
    [Space]
    [Range(0.0f, 1.0f)] 
    public float subMobChance = .1f;
    public GameObject prefab;

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
            var c = Random.insideUnitCircle * 3;
            Instantiate(prefab).transform.position = transform.position + new Vector3(c.x, 0, c.y);

    }
}
