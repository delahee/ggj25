using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public enum NPC_State
{
    Idle,
    Walk,
    Atk,
    Hit,
    Dead,
}

public class Enemy : MonoBehaviour, IHit
{
    public GameObject atkFX;

    public AnimationCurve Speedcurve;
    public AnimationCurve Sizecurve;

    public  GateOfHell       door;
    public  Hero             target;
    public  Enemies          data;
    public  ParticleSystem   buffHpFx;
    public  ParticleSystem   buffSpeedFX;
    [Space]
    public  float    speed = 3;
    public  int      hp;
    public  int      dmg;
    public  float      pushForce = 1.0f;

    [SerializeField] 
            float atkMaxDuration = 1.0f;
            float atkT = 0f;

            float hitT;
    [SerializeField] float hitDur = .1f;

    public  bool dead;
            bool hpBuffed;
            bool speedBuffed;


            List<KiwiPizza>   pizzaList   = new();
            List<Virtue>      virtueList  = new();

            SpriteRenderer renderer;
            Animator animator;



    public virtual void Init(Enemies data)
    {
        this.data = data;
        this.speed = data.speed;
        this.dmg = data.dmg;
        hp = data.hp 
            + (int)(Time.timeSinceLevelLoad / 20);  // Increases HP along time

        renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        Init(data);
    }

    private void Start()
    {
        if (door == null) door = GateOfHell.instance;
        Sizecurve.Evaluate(0);
    }
    
    protected virtual void Update()
    {
        if (dead) return;

        if (hitT >= 0)
        {
            hitT -= Time.deltaTime;
            if (hitT < 0)
            {
                renderer.color = Color.white;

            }
        }

        if (door == null) return;
        
        Vector3 pos = door.transform.position;
        pos.y = 0f;
        if (target)        
            pos = target.transform.position;

        float diff = (transform.position.z - door.transform.position.z) / 31.33f;
        if (EnemyManager.INSTANCE != null)
        {
            diff = Mathf.InverseLerp(
                door.transform.position.z, 
                EnemyManager.INSTANCE.transform.position.z, 
                transform.position.z);
        }
        if (Vector3.Distance(transform.position, pos) > 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, data.speed*Sizecurve.Evaluate(diff) * Time.deltaTime);
        }
        else
        {
            AttackSequence();
        }
        
        transform.localScale = new Vector3(Sizecurve.Evaluate(diff), Sizecurve.Evaluate(diff), 1) ;
    }


    void AttackSequence()
    {
        atkT -= Time.deltaTime;
        if (atkT < 0.0f)
            Attack();
    }

    void Attack()
    {
        atkT = Random.Range(0f, atkMaxDuration);
        animator.SetTrigger("Atk");
        if (target && !target.dead)
        {
            Vector3 dir = target.transform.position - transform.position;
            dir.y = 0;
            //target.Push(dir.normalized * pushForce);
            //if (target.transform.position.z < door.transform.position.z) target.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, door.transform.position.z);
            Instantiate(atkFX, target.transform.position, target.transform.rotation);
            target.OnHit(dmg);
        }
        else
        {
            FindTarget();
            door?.OnHit(data.dmg);
        }
     
    }

    protected void FindTarget()
    {
        float minDist = float.MaxValue;
        foreach (var h in FindObjectsOfType<Hero>())
        {
            float dist = Vector2.Distance(transform.position, h.transform.position);
            if (dist < minDist)
            {
                if (h.dead) continue;
                target = h;
                minDist = dist;
            }
        }
    }

    public virtual void OnHit(int dmg)
    {
        if (dead) return;

        animator.SetBool("Hit", true);
        hp-=dmg;
        hitT = hitDur;
        GetComponent<SpriteRenderer>().color = Color.black;
        if (hp < 0)
        {
            OnDie();
        }
    }

    public void Push(Vector3 dir)
    {
        transform.position += dir;
    }

    protected virtual void OnDie()
    {
        DeathCounterManager.Instance.DeathCounter += 1;
        renderer.color = Color.white;
        animator.SetBool("Dead", true);
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        dead = true;
        Destroy(gameObject, 5.0f);
        if (EnemyManager.INSTANCE != null)
            EnemyManager.INSTANCE.EnemyDestroyed();

        Hero.targeted.Remove(this);

    }


    private void OnCollisionEnter(Collision other)
    {
        // var tower = other.gameObject.GetComponent<Tower>();
        // if (tower != null)
        // {
        //     tower.Fire(transform.position);
        // }
        var fireball = other.gameObject.GetComponent<FireBall>();
        if (fireball != null)
        {
            OnDie();
        }
    }

    public void BuffHP(KiwiPizza kp)
    {
        if (pizzaList.Contains(kp)) return;
        pizzaList.Add(kp);
        
        if (hpBuffed) return;
        hpBuffed = true;
        hp += data.hp / 2;

        buffHpFx.Play();
    }
    public void DebuffHP(KiwiPizza kp)
    {
        if (!pizzaList.Contains(kp)) return;
        pizzaList.Remove(kp);
        if (pizzaList.Count > 0) return;
        
        if (!hpBuffed) return;
        hpBuffed = false;
        hp -= data.hp / 2;

        buffHpFx.Stop();
        
        if (hp < 0)
            OnDie();
    }


    public void BuffSpeed(Virtue v)
    {
        if (virtueList.Contains(v)) return;
        virtueList.Add(v);

        if (speedBuffed) return;
        speedBuffed = true;
        speed = data.speed * 1.5f;

        buffSpeedFX.Play();

    }
    public void DebuffSpeed(Virtue v)
    {
        if (!virtueList.Contains(v)) return;
        virtueList.Remove(v);
        if (virtueList.Count > 0) return;

        if (!speedBuffed) return;
        speedBuffed = false;
        speed = data.speed;

        buffSpeedFX.Stop();

    }

}
