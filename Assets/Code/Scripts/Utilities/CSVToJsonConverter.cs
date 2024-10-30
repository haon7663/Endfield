using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public static class CSVToJsonConverter
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
            
            var typeIndex = headers.Skip(1).ToList().FindIndex(h => h.Trim() == "$type") + 1;
            
            for (var j = typeIndex; j < headers.Length; j++)
            {
                data[headers[j].Trim()] = values[j].Trim();
                if (headers[j].Trim() == "$type")
                    if (data[headers[j].Trim()].EndsWith("Component"))
                        data[headers[j].Trim()] += ", Assembly-Csharp";
                    else
                        data[headers[j].Trim()] += "Component, Assembly-Csharp";
            }

            #region SaveName
            {
                var baseIndex = headers.ToList().FindIndex(h => h.Trim() == "name");
                var saveIndex = headers.ToList().FindIndex(h => h.Trim() == "saveName");
                
                if (string.IsNullOrEmpty(values[baseIndex].Trim()))
                {
                    data[headers[saveIndex].Trim()] = savedName;
                }
                else
                {
                    var name = values[baseIndex].Trim();
                    data[headers[saveIndex].Trim()] = name;
                    savedName = name;
                }
            }
            #endregion
            
            dataList.Add(data);
        }
        
        return JsonConvert.SerializeObject(dataList, Formatting.Indented);
    }
    
    public static string ConvertCSVToSkillJson(this TextAsset textAsset)
    {
        var csvText = textAsset.text;
        
        string[] lines = csvText.Split('\n');
        
        string[] headers = lines[0].Split(',');

        List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();
        
        for (int i = 1; i < lines.Length; i++)
        {
            Debug.Log(lines[i]);
            if (string.IsNullOrEmpty(lines[i])) continue;
            Debug.Log(lines[i]);
            
            string[] values = lines[i].Split(',');
            Dictionary<string, string> data = new Dictionary<string, string>();
            
            var typeIndex = headers.Skip(1).ToList().FindIndex(h => h.Trim() == "$type") + 1;
            
            for (var j = 0; j < typeIndex; j++)
            {
                data[headers[j].Trim()] = values[j].Trim();
                if (headers[j].Trim() == "$type")
                    data[headers[j].Trim()] += ", Assembly-Csharp";
            }

            if (string.IsNullOrEmpty(data["name"]))
                continue;
            dataList.Add(data);
        }
        
        return JsonConvert.SerializeObject(dataList, Formatting.Indented);
    }
}