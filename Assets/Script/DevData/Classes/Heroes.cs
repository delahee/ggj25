
using UnityEngine;

[CreateAssetMenu(fileName = "New Heraut", menuName = "Heraut")]
public class Heroes : ScriptableObject
{
    public string id;
    public bool isPlayable;
    public bool isCommander;
    public string name;
    public string desc;
    public string fx;
    public int AtkDmgBasis;
    public int AtkCooldown;
    public int Range;
    public int hp;
    
}					 	