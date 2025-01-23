using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lich : Enemy
{
    public GameObject zombie;

    float invokeT = .0f;
    float invokeInterval = 2.0f;
    float invokeDuration = 2.0f;
    int q = 3;
    bool isSummoning;
     

    void Summon()
    {
        GameObject z = Instantiate(zombie);
        var c = Random.insideUnitCircle;
        z.transform.position = transform.position + new Vector3(c.x, 0, c.y);
        isSummoning = false;
        invokeDuration = 2.0f;
        invokeT = invokeInterval;
    }

    protected override void Update()
    {

        if (isSummoning)
        {
            invokeDuration -= Time.deltaTime;
            if (invokeDuration < 0)
                Summon();
        }
        else
        {
            base.Update();

            invokeT -= Time.deltaTime;
            if (invokeT < 0)
            {
                invokeT = invokeInterval;
                isSummoning = true;
            }
        }
    }

    public override void Init(Enemies data)
    {
        base.Init(data);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Enemy_Lich_Spawn");
    }
    
    protected override void OnDie()
    {
        base.OnDie();
        FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Enemy_Lich_Death");
    }
}
