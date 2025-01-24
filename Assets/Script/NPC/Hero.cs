using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour, IHit
{
    public GameObject atkFX;


    public float speed = 3.0f;
    public float pushForce = 1.0f;


    public Heroes data;
    public int hp = 10;
    Enemy targetEnemy;

    public float atkMaxDuration = 1.0f;
    public float atkT = 0f;

    Vector3 patrolPos;
    float patrolT = 0.0f;
    float patrolInterval = 3.0f;

    public bool dead;



    void Init(Heroes data)
    {
        this.data = data;
        hp = data.hp;
    }

    private void Awake()
    {
        Init(data);
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

    private void Update()
    {
        if (dead) return;

        if (targetEnemy == null)
        {
            SeekEnemy();
        }

        if (targetEnemy == null) {

            if (Vector3.Distance(transform.position, transform.parent.position) > 10) { 
                transform.position = Vector3.MoveTowards(transform.position, transform.parent.position, speed * Time.deltaTime);
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
            Vector3 move = Vector3.MoveTowards(transform.position, targetEnemy.transform.position, speed * Time.deltaTime);
            transform.position = move;
        }
        else
        {
            AttackSequence();
        }
    }

    void RequestPatrolPos()
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
    void PatrolSequence()
    {
        if (Vector3.Distance(transform.position, patrolPos) > .1f) 
            transform.position = Vector3.MoveTowards(transform.position, patrolPos, speed * Time.deltaTime);
        else 
            patrolT -= Time.deltaTime;
        
        if (patrolT <= 0)
        {
            patrolT = patrolInterval;
            RequestPatrolPos();
        }
    }

    void SeekEnemy()
    {
        GateOfHell gate = GateOfHell.instance;
        if (gate == null) return;

        float max = -float.MaxValue;
        foreach (Enemy e in FindObjectsOfType<Enemy>())
        {
            if (e.dead) continue;
            float score = Vector3.Distance(gate.transform.position, e.transform.position) - Vector3.Distance(transform.position, e.transform.position);
            if (score > max) max = score;
            targetEnemy = e;
        }
        return;

        float minDist = GateOfHell.instance.radius;
        foreach (Enemy e in FindObjectsOfType<Enemy>())
        {
            if (e.dead) continue;
            var dist = Vector3.Distance(e.transform.position, transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                targetEnemy = e;
            }
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
        Vector3 dir = o.transform.position - transform.position;
        dir.y = 0;
        o.Push(dir.normalized*pushForce);
        o.OnHit(data.AtkDmgBasis);
        Instantiate(atkFX, o.transform.position, o.transform.rotation);
        o.target = this;
        atkT = Random.Range(0f, atkMaxDuration);
    }


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
