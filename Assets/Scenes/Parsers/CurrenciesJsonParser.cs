using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public static class CurrenciesDataJsonParser
{
    public static List<Currencies> ParseJson(string json)
    {
        List<Currencies> currencies = new List<Currencies>();

        try
        {
            JObject data = JObject.Parse(json);
            JArray entries = (JArray)data["values"];

            for (int i = 1; i < entries.Count; i++)
            {
                var entry = entries[i].ToObject<List<string>>();
                if (entry.Count < 4 || "".Equals(entry[0]) || string.Empty.Equals(entry[0]))
                    continue;
                if ("EOF".Equals(entry[0]))
                    break;
                Currencies currency = new Currencies
                {
                    id = entry[0],
                    name = entry[1],
                    bubbleType = entry[2],
                    gpDesc = entry[3]
                };
                currencies.Add(currency);
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("Error parsing Currencies JSON: " + ex.Message);
        }

        return currencies;
    }
}