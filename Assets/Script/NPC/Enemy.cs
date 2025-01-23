using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml.Linq;
using UnityEditor.Timeline.Actions;
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



    public void Init(Enemies data)
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
        if (target)        
            pos = target.transform.position;
        

        if (Vector3.Distance(transform.position, pos) > 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, data.speed * Time.deltaTime);
        }
        else
        {
            AttackSequence();
        }
    }


    void AttackSequence()
    {
        atkT -= Time.deltaTime;
        if (atkT < 0.0f)
            Attack();
    }

    void Attack()
    {
        atkT = UnityEngine.Random.Range(0f, atkMaxDuration);
        animator.SetTrigger("Atk");
        if (target)
        {
            Vector3 dir = target.transform.position - transform.position;
            dir.y = 0;
            target.Push(dir.normalized * pushForce);
            Instantiate(atkFX, target.transform.position, target.transform.rotation);
            target.OnHit(dmg);
        }
        else
        {
            door?.OnHit(data.dmg);
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
        renderer.color = Color.white;
        animator.SetBool("Dead", true);
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        dead = true;
        Destroy(gameObject, 30.0f);
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
