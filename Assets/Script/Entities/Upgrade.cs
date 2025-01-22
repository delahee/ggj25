using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour 
{
    public string Name = "";
    public UpgradeType Type;
    public string Description = "";
    public string Effect;
    public float popCost;
    public float meltCost;
    public float mithrilCost;

    public Upgrade(string name, UpgradeType type)
    {
        Name = name;
        Type = type;
    }

    public Upgrade(string name, UpgradeType type, string description, string effect, float popCost, float meltCost, float mithrilCost) : this(name, type)
    {
        Description = description;
        Effect = effect;
        this.popCost = popCost;
        this.meltCost = meltCost;
        this.mithrilCost = mithrilCost;
    }

    public void DoUpgrade()
    {
        Debug.Log($"{Name} upgrade !");
    }
}

public enum UpgradeType
{
    Equipment, //heroes, towers and their upgrades
    Volcano, //environment, unlocks or upgrades stats or equipments
    Economy //capitalism
}