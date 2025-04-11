using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour, IHit
{
    public GameObject atkFX;

    public float speed = 3.0f;
    public float pushForce = 1.0f;
    public float regenSpeed = 1;
    public float regenT = 1;

    public Heroes data;
    public int hp = 10;
    protected Enemy targetEnemy;

    public float atkMaxDuration = 1.0f;
    public float atkT = 0f;

    protected Vector3 patrolPos;
    float patrolT = 0.0f;
    float patrolInterval = 3.0f;

    public bool dead;
    public static List<Enemy> targeted = new();
      
    protected Animator animator;
    protected SpriteRenderer spr;

    void Init(Heroes data)
    {
        this.data = data;
        hp = Mathf.FloorToInt(data.hp * (1+Upgrade.boostHeroHP));
    }

    private void Awake()
    {
        Init(data);
        animator = GetComponent<Animator>();
        spr = GetComponentInChildren<SpriteRenderer>();
        InvokeRepeating(nameof(SeekEnemy), 00.0f, .3f);

        string eventName = "event:/Heroes/General/Hero_Spawn";
        if ("SMITH".Equals(data.id))
            eventName = "event:/Heroes/Hero_Smith_Spawn";
        else if("STARGAZER".Equals(data.id))
            eventName = "event:/Heroes/Hero_Stargazer_Spawn";
        else if("DANCER".Equals(data.id))
            eventName = "event:/Heroes/Hero_Dancer_Spawn";
        else if("FIGHTER".Equals(data.id))
            eventName = "event:/Heroes/Hero_Fighter_Spawn";
        else if("PYRO".Equals(data.id))
            eventName = "event:/Heroes/Hero_Pyro_Spawn";
        else if("CERBERUS".Equals(data.id))
            eventName = "event:/Heroes/Hero_Cerberus_Spawn";
    }

    protected virtual void Start()
    {
        InvokeRepeating(nameof(SeekEnemy), 00.0f, .3f);
    }

    protected virtual void Update()
    {
        if (dead) return;

        regenT -= Time.deltaTime * regenSpeed * (1+Upgrade.boostHeroRegen);
        if (regenT < 0)
        {
            if (hp < data.hp * (1 + Upgrade.boostHeroHP))
            {
                regenT = 1.0f;
                hp++;
            }
        }


        if (targetEnemy == null || targetEnemy.dead) {

            if (Vector3.Distance(transform.position, transform.parent.position) > 10) { 
                MoveTo(transform.parent.position);
            }
            else
            {
                PatrolSequence();
            }

            return;
        }

        float dist = Vector3.Distance(transform.position, targetEnemy.transform.position);
        if (dist > 1)
        {
            MoveTo(targetEnemy.transform.position);
        }
        else
        {
            AttackSequence();
        }
    }
    
    protected void MoveTo(Vector3 into)
    {
        var dir = (into - transform.position);
        if (dir.magnitude > 0.01f)
            spr.flipX = dir.x < 0;
        Vector3 move = Vector3.MoveTowards(transform.position, into, speed * Time.deltaTime);
        move.y = 0f;
        transform.position = move;
    }

    protected void RequestPatrolPos()
    {
        Vector3 parentPos = transform.parent.position;

        if (Vector3.Distance(transform.position, parentPos) > 5)
        {
            Vector3 c = Random.insideUnitCircle * 5;
            patrolPos = parentPos + new Vector3(c.x, 0, c.y);
        }
        else
        {
            Vector3 c = Random.insideUnitCircle;
            patrolPos = transform.position + new Vector3(c.x, 0, c.y);
        }

    }
    protected void PatrolSequence()
    {
        if (Vector3.Distance(transform.position, patrolPos) > .1f) 
            MoveTo(patrolPos);
        else 
            patrolT -= Time.deltaTime;
        
        if (patrolT <= 0)
        {
            patrolT = patrolInterval;
            RequestPatrolPos();
        }
    }

    protected void SeekEnemy()
    {
        GateOfHell gate = GateOfHell.instance;
        if (gate == null) { 
            targetEnemy = null;
            return;
        }

        Enemy old = targetEnemy;
        float max = float.MaxValue;
        foreach (Enemy e in FindObjectsOfType<Enemy>())
        {
            if (e.dead) continue;
            if (targeted.Contains(e)) continue;
            if (e.transform.position.z > gate.radius) continue;

            float score = Vector3.Distance(gate.transform.position, e.transform.position);
            if (score < max)
            {
                max = score;
                targetEnemy = e;
            }
        }

        // Heroes does not aim the same enemy
        if (targetEnemy == null) return;
        if (old != null)
            targeted.Remove(old);
        if (!targeted.Contains(targetEnemy))
        {
            targeted.Add(targetEnemy);
        }
    }

    void AttackSequence()
    {
        atkT -= Time.deltaTime;
        if (atkT < 0.0f)
            Attack(targetEnemy);
    }

    void Attack(Enemy o)
    {
        animator.SetTrigger("atk");
        Vector3 dir = o.transform.position - transform.position;
        dir.y = 0;
        o.Push(dir.normalized*pushForce);
        o.OnHit(getDmg());
        Instantiate(atkFX, o.transform.position, o.transform.rotation);
        o.target = this;
        atkT = Random.Range(0f, atkMaxDuration);
        SeekEnemy();
    }

    protected int getDmg() => Mathf.FloorToInt(data.AtkDmgBasis * (1+Upgrade.boostHeroDmg));

    public void Push(Vector3 dir)
    {
        transform.position += dir;
    }
	

    public void OnHit(int dmg)
    {
        hp--;
        if (hp < 0)
        {
            OnDie();
        }
    }
	


    void OnDie()
    {
        dead = true;
        enabled = false;

        string eventName = "event:/Heroes/General/Hero_Death";
        if ("SMITH".Equals(data.id))
            eventName = "event:/Heroes/Hero_Smith_Death";
        else if("STARGAZER".Equals(data.id))
            eventName = "event:/Heroes/Hero_Stargazer_Death";
        else if("DANCER".Equals(data.id))
            eventName = "event:/Heroes/Hero_Dancer_Death";
        else if("FIGHTER".Equals(data.id))
            eventName = "event:/Heroes/Hero_Fighter_Death";
        else if("PYRO".Equals(data.id))
            eventName = "event:/Heroes/Hero_Pyro_Death";
        else if("CERBERUS".Equals(data.id))
            eventName = "event:/Heroes/Hero_Cerberus_Death";
        
        FMODUnity.RuntimeManager.PlayOneShot(eventName);

        Destroy(gameObject);
    }
	
}
