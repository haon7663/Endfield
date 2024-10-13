using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public class SkillLoader : MonoBehaviour
{
    public TextAsset skillCsv;
    public List<Skill> skills = new List<Skill>();

    private void Start()
    {
        LoadCSV();
    }

    private void LoadCSV()
    {
        var asd = skillCsv.ConvertCsvToJson();
        Debug.Log(asd.ToString());
        
        var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        
        {
            var skill = new Skill
            {
                name = "Fireball",
                elixir = 10
            };
            skill.skillComponents.Add(new AttackComponent { damage = 50, distance = 10 });
            skill.skillComponents.Add(new MoveComponent { distance = 10 });
            skills.Add(skill);
        }
        {
            var skill = new Skill
            {
                name = "IceSpike",
                elixir = 5
            };
            skill.skillComponents.Add(new AttackComponent { damage = 30, distance = 15 });
            skills.Add(skill);
        }
        {
            var skill = new Skill
            {
                name = "Dash",
                elixir = 4
            };
            skill.skillComponents.Add(new AttackComponent { damage = 5, distance = 12 });
            skills.Add(skill);
        }
        
        Debug.Log(JsonConvert.SerializeObject(skills, Formatting.Indented, settings));
        return;
        
        //var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        var skillJson = skillCsv.ConvertCsvToJson();

        var jsonAppend = "{\n  \"$type\": \"Skill, Assembly-CSharp\",\n  \"skillComponents\": {\n    \"$type\": \"System.Collections.Generic.List`1[[CherryEditor.SkillComponent, Assembly-CSharp]], mscorlib\",\n    \"$values\":";
        var jsonEnd = "} }";
            
        var sb = new StringBuilder();
        sb.Append(jsonAppend);
        sb.Append(skillJson);
        sb.Append(jsonEnd);
            
        var data = JsonConvert.DeserializeObject(sb.ToString(), settings);
        Debug.Log(skillJson);
            
        if (data is Skill skillData)
        {
            Debug.Log(skillData.name);
            Debug.Log(skillData.elixir);
            foreach (var skillComponent in skillData.skillComponents)
            {
            }
        }
    }
    
    private static DataTable ParseCsvToDataTable(string[] lines)
    {
        var dataTable = new DataTable();
        var headers = lines[0].Split(',');

        foreach (var header in headers)
        {
            dataTable.Columns.Add(header);
        }

        for (int i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            dataTable.Rows.Add(values);
        }

        return dataTable;
    }

    private List<Skill> TestCSV(DataTable table)
    {
        List<Skill> skills = new List<Skill>();
        string prevSkillName = null;

        foreach (DataRow row in table.Rows)
        {
            var skillName = row["skillName"].ToString();
            int.TryParse(row["elixir"].ToString(), out var elixir);

            if (string.IsNullOrEmpty(skillName))
            {
                skillName = prevSkillName;
                //skills.FirstOrDefault(s => s.name == skillName).skillComponents.Add();
            }

            var skill = skills.Find(s => s.name == skillName);
            if (skill == null)
            {
                skill = new Skill
                {
                    name = skillName,
                    elixir = elixir,
                    skillComponents = new List<SkillComponent>()
                };
                skills.Add(skill);
            }

            prevSkillName = skillName;
        }

        return skills;
    }
    
    //{
    //    "$type": "System.Collections.Generic.List`1[[Skill, Assembly-CSharp]], mscorlib",
    //    "$values": [
    //    {
    //        "$type": "Skill, Assembly-CSharp",
    //        "name": "Fireball",
    //        "elixir": 10,
    //        "skillComponents": {
    //            "$type": "System.Collections.Generic.List`1[[SkillComponent, Assembly-CSharp]], mscorlib",
    //            "$values": [
    //            {
    //                "$type": "AttackComponent, Assembly-CSharp",
    //                "damage": 50
    //            },
    //            {
    //                "$type": "MoveComponent, Assembly-CSharp",
    //                "distance": 10
    //            }
    //            ]
    //        }
    //    },
    //    {
    //        "$type": "Skill, Assembly-CSharp",
    //        "name": "IceSpike",
    //        "elixir": 5,
    //        "skillComponents": {
    //            "$type": "System.Collections.Generic.List`1[[SkillComponent, Assembly-CSharp]], mscorlib",
    //            "$values": [
    //            {
    //                "$type": "AttackComponent, Assembly-CSharp",
    //                "damage": 30
    //            },
    //            {
    //                "$type": "MoveComponent, Assembly-CSharp",
    //                "distance": 8
    //            }
    //            ]
    //        }
    //    }
    //    ]
    //}

}