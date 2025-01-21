using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public static class HeroesDataJsonParser
{
    public static List<Heroes> ParseJson(string json)
    {
        List<Heroes> heroes = new List<Heroes>();

        try
        {
            JObject data = JObject.Parse(json);
            JArray entries = (JArray)data["values"];

            for (int i = 1; i < entries.Count; i++)
            {
                var entry = entries[i].ToObject<List<string>>();
                if (entry.Count < 10 || "".Equals(entry[0]) || string.Empty.Equals(entry[0]))
                    continue;
                if ("EOF".Equals(entry[0]))
                    break;
                Heroes hero = new Heroes
                {
                    id = entry[0],
                    isPlayable = entry[1],
                    isCommander = entry[2],
                    name = entry[3],
                    desc = entry[4],
                    effects = entry[5],
                    range = entry[6],
                    hp = entry[7],
                    cost = entry[8],
                    attackType = entry[9]
                };
                heroes.Add(hero);
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("Error parsing Heroes JSON: " + ex.Message);
        }

        return heroes;
    }
}