using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemies : ScriptableObject
{
    public string id;
    public string tier;
    public string tribe;
    public string EnemyName;
    public string desc;
    public string fx;
    public int hp;
    public int speed;
    public int dmg;
    public bool isElite;
    public float proba;
}
