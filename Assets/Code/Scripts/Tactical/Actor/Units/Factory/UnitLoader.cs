using System.Collections.Generic;
using System.IO;
using UnityEditor.Animations;
using UnityEngine;

public static class UnitLoader
{
    public static UnitData GetUnitData(string unitName)
    {
        var unitCSV = Resources.Load<TextAsset>("UnitData/unit");
        return GetUnitData(unitCSV, unitName);
    }
    
    private static UnitData GetUnitData(TextAsset csv, string unitName)
    {
        var list = CSVReader.Read(csv);

        var dict = list.Find(item => item["name"].ToString() == unitName);

        var skills = new List<Skill>();
        var index = list.FindIndex(d => d == dict);
        
        Debug.Log(index);
        while (index < list.Count - 1 && !string.IsNullOrEmpty(list[index]["skillName"].ToString()))
        {
            var skillName = list[index]["skillName"].ToString();
            skills.Add(SkillLoader.GetSkillFromUnitName(skillName));

            index++;
        }
        
        var animatorController = Resources.Load<AnimatorOverrideController>(Path.Combine("Units/", unitName, unitName));
        var data = new UnitData(dict["name"].ToString(), animatorController, (int)dict["health"], (float)dict["actionTime"], skills);

        return data;
    }
    
    
}