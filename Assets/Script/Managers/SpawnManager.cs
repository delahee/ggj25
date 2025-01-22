using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject prefab;

    private void Awake()
    {
        StartCoroutine(SpawnRoutine()); 
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(.5f);
        Spawn();
        StartCoroutine(SpawnRoutine());
    }

    void Spawn()
    {
        GameObject go = Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
