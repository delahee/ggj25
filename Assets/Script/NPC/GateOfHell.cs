using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOfHell : MonoBehaviour
{
    public int hp = 10;
    public float radius = 10;
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

    public void OnHit()
    {
        if (dead) return;
        hp--;
        if (hp <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        dead = true;
        Debug.Log("GameOver");
        Application.Quit();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
