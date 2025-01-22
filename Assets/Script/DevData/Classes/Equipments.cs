using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Equipments : ScriptableObject
{
    public string equipName;
    public string desc;
    public EquipType equipType;
    public int weight;
    public DependsOnEquip dependence;
    public float popCost;
    public float meltCost;
    public float mithrilCost;
}

public enum EquipType
{
    Armor,
    Weapon
}

public enum DependsOnEquip
{
    NONE,
    DARK_MITHRIL_SWORD,
    MITHRIL_ARMOR
}