using System.Collections.Generic;
using UnityEngine;

public class LoreFactsDisplay : MonoBehaviour, IDataObserver
{
    [SerializeField] private GoogleSheetsClient googleSheetsClient;

    private void Start()
    {
        googleSheetsClient.AttachObserver(this);
    }

    private void OnDestroy()
    {
        googleSheetsClient.DetachObserver(this);
    }

    public void OnDataReceived(List<Currencies> data)
    {
        foreach (var loreFact in data)
        {
            Debug.Log($"Id: {loreFact.id}, Fact: {loreFact.name}");
        }
    }
}