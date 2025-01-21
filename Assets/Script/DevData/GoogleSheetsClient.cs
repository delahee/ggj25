using System;
using System.Collections.Generic;
using System.Net.Http;
using FMOD;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GoogleSheetsClient : MonoBehaviour, IDataSubject
{
    [SerializeField] private GoogleSheetsConfig config;
    private readonly List<IDataObserver> observers = new List<IDataObserver>();

    private void Start()
    {
        FetchSheetData();
    }

    private async void FetchSheetData()
    {
        string url = $"https://sheets.googleapis.com/v4/spreadsheets/{config.sheetId}/values/{Uri.EscapeDataString(config.range)}?key={config.apiKey}";
        
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    List<Currencies> loreFacts = CharacterDataJsonParser.ParseJson(json);
                    NotifyObservers(loreFacts);
                }
                else
                {
                    Debug.LogError($"Data fetch failed. Status code: {response.StatusCode}. URL: {url}");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error encountered during data fetch: {ex.Message}");
        }
    }

    public void NotifyObservers(List<Currencies> currencies)
    {
        foreach (var observer in observers)
        {
            observer.OnDataReceived(currencies);
        }
    }

    public void AttachObserver(IDataObserver observer)
    {
        observers.Add(observer);
    }

    public void DetachObserver(IDataObserver observer)
    {
        observers.Remove(observer);
    }
}