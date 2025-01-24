using System.Collections.Generic;
using FMODUnity;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager INSTANCE;

    public float lineLength;
    public float[] steps;
    public List<GameObject> enemiesPrefab;
    public float dtEnemySpawn = 1;
    private float elapspedTime;


    private int enemiesCounter;
    private int maxEnemies = 10;
    private int enemiesKilled;

    [ContextMenu("Sort Enemy Prefabs")]
    void SortEnemies()
    {
        bool permut = true;
        while (permut)
        {
            permut = false;
            for (int i = 0; i < enemiesPrefab.Count - 1; i++)
            {
                Enemy e1 = enemiesPrefab[i].GetComponent<Enemy>();
                Enemy e2 = enemiesPrefab[i + 1].GetComponent<Enemy>();
                int tier1 = e1.data.tier;
                int tier2 = e2.data.tier;
                if (tier1 > tier2)
                {
                    permut = true;
                    GameObject temp = enemiesPrefab[i+1];
                    enemiesPrefab[i + 1] = enemiesPrefab[i];
                    enemiesPrefab[i] = temp;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;
    }

    // Update is called once per frame
    void Update()
    {
        elapspedTime += Time.deltaTime;
        float timeSinceLoaded = Time.timeSinceLevelLoad;
        int curStep = 0;
        if (enemiesCounter < maxEnemies && elapspedTime > dtEnemySpawn)
        {
            elapspedTime = 0;
            curStep = 0;
            for (int i = 0; i < steps.Length; i++)
            {
                if (steps[i] < timeSinceLoaded)
                    curStep = i;
            }

            float maxIdx = 0;
            foreach (GameObject go in enemiesPrefab)
            {
                Enemy en = go.GetComponent<Enemy>();
                if (en.data.tier <= curStep)
                    maxIdx++;
                else
                    break;
            }

            GameObject prefab = enemiesPrefab[Random.Range(0, curStep)];
            Enemy e = prefab.GetComponent<Enemy>();
            if (e == null) return;
                        
            GameObject enemyGo = Instantiate(prefab, transform);
            enemyGo.transform.position = new Vector3(Random.Range(-lineLength,lineLength), 1.5f, transform.position.z);
            Enemy enemy = enemyGo.GetComponent<Enemy>();
            enemiesCounter++;
        }
        
        // Increase maxEnemies every 10 kills
        if (enemiesKilled % 10 == 0)
        {
            if (dtEnemySpawn > 1)
                dtEnemySpawn -= 1;
            maxEnemies++;
        }
    }

    public void EnemyDestroyed()
    {
        enemiesCounter--;
        enemiesKilled++;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 start = Vector3.forward * transform.position.z;
        Gizmos.DrawLine(start + Vector3.left * lineLength / 2, start + Vector3.right * lineLength / 2);
    }
}
