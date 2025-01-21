using System;
using System.Collections.Generic;
using System.Net.Http;
using FMOD;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GoogleSheetsClient : MonoBehaviour
{
    [SerializeField] private GoogleSheetsConfig config;
    private readonly List<IDataObserver> observers = new List<IDataObserver>();

    public List<Currencies> currencies ;
    public List<Enemies> enemies;
    public List<Heroes> heroes;
    public List<Towers> towers;
    public List<VolcanoBuildings> volcanoBuildings;
    
    
    
    private void Start()
    {
        FetchCurrenciesData();
        FetchEnemiesData();
        FetchHeroesData();
        FetchTowersData();
        FetchVolcanoBuildingsData();
        var toto = "";
    }

    private async void FetchCurrenciesData()
    {
        string url = $"https://sheets.googleapis.com/v4/spreadsheets/{config.sheetId}/values/{Uri.EscapeDataString(config.currenciesTab)}?key={config.apiKey}";
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    currencies = CurrenciesDataJsonParser.ParseJson(json);
                }
                else
                {
                    Debug.LogError($"Currencies Data fetch failed. Status code: {response.StatusCode}. URL: {url}");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error encountered during data fetch: {ex.Message}");
        }
    }
    
    private async void FetchEnemiesData()
    {
        string url = $"https://sheets.googleapis.com/v4/spreadsheets/{config.sheetId}/values/{Uri.EscapeDataString(config.enemiesTab)}?key={config.apiKey}";
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    enemies = EnemiesDataJsonParser.ParseJson(json);
                }
                else
                {
                    Debug.LogError($"Enemies Data fetch failed. Status code: {response.StatusCode}. URL: {url}");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error encountered during data fetch: {ex.Message}");
        }
    }
    
    private async void FetchHeroesData()
    {
        string url = $"https://sheets.googleapis.com/v4/spreadsheets/{config.sheetId}/values/{Uri.EscapeDataString(config.heroesTab)}?key={config.apiKey}";
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    heroes = HeroesDataJsonParser.ParseJson(json);
                }
                else
                {
                    Debug.LogError($"Heroes Data fetch failed. Status code: {response.StatusCode}. URL: {url}");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error encountered during data fetch: {ex.Message}");
        }
    }
    
    private async void FetchTowersData()
    {
        string url = $"https://sheets.googleapis.com/v4/spreadsheets/{config.sheetId}/values/{Uri.EscapeDataString(config.towersTab)}?key={config.apiKey}";
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    towers = TowersDataJsonParser.ParseJson(json);
                }
                else
                {
                    Debug.LogError($"Towers Data fetch failed. Status code: {response.StatusCode}. URL: {url}");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error encountered during data fetch: {ex.Message}");
        }
    }
    
    private async void FetchVolcanoBuildingsData()
    {
        string url = $"https://sheets.googleapis.com/v4/spreadsheets/{config.sheetId}/values/{Uri.EscapeDataString(config.volcanoBuildingsTab)}?key={config.apiKey}";
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    volcanoBuildings = VolcanoBuildingsDataJsonParser.ParseJson(json);
                }
                else
                {
                    Debug.LogError($"Volcanos Buildings Data fetch failed. Status code: {response.StatusCode}. URL: {url}");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error encountered during data fetch: {ex.Message}");
        }
    }
}