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

    static float PopAutoRate = 0f;
    static float MeltAutoRate = 0f;
    static float MithrilAutoRate = 0f;

    public GameObject Spawns;

    private void Start()
    {
        PopRoutine();
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
        PopText.text = GameManager.Instance.Pops.ToString();
    }

    #endregion
}
