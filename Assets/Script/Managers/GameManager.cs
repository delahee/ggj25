using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance != null)
            Destroy(Instance.gameObject);

        instance = this;
        if (State == State.Starting || State == State.Playing)
        {
            InitGame();
        }
    }

    #endregion

    #region Game

    private float pops = 0f;
    private float melts = 0f;
    private float mithrils = 0f;

    public float Pops { 
        get => pops;
        set { pops = value; BubbleManager.Instance.PopText.text = value.ToString(); }
    }
    public float Melts { 
        get => melts;
        set { melts = value; BubbleManager.Instance.MeltText.text = value.ToString(); }
    }
    public float Mithrils { 
        get => mithrils;
        set { mithrils = value; BubbleManager.Instance.MithrilText.text = value.ToString(); }
    }

    public DataLists Data;

    private async Task InitGame()
    {
        //Init vars and game logic here
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