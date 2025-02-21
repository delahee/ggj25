using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("GM is null !!");

            return instance;
        }
    }

    static FMOD.Studio.EventInstance instanceAmb, music;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance != null)
        {
            /*music.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            instanceAmb.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);*/
            Destroy(Instance.gameObject);
        }

        instance = this;
        if (State == State.Starting || State == State.Playing)
        {
            InitGame();
        }

        SetMusic();
    }

    public void SetMusic()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            instanceAmb = RuntimeManager.CreateInstance("event:/Ambience/Amb_Inferno");
            //instance.set3DAttributes(RuntimeUtils.To3DAttributes(transposition));
            instanceAmb.start();
            instanceAmb.release();
            music = RuntimeManager.CreateInstance("event:/Music/Music_Game");
            music.start();
            music.release();
            //FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Ambience/Amb_Inferno", gameObject);
            //FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Music/Music_Game", gameObject);
        }
        else
        {
            music = RuntimeManager.CreateInstance("event:/Music/Music_Intro");
            music.start();
            music.release();
            //FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Music/Music_Intro", gameObject);
        }
    }

    private void OnDestroy()
    {
/*        music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instanceAmb.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);*/
    }

    #endregion

    #region Game

    private float pops = 0f;
    private float melts = 0f;
    private float mithrils = 0f;
    private Color PBC = new Color();
    private Color MeBC = new Color();
    private Color MiBC = new Color();

    public float Pops { 
        get => pops;
        set 
        {
            bool change = false;
            bool minoring = false;
            string prevtext = BubbleManager.Instance.PopText.text;
            if (Mathf.RoundToInt(pops) != Mathf.RoundToInt(value)) change = true;
            if (Mathf.RoundToInt(pops) > Mathf.RoundToInt(value)) minoring = true;
            pops = value; 
            BubbleManager.Instance.PopText.text = Mathf.RoundToInt(value).ToString(); 
            UpgradeManager.Instance.CalculateUpgradePrice("None");
            if (change)
            {
                ShakeManager.Instance.ObjectShaker(GameObject.Find("PopHolder").transform, new Vector3(1, 0, 0), 10, 3, 2);
                if(minoring) StartCoroutine(Redening(BubbleManager.Instance.PopText, PBC));
                int maxlength = Mathf.Max(BubbleManager.Instance.PopText.text.Length, prevtext.Length);
                var foundIndexes = new List<int>();
                for (int i = 0; i < maxlength; i++)
                {
                    if (i >= prevtext.Length || (i < BubbleManager.Instance.PopText.text.Length && prevtext[i]!= BubbleManager.Instance.PopText.text[i]))
                        foundIndexes.Add(i);
                }               
                if (foundIndexes.Count > 0)
                {
                    for(int i = 0; i<foundIndexes.Count; i++)
                    {
                        int j = Mathf.Abs(foundIndexes[i] - BubbleManager.Instance.PopText.text.Length);
                        BubbleManager.Instance.PopText.GetComponent<CurrencyJuice>().TextShaker(j, new Vector3(0, 1, 0), 25, 3, 1.5f);
                    }
                }
            }
        }
    }
    public float Melts { 
        get => melts;
        set 
        {
            bool change = false;
            bool minoring = false;
            string prevtext = BubbleManager.Instance.MeltText.text;
            if (Mathf.RoundToInt(melts) != Mathf.RoundToInt(value)) change = true;
            if (Mathf.RoundToInt(melts) > Mathf.RoundToInt(value)) minoring = true;
            melts = value; 
            BubbleManager.Instance.MeltText.text = Mathf.RoundToInt(value).ToString();
            if (change)
            {
                ShakeManager.Instance.ObjectShaker(GameObject.Find("MeltHolder").transform, new Vector3(1, 0, 0), 10, 3, 2);
                if (minoring) StartCoroutine(Redening(BubbleManager.Instance.MeltText, MeBC));
                int maxlength = Mathf.Max(BubbleManager.Instance.MeltText.text.Length, prevtext.Length);
                var foundIndexes = new List<int>();
                for (int i = 0; i < maxlength; i++)
                {
                    if (i >= prevtext.Length || (i < BubbleManager.Instance.MeltText.text.Length && prevtext[i] != BubbleManager.Instance.MeltText.text[i]))
                        foundIndexes.Add(i);
                }
                if (foundIndexes.Count > 0)
                {
                    for (int i = 0; i < foundIndexes.Count; i++)
                    {
                        int j = Mathf.Abs(foundIndexes[i] - BubbleManager.Instance.MeltText.text.Length);
                        BubbleManager.Instance.MeltText.GetComponent<CurrencyJuice>().TextShaker(j, new Vector3(0, 1, 0), 25, 3, 1.5f);
                    }
                }
            }
        }
    }
    public float Mithrils { 
        get => mithrils;
        set 
        {
            bool change = false;
            bool minoring = false;
            string prevtext = BubbleManager.Instance.MithrilText.text;
            if (Mathf.RoundToInt(mithrils) != Mathf.RoundToInt(value)) change = true;
            if (Mathf.RoundToInt(mithrils) > Mathf.RoundToInt(value)) minoring = true;
            mithrils = value; 
            BubbleManager.Instance.MithrilText.text = Mathf.RoundToInt(value).ToString();
            if (change)
            {
                ShakeManager.Instance.ObjectShaker(GameObject.Find("MithrilHolder").transform, new Vector3(1, 0, 0), 10, 3, 2);
                if (minoring) StartCoroutine(Redening(BubbleManager.Instance.MithrilText, MiBC));
                int maxlength = Mathf.Max(BubbleManager.Instance.MithrilText.text.Length, prevtext.Length);
                var foundIndexes = new List<int>();
                for (int i = 0; i < maxlength; i++)
                {
                    if (i >= prevtext.Length || (i < BubbleManager.Instance.MithrilText.text.Length && prevtext[i] != BubbleManager.Instance.MithrilText.text[i]))
                        foundIndexes.Add(i);
                }
                if (foundIndexes.Count > 0)
                {
                    for (int i = 0; i < foundIndexes.Count; i++)
                    {
                        int j = Mathf.Abs(foundIndexes[i] - BubbleManager.Instance.MithrilText.text.Length);
                        BubbleManager.Instance.MithrilText.GetComponent<CurrencyJuice>().TextShaker(j, new Vector3(0, 1, 0), 25, 3, 1.5f);
                    }
                }
            }
        }
    }

    IEnumerator Redening(TMP_Text text, Color precolor)
    {
        text.color = new Color(215, 0, 0);
        yield return new WaitForSeconds(1);
        text.color = precolor;
    }

    public DataLists Data;

    private async Task InitGame()
    {
        //UpgradeManager.Instance.CalculateUpgradePrice("None");
        Debug.Log("Test");
        HeroesManager.INSTANCE.SpawnHero("SMITH");
        PBC = BubbleManager.Instance.PopText.color;
        MeBC = BubbleManager.Instance.MeltText.color;
        MiBC = BubbleManager.Instance.MithrilText.color;
    }

    #endregion

    #region StateMachine


    private State state = State.MainMenu;
    public State State
    {
        get
        {
            return state;
        }
        set
        {
            OnStateChanged(value);
        }
    }

    //TODO
    private async void OnStateChanged(State value)
    {
        switch (value)
        {
            case State.MainMenu:
                SceneManager.LoadScene("MainMenu");
                break;

            case State.Starting:
                SceneManager.LoadScene("Game");
                break;

            case State.Playing:
                await InitGame();
                break;

            case State.Finishing:
                break;

            case State.Upgrading:
                SceneManager.LoadScene("UpgradingMenu");
                break;
        }

        state = value;
    }

    //Be carfull, might break :innocent:
    private async Task LoadAsyncGameScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            await Task.Yield();
        }
        await Task.Delay(300);
        await InitGame();
        //Debug.Log(TargetManager);
    }

    public void SetState(int stateName)
    {
        State = (State)stateName;
    }

    #endregion

    #region Save

    public void ResetSave()
    {
        PlayerPrefs.DeleteAll();
    }

    public void SaveValue(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }
    /*
    public void SaveValue(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }
    */
    public int GetSavedValue(string key)
    {
        return PlayerPrefs.GetInt(key);
    }

    public float GetSavedFloat(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }

    #endregion
}

//TODO
public enum State
{
    MainMenu,
    Starting,
    Playing,
    Finishing,
    Upgrading
}