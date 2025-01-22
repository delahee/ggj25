using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    #region Bubbles

    public TMP_Text PopText;
    public TMP_Text MeltText;
    public TMP_Text MithrilText;

    public TMP_Text PopAutoText;
    public TMP_Text MeltAutoText;
    public TMP_Text MithrilAutoText;

    static float PopValue = 1f;
    static float MeltValue = 0f;
    static float MithrilValue = 0f;

    static float PopRate = 1f;
    static float MeltRate = 0f;
    static float MithrilRate = 0f;

    float popAutoRate = 0f;
    float meltAutoRate = 0f;
    float mithrilAutoRate = 0f;

    public static float PopAutoRate
    {
        get => Instance.popAutoRate;
        set { Instance.popAutoRate = value; Instance.PopAutoText.text = value.ToString(); }
    }
    public float MeltAutoRate
    {
        get => Instance.meltAutoRate;
        set { Instance.meltAutoRate = value; Instance.MeltAutoText.text = value.ToString(); }
    }
    public float MithrilAutoRate
    {
        get => Instance.mithrilAutoRate;
        set { Instance.mithrilAutoRate = value;Instance.MithrilAutoText.text = value.ToString(); }
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
        go.GetComponentInChildren<Button>().onClick.AddListener(PopClick);
        Invoke(nameof(PopRoutine), PopRate);
    }

    void PopClick()
    {
        GameManager.Instance.Pops += PopValue;
    }

    void MeltRoutine()
    {
        GameObject go = Instantiate(GameManager.Instance.Data.Bubbles[1].prefab,
            Spawns.transform.position + new Vector3(Random.Range(-500f, 500f), Random.Range(-500f, 500), 0),
            Quaternion.identity,
            Spawns.transform);
        go.GetComponentInChildren<Button>().onClick.AddListener(MeltClick);
        Invoke(nameof(MeltRoutine), MeltRate);
    }

    void MeltClick()
    {
        GameManager.Instance.Melts += MeltValue;
    }

    void MithrilRoutine()
    {
        GameObject go = Instantiate(GameManager.Instance.Data.Bubbles[1].prefab,
            Spawns.transform.position + new Vector3(Random.Range(-500f, 500f), Random.Range(-500f, 500), 0),
            Quaternion.identity,
            Spawns.transform);
        go.GetComponentInChildren<Button>().onClick.AddListener(MithrilClick);
        Invoke(nameof(MithrilRoutine), MithrilRate);
    }

    void MithrilClick()
    {
        GameManager.Instance.Mithrils += MithrilValue;
    }

    void AutoRoutine()
    {
        GameManager.Instance.Pops += PopAutoRate;
        GameManager.Instance.Melts += MeltAutoRate;
        GameManager.Instance.Mithrils += MithrilAutoRate;
    }
    #endregion
}
