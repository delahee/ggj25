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

    int POPframecount = 1;
    int MELTframecount = 1;
    int MITHRILframecount = 1;

    public List<GameObject> PopList = new List<GameObject>();
    public List<GameObject> MeltList = new List<GameObject>();
    public List<GameObject> MithrilList = new List<GameObject>();

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
        PopRoutine();
        GameManager.Instance.State = State.Playing;
        //InvokeRepeating(nameof(AutoRoutine), 1f, 1f);
    }

    private void FixedUpdate()
    {

        if (popAutoRate > 0)
        {
            POPframecount += 1;
            if (POPframecount > 60 * popAutoRate)
            {
                POPframecount = 1;
                if (PopList.Count > 0)
                {
                    GameObject go = PopList[Random.Range(0, PopList.Count)];
                    go.GetComponentInChildren<Button>().onClick.Invoke();
                }
            }
        }
        if (meltAutoRate > 0)
        {
            MELTframecount += 1;
            if (MELTframecount > 60 * meltAutoRate)
            {
                MELTframecount = 1;
                if (MeltList.Count > 0)
                {
                    GameObject go = MeltList[Random.Range(0, MeltList.Count)];
                    go.GetComponentInChildren<Button>().onClick.Invoke();
                }
            }
        }
        if (mithrilAutoRate > 0)
        {
            MITHRILframecount += 1;
            if (MITHRILframecount > 60 * mithrilAutoRate)
            {
                MITHRILframecount = 1;
                if (MithrilList.Count > 0)
                {
                    GameObject go = MithrilList[Random.Range(0, MithrilList.Count)];
                    go.GetComponentInChildren<Button>().onClick.Invoke();
                }
            }
        }
    }

    public void PopRoutine() 
    {   
        float a = 279;
        float b = 90;
        float x = Random.Range(-a, a);
        float y = Random.Range(2, Mathf.Sqrt((b * b) * (1 - ((x * x) / (a * a)))));
        GameObject go = Instantiate(GameManager.Instance.Data.Bubbles[0].prefab,
            //Spawns.transform.position + new Vector3(Random.Range(-400f, 400f), Random.Range(-50f, 50f), 0), 
            Spawns.transform.position + new Vector3(x, y, 0),
            Quaternion.identity, 
            Spawns.transform);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Volcano/Bubble_Spawn");
        go.GetComponentInChildren<Button>().onClick.AddListener(PopClick);
        Invoke(nameof(PopRoutine), (PopRate != 0) ? 1 / PopRate : 1f);
        PopList.Add(go);
    }

    void PopClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Volcano/Bubble_Explode");
        GameManager.Instance.Pops += PopValue;
        LavaSlider.Instance.addValue(PopValue);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Currency/Currency_Pops_Gain");
    }

    public void MeltRoutine()
    {
        GameObject go = Instantiate(GameManager.Instance.Data.Bubbles[1].prefab,
              Spawns.transform.position + new Vector3(Random.Range(-400f, 400f), Random.Range(-50f, 50f), 0),
            Quaternion.identity,
            transform);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Volcano/Bubble_Spawn");
        go.GetComponentInChildren<Button>().onClick.AddListener(MeltClick);
        Invoke(nameof(MeltRoutine), (MeltRate != 0) ? 1 / MeltRate : 1f);
        MeltList.Add(go);
    }

    void MeltClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Volcano/Bubble_Explode");
        GameManager.Instance.Melts += MeltValue;
        LavaSlider.Instance.addValue(MeltValue * 2);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Currency/Currency_Melts_Gain");
    }

    public void MithrilRoutine()
    {
        GameObject go = Instantiate(GameManager.Instance.Data.Bubbles[1].prefab,
             Spawns.transform.position + new Vector3(Random.Range(-400f, 400f), Random.Range(-50f, 50f), 0),
            Quaternion.identity,
            Spawns.transform);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Volcano/Bubble_Spawn");
        go.GetComponentInChildren<Button>().onClick.AddListener(MithrilClick);
        Invoke(nameof(MithrilRoutine), (MithrilRate != 0) ? 1 / MithrilRate : 1f);
        MithrilList.Add(go);
    }

    void MithrilClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Volcano/Bubble_Explode");
        GameManager.Instance.Mithrils += MithrilValue;
        LavaSlider.Instance.addValue(MithrilValue * 4);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Currency/Currency_Mithril_Gain");
    }

    /*void AutoRoutine()
    {
        GameManager.Instance.Pops += PopAutoRate * PopValue;
        GameManager.Instance.Melts += MeltAutoRate * MeltValue;
        GameManager.Instance.Mithrils += MithrilAutoRate * MithrilValue;
    }*/

    public void CalculateAutoRate(int imps)
    {
        if(imps <= 0) return;

        PopAutoRate = imps * PopValue;
        if (UpgradeManager.Instance.UniqueUpgrades.Contains("Beholder"))
            MeltAutoRate = imps * MeltValue;
        else
            MeltAutoRate = 0;
        if (UpgradeManager.Instance.UniqueUpgrades.Contains("AutomatedElves"))
            MithrilAutoRate = imps * MithrilValue;
        else
            MithrilAutoRate = 0;
    }
    #endregion
}
