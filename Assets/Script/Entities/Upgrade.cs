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
    public float PopCost;
    public float MeltCost;
    public float MithrilCost;
    public string Method;

    public Upgrade(string name, UpgradeType type)
    {
        Name = name;
        Type = type;
    }

    public Upgrade(string name, UpgradeType type, string description, string effect, float popCost, float meltCost, float mithrilCost, string method) : this(name, type)
    {
        Description = description;
        Effect = effect;
        PopCost = popCost;
        MeltCost = meltCost;
        MithrilCost = mithrilCost;
        Method = method;
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
        var up = MakeItGO();

        if (Method != "")
            up.Invoke(Method,0f);
        else
            Debug.LogError($"No method given!");

        Destroy(up);
        Destroy(UpgradeManager.Instance.selectionGO);
    }

    void BathTest()
    {
        BubbleManager.Instance.MeltAutoRate += 2;
    }
}

public enum UpgradeType
{
    Equipment, //heroes, towers and their upgrades
    Volcano, //environment, unlocks or upgrades stats or equipments
    Economy //capitalism
}