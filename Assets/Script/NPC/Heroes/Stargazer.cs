using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using static UnityEngine.UI.GridLayoutGroup;

public class Stargazer : Hero
{
    [Header("Stargazer")]
    public float reloadTime = 1.0f;
    public float reloadT;

    public float safeSpace = 6.0f;
    public StargazerBall prefab;


    Enemy closest;

    private void Start()
    {
        reloadT = reloadTime;
    }

    protected override void Update()
    {

        Vector3 steering = new Vector3();
        bool safe = true;
        foreach (Enemy enemy in FindObjectsOfType<Enemy>()) {
            if (enemy.dead) continue;
            Vector3 dir = transform.position - enemy.transform.position;
            if (dir.magnitude > safeSpace) continue;
            if (dir.x == 0) continue;
            if (dir.y == 0) continue;
            dir = dir.normalized * (safeSpace - dir.magnitude);
            steering += dir;
            safe = false;
        }
        Vector3 toParent = transform.parent.position - transform.position;
        steering += toParent * .3f;
        steering.y = 0;
        if (safe)
            PatrolSequence();
        else
        {
            transform.position += steering.normalized * speed * Time.deltaTime;
        }

        reloadT -= Time.deltaTime;
        if (reloadT < 0)
        {
            reloadT = reloadTime;
            Shoot();
        }
    }

    void Shoot()
    {
        if (!targetEnemy || targetEnemy.dead) {
            SeekEnemy();
            return;
        }
        var ball = Instantiate(prefab, transform.position, transform.rotation);
        ball.dmg = data.AtkDmgBasis;
        ball.targetPos = targetEnemy.transform.position;
    }
}
