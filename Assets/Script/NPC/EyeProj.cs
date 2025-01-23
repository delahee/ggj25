using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeProj : MonoBehaviour
{
    public Angel owner;
    public Vector3 targetPos;
    public float speed = 3.0f;
    public float exploRadius = 3.0f;
    public int dmg;
    public bool dead;
    public bool hitEnemies;
    [Space]
    public SpriteRenderer explo;

    private void Start()
    {
        explo.gameObject.SetActive(false);
        
    }

    private void OnValidate()
    {
        explo.transform.localScale = Vector3.one * exploRadius;
    }

    private void Update()
    {
        if (dead)
        {
            explo.transform.localScale *= 1 - Time.deltaTime*10 ;
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPos) < .1f)
        {
            Destroy();
        }
    }

    void Destroy()
    {
        dead = true;
        explo.gameObject.SetActive(true);
        var hits = Physics.OverlapSphere(transform.position, exploRadius);
        foreach (var hit in hits)
        {
            Hero h = hit.GetComponent<Hero>();
            if (!hitEnemies) continue;
            Enemy e = hit.GetComponent<Enemy>();
            if (e != null)
            {
                if (e != owner)
                    e.OnHit(dmg);
            }
        }

        Destroy(gameObject, .10f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, exploRadius);   
    }

    private void OnTriggerEnter(Collider other)
    {
        var h = other.GetComponent<Hero>();
        var e = other.GetComponent<Enemy>();
        if (h != null)
        {
            // APply dmg
        }
        if (e != null)
        {
            if (e != owner)
                e.OnHit(1);
        }
    }
}
