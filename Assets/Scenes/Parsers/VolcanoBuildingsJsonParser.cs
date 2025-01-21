using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public static class VolcanoBuildingsDataJsonParser
{
    public static List<VolcanoBuildings> ParseJson(string json)
    {
        List<VolcanoBuildings> volcanoBuildings = new List<VolcanoBuildings>();

        try
        {
            JObject data = JObject.Parse(json);
            JArray entries = (JArray)data["values"];

            for (int i = 1; i < entries.Count; i++)
            {
                var entry = entries[i].ToObject<List<string>>();
                if (entry.Count < 5 || "".Equals(entry[0]) || string.Empty.Equals(entry[0]))
                    continue;
                if ("EOF".Equals(entry[0]))
                    break;
                VolcanoBuildings volcanoBuilding = new VolcanoBuildings
                {
                    id = entry[0],
                    name = entry[1],
                    desc = entry[2],
                    effect = entry[3],
                    model = entry[4]
                };
                volcanoBuildings.Add(volcanoBuilding);
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("Error parsing Volcano Building JSON: " + ex.Message);
        }

        return volcanoBuildings;
    }
}