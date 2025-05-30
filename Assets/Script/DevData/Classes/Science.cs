using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Science : ScriptableObject
{
    public string scienceName;
    public string desc;
    public string fx;
    public bool isUnique;
    public List<string> dependence = new List<string>();
    public GameObject prefab;
    public float popCost;
    public float meltCost;
    public float mithrilCost;
    
    public string EffectMethodName;
}

public enum DependsOnScience
{
    NONE,
    IRON_MINE,
    MITHRIL_FACTORY
}