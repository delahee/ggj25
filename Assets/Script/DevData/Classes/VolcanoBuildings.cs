using UnityEngine;

[CreateAssetMenu]
public class VolcanoBuildings : ScriptableObject
{
    public string buidingName;
    public string desc;
    public string effect;
    public float popCost;
    public float meltCost;
    public float mithrilCost;
    public GameObject prefab;
}