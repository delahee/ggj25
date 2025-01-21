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

    int techLevel    = 0;
    int barrackLevel = 0;
    int ecoLevel     = 0;

    //TODO Add juice here =)
    public int TechLevel { get => techLevel; set => techLevel = value; }
    public int BarrackLevel { get => barrackLevel; set => barrackLevel = value; }
    public int EcoLevel { get => ecoLevel; set => ecoLevel = value; }

    List<Upgrade> techUpgrades      = new List<Upgrade>();
    List<Upgrade> barrackUpgrades   = new List<Upgrade>();
    List<Upgrade> ecoUpgrades       = new List<Upgrade>();

    //TODO populate with dev data
    void PopulateUpgrades()
    {
        techUpgrades.Add(new Upgrade("tech1"));
        techUpgrades.Add(new Upgrade("tech2"));
        techUpgrades.Add(new Upgrade("tech3"));
        techUpgrades.Add(new Upgrade("tech4"));

        barrackUpgrades.Add(new Upgrade("barrack1"));
        barrackUpgrades.Add(new Upgrade("barrack2"));
        barrackUpgrades.Add(new Upgrade("barrack3"));
        barrackUpgrades.Add(new Upgrade("barrack4"));

        ecoUpgrades.Add(new Upgrade("eco1"));
        ecoUpgrades.Add(new Upgrade("eco2"));
        ecoUpgrades.Add(new Upgrade("eco3"));
        ecoUpgrades.Add(new Upgrade("eco4"));
    }

    GameObject selectionGO;
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
        switch (type)
        {
            case "tech":
                techUpgrades.Shuffle();
                selection.Add(techUpgrades[0]);
                selection.Add(techUpgrades[1]);
                selection.Add(techUpgrades[2]);
                break;

            case "barrack":
                barrackUpgrades.Shuffle();
                selection.Add(barrackUpgrades[0]);
                selection.Add(barrackUpgrades[1]);
                selection.Add(barrackUpgrades[2]);
                break;
            
            case "eco":
                ecoUpgrades.Shuffle();
                selection.Add(ecoUpgrades[0]);
                selection.Add(ecoUpgrades[1]);
                selection.Add(ecoUpgrades[2]);
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
        Destroy(selectionGO);
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
