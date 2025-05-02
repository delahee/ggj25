using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    #region Singleton

    private static UpgradeManager instance;
    public static UpgradeManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("UpgradeManager is null !!");

            return instance;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (instance != null)
            Destroy(Instance.gameObject);

        instance = this;

        UniqueUpgrades.Clear();
        Darkening = GameObject.Find("Darkening").GetComponent<SpriteRenderer>();
        PopulateUpgrades();
    }

    #endregion

    #region Upgrades

    public List<string> UniqueUpgrades;
    public Button Equipment, Volcano, Economy;
    public TextMeshProUGUI EquipPrice, VolcanoPrice, EcoPrice;
    public int equipPriceVar = 10;
    public int volcanoPriceVar = 10;
    public int ecoPriceVar = 10;

    public SpriteRenderer Darkening;
    public GameObject Lavastream;

    bool selectionongoing = false;
    
    public void CalculateUpgradePrice(string F)
    {
        if(F == "Volcano")
        {
            equipPriceVar = Mathf.RoundToInt((float)equipPriceVar * 1.1f);
            volcanoPriceVar = Mathf.RoundToInt((float)volcanoPriceVar * 1.5f);
            ecoPriceVar = Mathf.RoundToInt((float)ecoPriceVar * 1.1f);
        }
        else if (F == "Equipment")
        {
            equipPriceVar = Mathf.RoundToInt((float)equipPriceVar * 1.5f);
            volcanoPriceVar = Mathf.RoundToInt((float)volcanoPriceVar * 1.1f);
            ecoPriceVar = Mathf.RoundToInt((float)ecoPriceVar * 1.1f);
        }
        else if (F == "Economy")
        {
            equipPriceVar = Mathf.RoundToInt((float)equipPriceVar * 1.1f);
            volcanoPriceVar = Mathf.RoundToInt((float)volcanoPriceVar * 1.1f);
            ecoPriceVar = Mathf.RoundToInt((float)ecoPriceVar * 1.5f);
        }

        /*float equipPrice = EquipmentLevel * 1.5f + (VolcanoLevel + EcoLevel) * 1.1f;
        float volcanoPrice = VolcanoLevel * 1.5f + (EquipmentLevel + EcoLevel) * 1.1f;
        float ecoPrice = EcoLevel * 1.5f + (VolcanoLevel + EquipmentLevel) * 1.1f;*/

        EquipPrice.text = equipPriceVar + " Pops";
        VolcanoPrice.text = volcanoPriceVar + " Pops";
        EcoPrice.text = ecoPriceVar + " Pops";

        /*bool heroesFull = true;
        foreach (var spawnGo in HeroesManager.INSTANCE.SpawnPoints)
        {
            SpawnPoint spawn = spawnGo.GetComponent<SpawnPoint>();
            if (spawn.isAvailable)
            {
                heroesFull = false;
                break;
            }
        }

        Equipment.interactable = (equipPriceVar <= GameManager.Instance.Pops && !heroesFull); //Checked if heroes are full, but equipment should still be buyable
        Volcano.interactable = (volcanoPriceVar <= GameManager.Instance.Pops);
        Economy.interactable = (ecoPriceVar <= GameManager.Instance.Pops);*/

        CheckInteraction();

        if (Equipment.interactable)
        {
            var texts = Equipment.GetComponentsInChildren<TextMeshProUGUI>();
            foreach(var t in texts)
                t.color = Color.white;
        }
        else
        {
            var texts = Equipment.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var t in texts)
                t.color = Color.red;
        }

        if (Volcano.interactable)
        {
            var texts = Volcano.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var t in texts)
                t.color = Color.white;
        }
        else
        {
            var texts = Volcano.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var t in texts)
                t.color = Color.red;
        }

        if (Economy.interactable)
        {
            var texts = Economy.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var t in texts)
                t.color = Color.white;
        }
        else
        {
            var texts = Economy.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var t in texts)
                t.color = Color.red;
        }

    }

    int imps;

    public void CheckInteraction() // NEW -> Function to check interaction
    {
        Equipment.interactable = (equipPriceVar <= GameManager.Instance.Pops);
        Volcano.interactable = (volcanoPriceVar <= GameManager.Instance.Pops);
        Economy.interactable = (ecoPriceVar <= GameManager.Instance.Pops);
    }

    public static int Imps
    {
        get => Instance.imps;
        set { Instance.imps = value; BubbleManager.Instance.CalculateAutoRate(value); }
    }

    int equipmentLevel = 0;
    int volcanoLevel   = 0;
    int ecoLevel       = 0;

    //TODO Add juice here =)
    public int EquipmentLevel { get => equipmentLevel; set => equipmentLevel = value; }
    public int VolcanoLevel { get => volcanoLevel; set => volcanoLevel = value; }
    public int EcoLevel { get => ecoLevel; set => ecoLevel = value; }

    List<Upgrade> equipmentUpgrades = new List<Upgrade>();
    List<Upgrade> volcanoUpgrades   = new List<Upgrade>();
    List<Upgrade> ecoUpgrades       = new List<Upgrade>();
    List<Upgrade> heroUpgrades       = new List<Upgrade>();

    private void Update()
    {
        if (selectionGO)
        {
            Equipment.interactable = false;
            Volcano.interactable = false;
            Economy.interactable = false;

            HellButton[] buttons = selectionGO.GetComponentsInChildren<HellButton>();
            PriceUpdater[] priceTag = selectionGO.GetComponentsInChildren<PriceUpdater>();
            
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].tag == "Skip")
                    continue;
                if (selection[i].PopCost > GameManager.Instance.Pops ||
                    selection[i].MeltCost > GameManager.Instance.Melts ||
                    selection[i].MithrilCost > GameManager.Instance.Mithrils)
                {
                    priceTag[i].price.color = Color.red;
                    buttons[i].interactable = false;
                }
            }
        }

        else if(selectionongoing)
        {
            Equipment.interactable = false;
            Volcano.interactable = false;
            Economy.interactable = false;
        }
        else
            CalculateUpgradePrice("None");
    }

    void PopulateUpgrades()
    {
        equipmentUpgrades.Clear();
        volcanoUpgrades.Clear();
        ecoUpgrades.Clear();
        heroUpgrades.Clear();

        /*equipmentUpgrades.Add(new Upgrade("tech1", UpgradeType.Equipment));
        equipmentUpgrades.Add(new Upgrade("tech2", UpgradeType.Equipment));
        equipmentUpgrades.Add(new Upgrade("tech3", UpgradeType.Equipment));
        equipmentUpgrades.Add(new Upgrade("tech4", UpgradeType.Equipment));*/

        foreach (var s in GameManager.Instance.Data.SciencesUpgrades) 
        { 
            UpgradeDependence dep = new UpgradeDependence();
            dep.Depends = s.dependence;

            List<string> deps = dep.CheckDependence();
            bool contains = true;
            if (deps.Count > 0)
            {
                foreach (string depsi in deps)
                {
                    if (!UniqueUpgrades.Contains(depsi)) contains = false;
                }
            }
            if (!contains || UniqueUpgrades.Contains(s.scienceName))
                continue;

            volcanoUpgrades.Add(new Upgrade(
                s.scienceName, 
                UpgradeType.Volcano, 
                s.desc, 
                s.fx,
                s.isUnique,
                dep,
                s.EffectMethodName,
                s.prefab,
                s.popCost, 
                s.meltCost, 
                s.mithrilCost
                ));
        }

        foreach(var e in GameManager.Instance.Data.EconomyUpgrades)
        {
            UpgradeDependence dep = new UpgradeDependence();
            dep.Depends = e.dependence;

            List<string> deps = dep.CheckDependence();
            bool contains = true;
            if (deps.Count > 0)
            {
                foreach (string depsi in deps)
                {
                    if (!UniqueUpgrades.Contains(depsi)) contains = false;
                }
            }
            if (!contains || UniqueUpgrades.Contains(e.ecoName))
                continue;

            ecoUpgrades.Add(new Upgrade(
                e.ecoName,
                UpgradeType.Volcano,
                e.desc,
                e.fx,
                e.isUnique,
                dep,
                e.EffectMethodName,
                null,
                e.popCost,
                e.meltCost,
                e.mithrilCost
                ));
            if (e.ecoName == "Iron Mine") Debug.Log("populated");
        }

        foreach (var e in GameManager.Instance.Data.HeroesUpgrades)
        {
            UpgradeDependence dep = new UpgradeDependence();
            //dep.DependsOnEco = e.dependence;
           /* List<string> deps = dep.CheckDependence();
            bool contains = true;
            if (deps.Count > 0) 
            {
                foreach (string depsi in deps)
                {
                    if (!UniqueUpgrades.Contains(depsi)) contains = false;
                }
            }
            if (deps.Count != 0 && !contains && !UniqueUpgrades.Contains(e.name))
            {                
                continue;
            }*/
            if (UniqueUpgrades.Contains(e.heroName))
            {
                continue;
            }
            heroUpgrades.Add(new Upgrade(
                e.id,
                UpgradeType.Hero,
                e.desc,
                e.fx,
                false,
                dep,
                "",
                null,
                0,
                0,
                0
                ));
        }

       /* heroUpgrades.Add(new Upgrade("SMITH", UpgradeType.Hero));
        heroUpgrades.Add(new Upgrade("STARGAZER", UpgradeType.Hero));
        heroUpgrades.Add(new Upgrade("DANCER", UpgradeType.Hero));
        heroUpgrades.Add(new Upgrade("FIGHTER", UpgradeType.Hero));
        heroUpgrades.Add(new Upgrade("PYRO", UpgradeType.Hero));
        heroUpgrades.Add(new Upgrade("CERBERUS", UpgradeType.Hero));*/
    }

    [HideInInspector]
    public GameObject selectionGO;
    List<Upgrade> selection = new List<Upgrade>();

    void PopulateSelection()
    {
        Time.timeScale = 0.0f;

        selectionGO = Instantiate(SelectionPrefab, transform.parent);
        HellButton[] buttons = selectionGO.GetComponentsInChildren<HellButton>();
        PriceUpdater[] priceTag = selectionGO.GetComponentsInChildren<PriceUpdater>();
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].tag == "Skip")
                buttons[i].onClick.AddListener(() => {
                    /*HellButton[]*/ buttons = selectionGO.GetComponentsInChildren<HellButton>();
                    for (int j = 0; j < buttons.Length; j++)
                    {
                        buttons[j].onClick.RemoveAllListeners();
                    }
                    selection.Clear();
                    Destroy(selectionGO);
                    Time.timeScale = 1.0f;
                });
            else
            {
                buttons[i].GetComponentInChildren<TMP_Text>().text = selection[i].Name;
                buttons[i].transform.GetChild(0).GetChild(0).GetComponentInChildren<TMP_Text>().text = selection[i].Effect;
                int index = i;
                buttons[i].onClick.AddListener(() => SelectUpgrade(index));
                priceTag[i].price.text = priceTag[i].PriceFormatter(selection[i].PopCost, selection[i].MeltCost, selection[i].MithrilCost);

                if (selection[i].PopCost > GameManager.Instance.Pops ||
                    selection[i].MeltCost > GameManager.Instance.Melts ||
                    selection[i].MithrilCost > GameManager.Instance.Mithrils)
                {
                    priceTag[i].price.color = Color.red;
                    buttons[i].interactable = false;
                }
            }
        }
    }

    IEnumerator PopSelection()
    {
        selectionGO = Instantiate(SelectionPrefab, transform.parent);
        HellButton[] buttons = selectionGO.GetComponentsInChildren<HellButton>();
        PriceUpdater[] priceTag = selectionGO.GetComponentsInChildren<PriceUpdater>();
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].tag == "Skip")
            {
                buttons[i].onClick.AddListener(() =>
                {
                    /*HellButton[]*/
                    buttons = selectionGO.GetComponentsInChildren<HellButton>();
                    for (int j = 0; j < buttons.Length; j++)
                    {
                        buttons[j].onClick.RemoveAllListeners();
                    }
                    selection.Clear();
                    Destroy(selectionGO);
                    Time.timeScale = 1.0f;
                    StartCoroutine(buyingcinematic_end());

                }); buttons[i].gameObject.SetActive(false);

            }
            else
            {
                buttons[i].GetComponentInChildren<TMP_Text>().text = selection[i].Name;
                buttons[i].transform.GetChild(0).GetChild(0).GetComponentInChildren<TMP_Text>().text = selection[i].Effect;
                int index = i;
                buttons[i].onClick.AddListener(() => SelectUpgrade(index));
                priceTag[i].price.text = priceTag[i].PriceFormatter(selection[i].PopCost, selection[i].MeltCost, selection[i].MithrilCost);

                if (selection[i].PopCost > GameManager.Instance.Pops ||
                    selection[i].MeltCost > GameManager.Instance.Melts ||
                    selection[i].MithrilCost > GameManager.Instance.Mithrils)
                {
                    priceTag[i].price.color = Color.red;
                    buttons[i].interactable = false;
                }
                buttons[i].transform.Rotate(new Vector3(0, 90));
                buttons[i].transform.localScale = new Vector3(0, 0, 0);
            }
        }

        for (int d = 0; d < 60; d++)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].tag == "Skip" && d == 59)
                {
                    buttons[i].gameObject.SetActive(true);
                }
                else if(buttons[i].tag != "Skip" && d == 59)
                {
                    buttons[i].transform.rotation = Quaternion.identity;
                    buttons[i].transform.localScale = new Vector3(4, 4, 4);
                }
                else if(buttons[i].tag != "Skip")
                {
                    buttons[i].transform.Rotate(new Vector3(0, -1.5f));
                    if(buttons[i].transform.localScale.x < 4 ) buttons[i].transform.localScale = new Vector3((3.0f * 4.0f*d)/60.0f, (3.0f * 4.0f * d)/60.0f, (3.0f * 4.0f * d)/60.0f);
                    if(buttons[i].transform.localScale.x > 4 ) buttons[i].transform.localScale = new Vector3(4, 4, 4);
                }            
            }
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForEndOfFrame();
    }
    #endregion

        #region UI utils

    public GameObject SelectionPrefab;

    public void UpgradeButton(string type)
    {

        //TODO pause game
        selectionongoing = true;
        StartCoroutine(buyingcinematic(type));
        /*switch (type)
        {
            case "equip":
                GameManager.Instance.Pops = GameManager.Instance.Pops - equipPriceVar;
                equipmentUpgrades.Shuffle();
                selection.Add(equipmentUpgrades[0]);
                selection.Add(equipmentUpgrades[1]);
                selection.Add(equipmentUpgrades[2]);
                EquipmentLevel++;
                CalculateUpgradePrice("Equipment");
                break;

            case "volcano":
                GameManager.Instance.Pops = GameManager.Instance.Pops - volcanoPriceVar;
                volcanoUpgrades.Shuffle();
                selection.Add(volcanoUpgrades[0]);
                selection.Add(volcanoUpgrades[1]);
                selection.Add(volcanoUpgrades[2]);
                VolcanoLevel++;
                CalculateUpgradePrice("Volcano");
                break;
            
            case "eco":
                GameManager.Instance.Pops = GameManager.Instance.Pops - ecoPriceVar;
                ecoUpgrades.Shuffle();
                selection.Add(ecoUpgrades[0]);
                selection.Add(ecoUpgrades[1]);
                selection.Add(ecoUpgrades[2]);
                EcoLevel++;
                CalculateUpgradePrice("Economy");
                break;
            
            case "hero":
                GameManager.Instance.Pops = GameManager.Instance.Pops - equipPriceVar;
                heroUpgrades.Shuffle();
                selection.Add(heroUpgrades[0]);
                selection.Add(heroUpgrades[1]);
                selection.Add(heroUpgrades[2]);
                EquipmentLevel++;
                CalculateUpgradePrice("Equipment");
                break;
        }
        PopulateSelection();*/
    }
       
    public void SelectUpgrade(int index)
    {
        if (selection[index].IsUnique) 
        {
            UniqueUpgrades.Add(selection[index].Name);
            Debug.Log("UNIQUE ADDED");
        }
        selection[index].DoUpgrade();  
        if (!UpgradeType.Hero.Equals(selection[index].Type))
        {
            GameManager.Instance.Pops -= selection[index].PopCost;
            GameManager.Instance.Melts -= selection[index].MeltCost;
            GameManager.Instance.Mithrils -= selection[index].MithrilCost;
        }
        HellButton[] buttons = selectionGO.GetComponentsInChildren<HellButton>();
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.RemoveAllListeners();
        }
        selection.Clear();
        PopulateUpgrades();
        StartCoroutine(buyingcinematic_end());
    }

    Vector3 CameraTransform;
    float CameraSize;
    Color DarkColor;

    IEnumerator buyingcinematic(string type)
    {
        Equipment.interactable = false;
        Volcano.interactable = false;
        Economy.interactable = false;

        // Zoom on magma + darkening except magma + UI moves on the side + light screenshake
        GameObject Camera = GameObject.Find("Main Camera");
        CameraTransform = Camera.transform.position;
        CameraSize = Camera.GetComponent<Camera>().orthographicSize;
        DarkColor = Darkening.color;
        int Duration = 90;
        for (int i = 0; i < Duration; i++)
        {
            Camera.GetComponent<Camera>().orthographicSize -= 0.05f;
            if (i == 0) Camera.transform.Translate(new Vector3(0.3f, 0, 0));
            if (i%4 ==2) Camera.transform.Translate(new Vector3(-0.6f, 0, 0));
            else if (i % 4 == 0) Camera.transform.Translate(new Vector3(0.6f, 0, 0));
            Camera.transform.Translate(new Vector3(0, -0.1f, 0));
            Darkening.color = new Color(DarkColor.r, DarkColor.g, DarkColor.b, ((float)i/1.5f) / (float)Duration);

            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(lavaStream());

        //  Sudden dezoom + UI back + Erruption + potential light screenshake

        for (int i = 9; i >= 0; i--)
        {
            Camera.GetComponent<Camera>().orthographicSize += 0.45f;         
            Camera.transform.Translate(new Vector3(0, 1f, 0));
            //Darkening.color = new Color(DarkColor.r, DarkColor.g, DarkColor.b, 150 * (i / 9));

            yield return new WaitForFixedUpdate();
        }

        Camera.transform.position = CameraTransform;
        Camera.GetComponent<Camera>().orthographicSize = CameraSize;


        for (int i = 0; i < 90; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        // Cards come one after the other from the magma and turn

        switch (type)
        {
            case "equip":
                GameManager.Instance.Pops = GameManager.Instance.Pops - equipPriceVar;
                equipmentUpgrades.Shuffle();
                selection.Add(equipmentUpgrades[0]);
                selection.Add(equipmentUpgrades[1]);
                selection.Add(equipmentUpgrades[2]);
                EquipmentLevel++;
                CalculateUpgradePrice("Equipment");
                break;

            case "volcano":
                GameManager.Instance.Pops = GameManager.Instance.Pops - volcanoPriceVar;
                volcanoUpgrades.Shuffle();
                selection.Add(volcanoUpgrades[0]);
                selection.Add(volcanoUpgrades[1]);
                selection.Add(volcanoUpgrades[2]);
                VolcanoLevel++;
                CalculateUpgradePrice("Volcano");
                break;

            case "eco":
                GameManager.Instance.Pops = GameManager.Instance.Pops - ecoPriceVar;
                ecoUpgrades.Shuffle();
                selection.Add(ecoUpgrades[0]);
                selection.Add(ecoUpgrades[1]);
                selection.Add(ecoUpgrades[2]);
                EcoLevel++;
                CalculateUpgradePrice("Economy");
                break;

            case "hero":
                GameManager.Instance.Pops = GameManager.Instance.Pops - equipPriceVar;
                heroUpgrades.Shuffle();
                selection.Add(heroUpgrades[0]);
                selection.Add(heroUpgrades[1]);
                selection.Add(heroUpgrades[2]);
                EquipmentLevel++;
                CalculateUpgradePrice("Equipment");
                break;
        }
        //PopulateSelection();
        StartCoroutine(PopSelection());
        // Allow selection

        yield return new WaitForEndOfFrame();
    }

    IEnumerator buyingcinematic_end()
    {
        // Zoom on magma + darkening except magma + UI moves on the side + light screenshake
        GameObject Camera = GameObject.Find("Main Camera");

        // Darkening removed

        for (int i = 9; i >= 0; i--)
        {
            Darkening.color = new Color(DarkColor.r, DarkColor.g, DarkColor.b,(i / 9.0f));

            yield return new WaitForFixedUpdate();
        }
        Darkening.color = DarkColor;
        Time.timeScale = 1.0f;
        selectionongoing = false;
        yield return new WaitForEndOfFrame();
    }

    IEnumerator lavaStream()
    {
        int duration = 180;
        // 14.3
        for(int i = 0; i<duration; i++)
        {
            if (i % 13 == 12) 
            { 
                GameObject lava = Instantiate(Lavastream, new Vector3(-0.08f, 5, -12f), Quaternion.identity);
                lava.transform.Rotate(new Vector3(90, 0, 0));
                //lava.transform.Translate(new Vector3(Mathf.Cos((float)i / 10.0f),0,0));
            }
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForEndOfFrame();
    }
    #endregion
}

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}
