using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Bubbles : ScriptableObject
{
    public string bubbleName;
    public string desc;
    public BubbleType type;
    public GameObject prefab;
}

public enum BubbleType
{
    Pop,
    Melt,
    Mithril
}
