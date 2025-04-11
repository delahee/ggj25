using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Diagnostics;

public interface IHit
{
    void OnHit(int dmg);
}


public class GateOfHell : MonoBehaviour, IHit
{
    public int hp;
    int maxhp;
    public float radius = 20;
    bool dead = false;
    public static GateOfHell instance;
    public Material matbase;
    public Material matlight;
    public Material matdark;
    float basey;
    public float targety;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        maxhp = hp;
        basey = transform.position.y;
    }

    void Crash()
    {
        //Utils.ForceCrash(ForcedCrashCategory.FatalError);
        //Application.Quit();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }


    void GameOver()
    {
        dead = true;
        Crash();
        Debug.Log("GameOver");
    }

    public void OnHit(int dmg)
    {
        if (dead) return;
        hp-=dmg;
        float t = (float)hp / (float)maxhp;
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(targety, basey, t), transform.position.z);
        StartCoroutine("Blink");
        if (hp <= 0) 
        {
            GameOver();
        }
        FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Generic/Enemy_Impact");
    }

    IEnumerator Blink()
    {
        gameObject.GetComponent<MeshRenderer>().material = matlight;
        for (int i = 0; i < 3; i++) yield return new WaitForFixedUpdate();
        gameObject.GetComponent<MeshRenderer>().material = matdark;
        for (int i = 0; i < 3; i++) yield return new WaitForFixedUpdate();
        gameObject.GetComponent<MeshRenderer>().material = matbase;
    }
    
}
