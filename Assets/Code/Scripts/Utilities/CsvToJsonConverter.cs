using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public static class CsvToJsonConverter
{
    public static string ConvertCSVToJson(this TextAsset textAsset)
    {
        var csvText = textAsset.text;
        
        string[] lines = csvText.Split('\n');
        
        string[] headers = lines[0].Split(',');

        List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();
        var savedName = "";
        
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] values = lines[i].Split(',');
            Dictionary<string, string> data = new Dictionary<string, string>();
            
            var typeIndex = headers.ToList().FindIndex(h => h.Trim() == "$type");
            
            for (var j = typeIndex; j < headers.Length; j++)
            {
                data[headers[j].Trim()] = values[j].Trim();
                if (headers[j].Trim() == "$type")
                    data[headers[j].Trim()] += ", Assembly-Csharp";
            }

            {
                var nameIndex = headers.ToList().FindIndex(h => h.Trim() == "name");
                var saveIndex = headers.ToList().FindIndex(h => h.Trim() == "saveName");
                
                if (string.IsNullOrEmpty(values[nameIndex].Trim()))
                {
                    data[headers[saveIndex].Trim()] = savedName;
                }
                else
                {
                    var name = values[nameIndex].Trim();
                    data[headers[saveIndex].Trim()] = name;
                    savedName = name;
                }
            }
            dataList.Add(data);
        }
        
        return JsonConvert.SerializeObject(dataList, Formatting.Indented);
    }
}