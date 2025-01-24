using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Turret : MonoBehaviour, IHit
{
    public GameObject fx;
    public Enemy target;
    public float reloadTime;
    public float reloadT;
    public float lifespan;
    public int hp;
    public int dmg;

    bool dead;

    private void Update()
    {
        lifespan -= Time.deltaTime;
        if (lifespan < 0)
        {
            Destroy();
        }

        if (!target || (target && target.dead))
        {
            FindTarget();
            return;
        }

        Vector3 toEnemy = target.transform.position - transform.position;
        Vector2 dir = new Vector2(toEnemy.x, toEnemy.z);
        transform.rotation = Quaternion.Euler(50, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);

        reloadT -= Time.deltaTime;
        if (reloadT < 0)
        {
            reloadT = reloadTime;
            shoot();
        }
    }

    void FindTarget()
    {

        foreach (var e in FindObjectsOfType<Enemy>())
        {
            if (e.dead) continue;
            target = e;
            return;
        }
    }

    void shoot()
    {
        if (!target) return;
        target.OnHit(dmg);
        Instantiate(fx, target.transform.position, target.transform.rotation);
        
    }

    public void OnHit(int dmg)
    {
        if (dead) return;
        hp -= dmg;
        if (hp < 0)
            Destroy();
    }

    void Destroy()
    {
        dead = true;
        Destroy(gameObject);
    }
}
