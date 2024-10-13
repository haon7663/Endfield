using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public static class CsvToJsonConverter
{
    public static string ConvertCsvToJson(this TextAsset textAsset)
    {
        var csvText = textAsset.text;
        
        string[] lines = csvText.Split('\n');
        
        string[] headers = lines[0].Split(',');

        List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();
        
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] values = lines[i].Split(',');
            Dictionary<string, string> data = new Dictionary<string, string>();

            for (int j = 0; j < headers.Length; j++)
            {
                data[headers[j].Trim()] = values[j].Trim();
            }

            dataList.Add(data);
        }
        
        return JsonConvert.SerializeObject(dataList, Formatting.Indented);
    }
}