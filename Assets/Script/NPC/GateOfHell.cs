using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOfHell : MonoBehaviour
{
    public int hp = 10;
    bool dead = false;

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
}
