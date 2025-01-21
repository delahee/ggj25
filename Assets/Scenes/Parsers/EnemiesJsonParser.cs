using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public static class EnemiesDataJsonParser
{
    public static List<Enemies> ParseJson(string json)
    {
        List<Enemies> enemies = new List<Enemies>();

        try
        {
            JObject data = JObject.Parse(json);
            JArray entries = (JArray)data["values"];

            for (int i = 1; i < entries.Count; i++)
            {
                var entry = entries[i].ToObject<List<string>>();
                if (entry.Count < 6 || "".Equals(entry[0]) || string.Empty.Equals(entry[0]))
                    continue;
                if ("EOF".Equals(entry[0]))
                    break;
                Enemies enemy = new Enemies
                {
                    id = entry[0],
                    tier =  entry[1],
                    tribe = entry[2],
                    name = entry[3],
                    desc = entry[4],
                    hp = entry[5]
                };
                enemies.Add(enemy);
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("Error parsing Enemies JSON: " + ex.Message);
        }

        return enemies;
    }
}