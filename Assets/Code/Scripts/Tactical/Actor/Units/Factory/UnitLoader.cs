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
        
        var animatorController = Resources.Load<AnimatorOverrideController>(Path.Combine("Units/", unitName, unitName));
        var skills = SkillLoader.GetSkillsFromUnitName(dict["skillName"].ToString());
        var data = new UnitData(dict["name"].ToString(), animatorController, (int)dict["health"], (float)dict["actionTime"], skills);

        return data;
    }
    
    
}