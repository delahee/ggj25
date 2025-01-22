using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct EnemySpawn
{
    public Enemies e;
    public float cooldown;
    public int q;
}

public class SpawnManager : MonoBehaviour
{
    public List<EnemySpawn> spawnList;
    public float timer = 1.0f;

    private void Awake()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        foreach (var spawn in spawnList)
        {
            yield return new WaitForSeconds(spawn.cooldown);
            for (int i = 0; i < spawn.q; i++)
            {
                Enemy e = Instantiate(spawn.e.prefab).GetComponent<Enemy>();
                if (e != null) e.Init(spawn.e);
                SetPosition(e.gameObject);
                yield return new WaitForSeconds(timer);
            }
        }
    }

    void SetPosition(GameObject go)
    {
        go.transform.position = transform.position;
    }
}
