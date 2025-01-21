using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public static class CharacterDataJsonParser
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
                if ("EOF".Equals(entry[0]))
                    break;
                Currencies character = new Currencies
                {
                    id = entry[0],
                    name = entry[1],
                    bubble_type = entry[2],
                    gp_desc = entry[3]

                };
                currencies.Add(character);
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("Error parsing JSON: " + ex.Message);
        }

        return currencies;
    }
}