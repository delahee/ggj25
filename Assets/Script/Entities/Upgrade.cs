using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour 
{
    public string Name = "upgrade";

    public Upgrade(string name)
    {
        Name = name;
    }

    public void DoUpgrade()
    {
        Debug.Log($"{Name} upgrade !");
    }
}
