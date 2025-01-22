using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemies : ScriptableObject
{
    public GameObject prefab;
    [Space]
    public string id;
    public string tier;
    public string tribe;
    public string EnemyName;
    public string desc;
    public int hp;
    public int speed;
    public int dmg;
    public bool isElite;
}
