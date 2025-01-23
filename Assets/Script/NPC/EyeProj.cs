using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeProj : MonoBehaviour
{
    public Vector3 targetPos;
    public float speed = 3.0f;
    public float radius = 3.0f;
    public int dmg;
    public bool dead;
    public bool hitEnemies;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (dead) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPos) < .1f)
        {
            var hits = Physics.OverlapSphere(transform.position, radius);
            foreach (var hit in hits) {
                Hero h = hit.GetComponent<Hero>();
                if (!hitEnemies) continue;
                Enemy e = hit.GetComponent<Enemy>();
                if (e != null)
                {
                    e.OnHit(dmg);
                }
                Destroy();
            }
        }
    }

    void Destroy()
    {
        dead = true;
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);   
    }
}
