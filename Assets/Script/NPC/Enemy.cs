using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml.Linq;
using UnityEngine;



public class Enemy : MonoBehaviour, IHit
{
    public GateOfHell door;
    public Enemies data;
    [Space]
    [HideInInspector] public int speed = 3;     // Obsolete. Use data.speed instead
    public int hp;

    public float atkMaxDuration = 2.0f;
    public float atkT = 0f;

    public float hitT;
    public float hitDur = .1f;

    bool dead = false;
    SpriteRenderer renderer;

    public void Init(Enemies data)
    {
        this.data = data;
        hp = data.hp 
            + (int)(Time.timeSinceLevelLoad / 20);  // Increases HP along time
        renderer=GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        Init(data);
    }

    protected virtual void Update()
    {
        if (hitT >= 0)
        {
            hitT -= Time.deltaTime;
            if (hitT < 0)
            {
                renderer.color = Color.white;
            }
        }

        GateOfHell gate = GateOfHell.instance;
        if (gate == null) return;

        if (Vector3.Distance(transform.position, gate.transform.position) > 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, gate.transform.position, data.speed * Time.deltaTime);
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
        atkT = Random.Range(0f, atkMaxDuration);
        GateOfHell.instance?.OnHit(data.dmg);
    }

    public void OnHit(int dmg)
    {
        if (dead) return;
        hp-=dmg;
        hitT = hitDur;
        GetComponent<SpriteRenderer>().color = Color.black;
        if (hp < 0)
        {
            OnDie();
        }
    }

    void OnDie()
    {
        dead = true;
        Destroy(gameObject);
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
    
}
