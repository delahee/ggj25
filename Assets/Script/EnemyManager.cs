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

    private void Awake()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:Music/Music_Game");
    }

    // Start is called before the first frame update
    void Start()
    {
        SortEnemies();
        INSTANCE = this;
        InvokeRepeating(nameof(IncreaseSpawnAMountEachSecond), 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        elapspedTime += Time.deltaTime;
        
        if (enemiesCounter < maxEnemies && elapspedTime > dtEnemySpawn)
        {
            elapspedTime = 0;
            
            // Select only demons which their tier is less than actual
            int curStep = 0;
            for (int i = 0; i < steps.Length; i++)
            {
                if (steps[i] < Time.timeSinceLevelLoad)
                    curStep = i+1;
                else
                    break;
            }

            if (curStep > 0)
            {
                int maxIdx = 0;
                foreach (GameObject go in enemiesPrefab)
                {
                    Enemy en = go.GetComponent<Enemy>();
                    if (en.data.tier <= curStep)
                        maxIdx++;
                    else
                        break;
                }
                
                GameObject prefab = enemiesPrefab[Random.Range(0, maxIdx)];
                GameObject enemyGo = Instantiate(prefab, transform);
                enemyGo.transform.position = new Vector3(Random.Range(-lineLength, lineLength), 1.5f, transform.position.z);
                enemiesCounter++;
            }
        }

        // Increase maxEnemies every 10 kills
        if (enemiesKilled % 10 == 0 && enemiesKilled > 0)
        {
            if (dtEnemySpawn > 1)
                dtEnemySpawn -= 1;
            maxEnemies++;
        }
    }

    void IncreaseSpawnAMountEachSecond()
    {
        if (Time.timeSinceLevelLoad > steps[steps.Length - 1])
        {
            dtEnemySpawn -= (1/60.0f)*dtEnemySpawn;
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
