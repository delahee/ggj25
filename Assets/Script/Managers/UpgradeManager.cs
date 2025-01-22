using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
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

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance != null)
            Destroy(Instance.gameObject);

        instance = this;

        PopulateUpgrades();
    }

    #endregion

    #region Upgrades

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

    //TODO populate with dev data
    void PopulateUpgrades()
    {
        equipmentUpgrades.Add(new Upgrade("tech1", UpgradeType.Equipment));
        equipmentUpgrades.Add(new Upgrade("tech2", UpgradeType.Equipment));
        equipmentUpgrades.Add(new Upgrade("tech3", UpgradeType.Equipment));
        equipmentUpgrades.Add(new Upgrade("tech4", UpgradeType.Equipment));

        foreach (Science vb in GameManager.Instance.Data.SciencesUpgrades) 
        { 
            volcanoUpgrades.Add(new Upgrade(vb.name,UpgradeType.Volcano, vb.desc, vb.effect, vb.popCost, vb.meltCost, vb.mithrilCost, vb.EffectMethodName));
        }

        ecoUpgrades.Add(new Upgrade("eco1", UpgradeType.Economy));
        ecoUpgrades.Add(new Upgrade("eco2", UpgradeType.Economy));
        ecoUpgrades.Add(new Upgrade("eco3", UpgradeType.Economy));
        ecoUpgrades.Add(new Upgrade("eco4", UpgradeType.Economy));
    }

    [HideInInspector]
    public GameObject selectionGO;
    List<Upgrade> selection = new List<Upgrade>();

    void PopulateSelection()
    {
        selectionGO = Instantiate(SelectionPrefab, transform.parent);
        Button[] buttons = selectionGO.GetComponentsInChildren<Button>();

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<TMP_Text>().text = selection[i].Name;
            int index = i;
            buttons[i].onClick.AddListener(() => SelectUpgrade(index));
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
        }
        PopulateSelection();
    }

    public void SelectUpgrade(int index)
    {
        selection[index].DoUpgrade();
        Button[] buttons = selectionGO.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.RemoveAllListeners();
        }
        selection.Clear();
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
