using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = FMOD.Debug;

public class HeroesManager : MonoBehaviour
{
    public static HeroesManager INSTANCE;
    
    public List<GameObject> SpawnPoints;
    
    public GameObject SmithPrefab;
    public GameObject StargazerPrefab;
    public GameObject DancerPrefab;
    public GameObject HellgirlPrefab;
    public GameObject PyroPrefab;
    public GameObject KerberosPrefab;
    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnHero(string name)
    {
        foreach (var spawnGo in SpawnPoints)
        {
            SpawnPoint spawn = spawnGo.GetComponent<SpawnPoint>();
            if (spawn.isAvailable)
            {   
                GameObject pyro = Instantiate(PyroPrefab, transform);
                spawn.isAvailable = false;
                spawn.attachedHero = pyro.GetComponent<Hero>();
                GameManager.Instance.Melts -= 10;
                return;
            }
        }
        UnityEngine.Debug.Log("All Spawnpoint full");
    }
    
}
