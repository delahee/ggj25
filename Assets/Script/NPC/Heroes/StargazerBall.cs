using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StargazerBall : MonoBehaviour
{
    public Transform target;
    public float speed = 3.0f;
    public float exploRadius = 3.0f;
    public int dmg;
    public bool dead;
    [Space]
    public SpriteRenderer explo;


    private void Start()
    {
        explo.gameObject.SetActive(false);
        explo.transform.localScale = Vector3.one * exploRadius;
    }

    private void Update()
    {
        if (dead)
        {
            // explo.transform.localScale *= 1 - Time.deltaTime * 10;

            Color c = explo.color;
            c.a *= 1 - Time.deltaTime * 10;
            explo.color = c;
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.position) < .1f)
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
            Enemy e = hit.GetComponent<Enemy>();
            if (e != null)
            {
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
        var e = other.GetComponent<Enemy>();
        if (e != null)
        {
            e.OnHit(1);
        }
    }
}
