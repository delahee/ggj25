using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class SpawnManager : MonoBehaviour
{
    public GameObject prefab;
    public float cooldown = 1.0f;
    public float timer = 1.0f;
    public float t = 0.0f;

    private void Awake()
    {
        StartCoroutine(SpawnRoutine());
    }


    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(cooldown);
        while (true) {
            var go = Instantiate(prefab);
            go.transform.position = transform.position;
            yield return new WaitForSeconds(timer);
        }

        
        //foreach (var spawn in spawnList)
        //{
        //    yield return new WaitForSeconds(spawn.cooldown);
        //    for (int i = 0; i < spawn.q; i++)
        //    {
        //        Enemy e = Instantiate(spawn.e.prefab).GetComponent<Enemy>();
        //        if (e != null) e.Init(spawn.e);
        //        SetPosition(e.gameObject);
        //        yield return new WaitForSeconds(timer);
        //    }
        //}
    }

}
