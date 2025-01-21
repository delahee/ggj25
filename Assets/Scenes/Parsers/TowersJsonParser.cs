using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public static class TowersDataJsonParser
{
    public static List<Towers> ParseJson(string json)
    {
        List<Towers> towers = new List<Towers>();

        try
        {
            JObject data = JObject.Parse(json);
            JArray entries = (JArray)data["values"];

            for (int i = 1; i < entries.Count; i++)
            {
                var entry = entries[i].ToObject<List<string>>();
                if (entry.Count < 10  || "".Equals(entry[0]) || string.Empty.Equals(entry[0]))
                    continue;
                if ("EOF".Equals(entry[0]))
                    break;
                Towers tower = new Towers()
                {
                    id = entry[0],
                    name = entry[1],
                    priority = entry[2],
                    desc = entry[3],
                    vanilla = entry[4],
                    upgrade1 = entry[5],
                    upgrade2 = entry[6],
                    upgrade3 = entry[7],
                    upgrade4A = entry[8],
                    upgrade4B = entry[9]
                };
                towers.Add(tower);
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("Error parsing Towers JSON: " + ex.Message);
        }

        return towers;
    }
}