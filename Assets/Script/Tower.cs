using System;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float radius = 10f;
    public float fireCooldown = 1f;



    public GameObject fireBall;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            Fire(enemy.transform.position);
        }
    }

    public void Fire(Vector3 target)
    {
        var fireball = Instantiate(fireBall);
        fireball.GetComponent<FireBall>().target = target;
    }
    
}
