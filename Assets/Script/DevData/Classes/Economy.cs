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
    public List<string> dependence = new List<string>();
    public GameObject buildingPrefab;
    public float popCost;
    public float meltCost;
    public float mithrilCost;

    public string EffectMethodName;
}

public enum Depends
{
    NONE,
    Imps,
    IronMine,
    MithrilFactory
}