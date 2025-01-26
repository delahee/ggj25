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
    public int hp = 6666;
    public float radius = 20;
    bool dead = false;
    public static GateOfHell instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

    }

    void Crash()
    {
        Utils.ForceCrash(ForcedCrashCategory.FatalError);
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
        if (hp <= 0) 
        {
            GameOver();
        }
        FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Generic/Enemy_Impact");
    }
    
}
