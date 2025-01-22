using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager INSTANCE;
    
    public GameObject enemyPrefab;
    public GateOfHell gateOfHell;
    public float dtEnemySpawn = 1;
    private float elapspedTime;
    
    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;
    }

    // Update is called once per frame
    void Update()
    {
        elapspedTime += Time.deltaTime;
        if (elapspedTime > dtEnemySpawn)
        {
            elapspedTime = 0;
            GameObject enemyGo = Instantiate(enemyPrefab, transform);
            enemyGo.transform.position = new Vector3(Random.Range(-4.0f,4.0f), 1.5f, transform.position.z);
            Enemy enemy = enemyGo.GetComponent<Enemy>();
            
        }
    }
}
