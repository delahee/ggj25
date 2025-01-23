using System;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Newtonsoft.Json.Linq;

public class BubbleManager : MonoBehaviour
{
    #region Singleton

    private static BubbleManager instance;
    public static BubbleManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("BubbleManager is null !!");

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

    #region Bubbles

    public TMP_Text PopText;
    public TMP_Text MeltText;
    public TMP_Text MithrilText;

    public TMP_Text PopAutoText;
    public TMP_Text MeltAutoText;
    public TMP_Text MithrilAutoText;

    //Value of a bubble
    public static float PopValue = 1f;
    public static float MeltValue = 0f;
    public static float MithrilValue = 0f;

    //Spawn time
    public static float PopRate = 1f;
    public static float MeltRate = 0f;
    public static float MithrilRate = 0f;

    //nuber of bubbles auto clicked (value shown to the player is autoReate * bubbleValue)
    float popAutoRate = 0f;
    float meltAutoRate = 0f;
    float mithrilAutoRate = 0f;

    public float PopAutoRate
    {
        get => Instance.popAutoRate;
        set { Instance.popAutoRate = value; Instance.PopAutoText.text = (value * PopValue).ToString(); }
    }
    public float MeltAutoRate
    {
        get => Instance.meltAutoRate;
        set { Instance.meltAutoRate = value; Instance.MeltAutoText.text = (value * MeltValue).ToString(); }
    }
    public float MithrilAutoRate
    {
        get => Instance.mithrilAutoRate;
        set { Instance.mithrilAutoRate = value;Instance.MithrilAutoText.text = (value * MithrilValue).ToString(); }
    }

    public GameObject Spawns;

    private void Start()
    {
        MeltAutoRate = 0.20f;
        PopRoutine();
        InvokeRepeating(nameof(AutoRoutine), 1f, 1f);
    }

    void PopRoutine() 
    {
        GameObject go = Instantiate(GameManager.Instance.Data.Bubbles[0].prefab, 
            Spawns.transform.position + new Vector3(Random.Range(-500f,500f), Random.Range(-500f, 500), 0), 
            Quaternion.identity, 
            Spawns.transform);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Volcano/Bubble_Spawn");
        go.GetComponentInChildren<Button>().onClick.AddListener(PopClick);
        Invoke(nameof(PopRoutine), PopRate);
    }

    void PopClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Volcano/Bubble_Explode");
        GameManager.Instance.Pops += PopValue;
        FMODUnity.RuntimeManager.PlayOneShot("event:/Currency/Currency_Pops_Gain");
    }

    void MeltRoutine()
    {
        GameObject go = Instantiate(GameManager.Instance.Data.Bubbles[1].prefab,
            Spawns.transform.position + new Vector3(Random.Range(-500f, 500f), Random.Range(-500f, 500), 0),
            Quaternion.identity,
            Spawns.transform);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Volcano/Bubble_Spawn");
        go.GetComponentInChildren<Button>().onClick.AddListener(MeltClick);
        Invoke(nameof(MeltRoutine), MeltRate);
    }

    void MeltClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Volcano/Bubble_Explode");
        GameManager.Instance.Melts += MeltValue;
        FMODUnity.RuntimeManager.PlayOneShot("event:/Currency/Currency_Melts_Gain");
    }

    void MithrilRoutine()
    {
        GameObject go = Instantiate(GameManager.Instance.Data.Bubbles[1].prefab,
            Spawns.transform.position + new Vector3(Random.Range(-500f, 500f), Random.Range(-500f, 500), 0),
            Quaternion.identity,
            Spawns.transform);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Volcano/Bubble_Spawn");
        go.GetComponentInChildren<Button>().onClick.AddListener(MithrilClick);
        Invoke(nameof(MithrilRoutine), MithrilRate);
    }

    void MithrilClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Volcano/Bubble_Explode");
        GameManager.Instance.Mithrils += MithrilValue;
        FMODUnity.RuntimeManager.PlayOneShot("event:/Currency/Currency_Mithril_Gain");
    }

    void AutoRoutine()
    {
        GameManager.Instance.Pops += PopAutoRate * PopValue;
        GameManager.Instance.Melts += MeltAutoRate * MeltValue;
        GameManager.Instance.Mithrils += MithrilAutoRate * MithrilValue;
    }

    public void CalculateAutoRate(int imps)
    {
        PopAutoRate = imps * PopValue;
        MeltAutoRate = imps * MeltValue;
        MithrilAutoRate = imps * MithrilValue;
    }
    #endregion
}
