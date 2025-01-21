using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    #region Singleton

    private static BubbleManager instance;
    public static BubbleManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("UpgradeManager is null !!");

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

    #endregion

    #region Bubbules

    

    #endregion
}
