using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathCounterManager : MonoBehaviour
{
    private int deathcounter = 0;
    public TMP_Text DeathText;

    private static DeathCounterManager instance;
    public static DeathCounterManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("DeathCounterManager is null !!");

            return instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance != null)
            Destroy(Instance.gameObject);

        instance = this;
    }

    public int DeathCounter
    {
        get => deathcounter;
        set { deathcounter = value; DeathText.text = deathcounter.ToString(); }
    }

}
