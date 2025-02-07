using System.Collections.Generic;
using TMPro;
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

        PopulateUpgrades();
    }

    #endregion

    #region Upgrades

    public List<string> UniqueUpgrades;
    public Button Equipment, Volcano, Economy;
    public TextMeshProUGUI EquipPrice, VolcanoPrice, EcoPrice;

    public void CalculateUpgradePrice()
    {
        float equipPrice = EquipmentLevel * 1.5f + (VolcanoLevel + EcoLevel) * 1.1f;
        float volcanoPrice = VolcanoLevel * 1.5f + (EquipmentLevel + EcoLevel) * 1.1f;
        float ecoPrice = EcoLevel * 1.5f + (VolcanoLevel + EquipmentLevel) * 1.1f;

        EquipPrice.text = equipPrice + " Pops";
        VolcanoPrice.text = volcanoPrice + " Pops";
        EcoPrice.text = ecoPrice + " Pops";

        bool heroesFull = true;
        foreach (var spawnGo in HeroesManager.INSTANCE.SpawnPoints)
        {
            SpawnPoint spawn = spawnGo.GetComponent<SpawnPoint>();
            if (spawn.isAvailable)
            {
                heroesFull = false;
                break;
            }
        }

        Equipment.interactable = (equipPrice <= GameManager.Instance.Pops && !heroesFull);
        Volcano.interactable = (volcanoPrice <= GameManager.Instance.Pops);
        Economy.interactable = (ecoPrice <= GameManager.Instance.Pops);

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
        else
            CalculateUpgradePrice();
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
            dep.DependsOnScience = s.dependence;

            if (dep.CheckDependence() != "" && !UniqueUpgrades.Contains(dep.CheckDependence()))
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
            dep.DependsOnEco = e.dependence;

            if (dep.CheckDependence() != "" && !UniqueUpgrades.Contains(dep.CheckDependence()))
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
        }

        foreach (var e in GameManager.Instance.Data.HeroesUpgrades)
        {
            UpgradeDependence dep = new UpgradeDependence();
            //dep.DependsOnEco = e.dependence;

            /*if (dep.CheckDependence() != "" && !UniqueUpgrades.Contains(dep.CheckDependence()))
                continue;*/

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
                    HellButton[] buttons = selectionGO.GetComponentsInChildren<HellButton>();
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        buttons[i].onClick.RemoveAllListeners();
                    }
                    selection.Clear();
                    Destroy(selectionGO);
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
    #endregion

    #region UI utils

    public GameObject SelectionPrefab;

    public void UpgradeButton(string type)
    {
        //TODO pause game
        switch (type)
        {
            case "equip":
                equipmentUpgrades.Shuffle();
                selection.Add(equipmentUpgrades[0]);
                selection.Add(equipmentUpgrades[1]);
                selection.Add(equipmentUpgrades[2]);
                EquipmentLevel++;
                break;

            case "volcano":
                volcanoUpgrades.Shuffle();
                selection.Add(volcanoUpgrades[0]);
                selection.Add(volcanoUpgrades[1]);
                selection.Add(volcanoUpgrades[2]);
                VolcanoLevel++;
                break;
            
            case "eco":
                ecoUpgrades.Shuffle();
                selection.Add(ecoUpgrades[0]);
                selection.Add(ecoUpgrades[1]);
                selection.Add(ecoUpgrades[2]);
                EcoLevel++;
                break;
            
            case "hero":
                heroUpgrades.Shuffle();
                selection.Add(heroUpgrades[0]);
                selection.Add(heroUpgrades[1]);
                selection.Add(heroUpgrades[2]);
                EquipmentLevel++;
                break;
        }
        PopulateSelection();
    }
       
    public void SelectUpgrade(int index)
    {
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
        Time.timeScale = 1.0f;
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
