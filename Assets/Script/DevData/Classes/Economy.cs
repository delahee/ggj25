using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Economy : ScriptableObject
{
    public string ecoName;
    public string desc;
    public string fx;
    public bool isUnique;
    public DependsOnEco dependence;
    public GameObject buildingPrefab;
    public float popCost;
    public float meltCost;
    public float mithrilCost;
}

public enum DependsOnEco
{
    NONE,
    IMPS,
    IRON_MINE,
    MITHRIL_FACTORY
}