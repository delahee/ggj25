using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;

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

    public static int   maxTurret           = 1;
    public static float boostTurretFireRate = 1;
    public static float boostHeroDmg        = 0;
    public static float boostHeroHP         = 0;
    public static float boostHeroRegen      = 0;
    public static int   turretShot          = 0;
    public static int   additionnalHeroSlot = 0;


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
        if (MeltCost > 0)
        {
            if (!dependence.Depends.Contains("Iron Mine"))
                dependence.Depends.Add("Iron Mine");
        }
        if(MithrilCost > 0)
        {
            if (!dependence.Depends.Contains("Mithril Factory"))
                dependence.Depends.Add("Mithril Factory");
        }

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

    public void DoUpgrade(bool skip = false)
    {
        if (skip)
            Destroy(UpgradeManager.Instance.selectionGO);

        if (UpgradeType.Hero.Equals(Type)) 
        {
            HeroesManager.INSTANCE.SpawnHero(Name);
        }
        else
        {
            var up = MakeItGO();

            if (IsUnique)
                UpgradeManager.Instance.UniqueUpgrades.Add(Name);

            if (Method != "")
            { 
                up.Invoke(Method, 0f);
                Debug.Log($"Execute {Method}");
            }
            /*else
                Debug.LogError($"No method given!");*/
            Destroy(up.transform.parent);
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

    void ElvenLoveMachine()
    {
        BubbleManager.MithrilRate *= 1.50f;
    }

    void ElvenLubricant()
    {
        BubbleManager.MithrilValue *= 2f;
    }

    void FurnaceOverload()
    {
        BubbleManager.MeltRate *= 1.50f;
    }

    void GoldPlatedFurnace()
    {
        BubbleManager.MeltValue *= 2f;
    }

    void Imps()
    {
        UpgradeManager.Imps = 1;
    }

    void ImpSpeed()
    {
        UpgradeManager.Imps *= 2;
    }

    void IronMine()
    {
        BubbleManager.MeltRate = 1f;
        BubbleManager.MeltValue = 1f;
        BubbleManager.Instance.MeltRoutine();
    }

    void MithrilFactory()
    {
        BubbleManager.MithrilRate = 1f;
        BubbleManager.MithrilValue = 1f;
        BubbleManager.Instance.MithrilRoutine();
    }

    void Sustainer()
    {
        BubbleManager.PopValue *= 2f;
    }

    void Aerodynamic()
    {
        //TODO Emile : black smith land one more turret
        maxTurret++;
    }

    void Barking()
    {
        //TODO Emile : black smith's turret shoots 20% faster
        boostTurretFireRate += .2f;
    }

    void Enhancer()
    {
        //TODO Emile : all heroes do 25% more damage
        boostHeroDmg += .25f;
    }

    void Hammergedon()
    {
        //TODO Emile : Immediately deal 50% of their health as damage to each enemy on screen.
        foreach (Enemy e in FindObjectsOfType<Enemy>())
        {
            if (e.dead) continue;
            e.OnHit(e.hp / 2);
        }
    }

    void Lounge()
    {
        //TODO Emile : Add 2 more hero slots
        additionnalHeroSlot += 2;
    }

    void Masseur()
    {
        //TODO Emile : Heroes regen 20% faster
        boostHeroRegen += .2f;
    }

    void Nova()
    {
        //TODO Emile : Heroes all do twice the damage
        boostHeroDmg *= 2;
    }

    void Heart()
    {
        //TODO Emile : Heroes all gain +50% HP
        boostHeroHP += .5f;
    }

    void Pill()
    {
        //TODO Emile : Heroes all do 50% more damage
        boostHeroDmg += .5f;
    }

    void Shotgun()
    {
        //TODO Emile : Blacksmith turrets now shoot two more projectiles in a cone.
        turretShot += 2;
    }
    #endregion
}

public class UpgradeDependence
{
    public List<string> Depends;
    //public List<DependsOnScience> DependsOnScience;
    //public List<DependsOnEco> DependsOnEco;

    public List<string> CheckDependence()
    {
        /* if(DependsOnEquip.Count != 0) 
         {
             List<string> dependences = new List<string>();
             foreach(DependsOnEquip a in DependsOnEquip)
             {
                 dependences.Add(a.ToString());
             }

             return dependences;
         }
             //return DependsOnEquip.ToString();
         if (DependsOnScience.Count != 0) 
         {
             List<string> dependences = new List<string>();
             foreach (DependsOnScience a in DependsOnScience)
             {
                 dependences.Add(a.ToString());
             }
             return dependences;
         }
             //return DependsOnScience.ToString();
         if (DependsOnEco.Count != 0) 
         {
             List<string> dependences = new List<string>();
             foreach (DependsOnEco a in DependsOnEco)
             {
                 dependences.Add(a.ToString());
             }
             return dependences;
         }*/
        /*if (Depends.Count != 0)
        {
            List<string> dependences = new List<string>();
            foreach (string a in Depends)
            {
                dependences.Add(a.Name);
            }

            return dependences;
        }*/
        //return DependsOnEco.ToString();
        return Depends;
    }
}

public enum UpgradeType
{
    Equipment, //heroes, towers and their upgrades
    Volcano, //environment, unlocks or upgrades stats or equipments
    Economy, //capitalism
    Hero
}