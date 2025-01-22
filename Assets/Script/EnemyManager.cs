using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager INSTANCE;
 
    public List<GameObject> enemiesPrefab;
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
            GameObject enemyGo = Instantiate(enemiesPrefab[Random.Range(0,enemiesPrefab.Count)], transform);
            enemyGo.transform.position = new Vector3(Random.Range(-20.0f,20.0f), 1.5f, transform.position.z);
            Enemy enemy = enemyGo.GetComponent<Enemy>();
            
        }
    }
}
