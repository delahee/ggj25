using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Upgrade : MonoBehaviour 
{
    public string Name = "";
    public UpgradeType Type;
    public string Description = "";
    public string Effect;
    public bool IsUnique;
    public UpgradeDependence Dependence;
    public GameObject Prefab;
    public float PopCost;
    public float MeltCost;
    public float MithrilCost;
    public string Method;
    public EquipType EquipType;

    public Upgrade(string name, UpgradeType type)
    {
        Name = name;
        Type = type;
    }

    public Upgrade(string name, 
        UpgradeType type, 
        string description, 
        string effect, 
        bool isUnique, 
        UpgradeDependence dependence, 
        string method, 
        GameObject prefab = null, float popCost = 0, float meltCost = 0, float mithrilCost = 0, EquipType equipType = EquipType.Weapon) : this(name, type)
    {
        Description = description;
        Effect = effect;
        IsUnique = isUnique;
        Dependence = dependence;
        Prefab = prefab;
        PopCost = popCost;
        MeltCost = meltCost;
        MithrilCost = mithrilCost;
        Method = method;

        if (type == UpgradeType.Equipment)
            EquipType = equipType;
    }

    public Upgrade MakeItGO() 
    {
        var go = new GameObject();
        var up = go.AddComponent<Upgrade>();
        up.Name = Name;
        up.Type = Type;
        up.Description = Description;
        up.Effect = Effect;
        up.PopCost = PopCost;
        up.MeltCost = MeltCost;
        up.MithrilCost = MithrilCost;
        up.Method = Method;
        return up;
    }

    public void DoUpgrade()
    {
        if (UpgradeType.Hero.Equals(Type))
            HeroesManager.INSTANCE.SpawnHero(Name);
        else
        {
            var up = MakeItGO();

            if (Method != "")
                up.Invoke(Method,0f);
            else
                Debug.LogError($"No method given!");
            Destroy(up);
        }
        Destroy(UpgradeManager.Instance.selectionGO);
    }

    #region methods

    void AutoClick()
    {
        UpgradeManager.Imps += 1;
    }

    void Bps()
    {
        BubbleManager.PopRate += 1f;
    }

    void ElbowGrease()
    {
        BubbleManager.PopValue *= 1.20f;
    }

    #endregion
}

public class UpgradeDependence
{
    public DependsOnEquip DependsOnEquip;
    public DependsOnScience DependsOnScience;
    public DependsOnEco DependsOnEco;

    public string CheckDependence()
    {
        if(DependsOnEquip != DependsOnEquip.NONE)
            return DependsOnEquip.ToString();
        if (DependsOnScience != DependsOnScience.NONE)
            return DependsOnScience.ToString();
        if (DependsOnEco != DependsOnEco.NONE)
            return DependsOnEco.ToString();
        return "";
    }
}

public enum UpgradeType
{
    Equipment, //heroes, towers and their upgrades
    Volcano, //environment, unlocks or upgrades stats or equipments
    Economy, //capitalism
    Hero
}