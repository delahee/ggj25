using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    public List<GameObject> fireBalls = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Heroes/Hero_Pyro_Firebolt");
        foreach (GameObject child in transform)
        {
            fireBalls.Add(child);
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            fireBalls.Add(fireBalls[i]);
            fireBalls[i].transform.DOMoveZ(transform.position.z * 1.5f, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
