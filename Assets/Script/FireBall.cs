using FMOD;
using UnityEngine;
using Debug = FMOD.Debug;

public class FireBall : MonoBehaviour
{
    public Vector3 target;
    public float elapsedTime;

    public int dmg;
    
    public float speed = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Heroes/Hero_Pyro_Firebolt");
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3.MoveTowards(transform.position, target, speed);
        var direction = target - transform.position;
        
        transform.Translate(speed * direction);
        
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 15)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.OnHit(dmg);
            Destroy(this.gameObject);
        }
    }
}
