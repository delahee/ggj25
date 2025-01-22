using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lich : Enemy
{
    float invokeT = .0f;
    float invokeInterval = 2.0f;
    int q = 3;

     

    protected override void Update()
    {
        
        base.Update();

        invokeT -= Time.deltaTime;
    }
}
