using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = FMOD.Debug;

public class HeroesManager : MonoBehaviour
{
    public static HeroesManager INSTANCE;
    
    public List<GameObject> SpawnPoints;
    
    public GameObject SmithPrefab;
    public GameObject StargazerPrefab;
    public GameObject DancerPrefab;
    public GameObject FighterPrefab;
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

    public void SpawnHero(string heroName)
    {
        foreach (var spawnGo in SpawnPoints)
        {
            SpawnPoint spawn = spawnGo.GetComponent<SpawnPoint>();
            if (spawn.isAvailable)
            {
                GameObject heroGo = instantiateHero(heroName);
                heroGo.transform.SetParent(spawnGo.transform);
                spawn.isAvailable = false;
                spawn.attachedHero = heroGo.GetComponent<Hero>();
                heroGo.transform.position = spawn.transform.position;
                GameManager.Instance.Melts -= 5;
                return;
            }
        }
        UnityEngine.Debug.Log("All Spawnpoint full");
    }

    private GameObject instantiateHero(string heroName)
    {
        if ("SMITH".Equals(heroName))
            return Instantiate(SmithPrefab, transform);
        else if ("STARGAZER".Equals(heroName))
            return Instantiate(StargazerPrefab, transform);
        else if ("DANCER".Equals(heroName))
            return Instantiate(DancerPrefab, transform);
        else if ("FIGHTER".Equals(heroName))
            return Instantiate(FighterPrefab, transform);
        else if ("CERBERUS".Equals(heroName))
            return Instantiate(KerberosPrefab, transform);
        
        return Instantiate(PyroPrefab, transform);
    }
    
}
