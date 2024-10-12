using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{

    public List<SkillSO> realSkills;
    List<SkillSO> skillBuffer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetupSKillBuffer();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) PopSKill();
    }


    //어떻게 사용될지는 모르겠지만 필요하면 더 추가해서 활용하셈
    public void PopSKill() //스킬 사용 
    {
        if(skillBuffer.Count ==0)SetupSKillBuffer();

        SkillSO skill= skillBuffer[0];
        skillBuffer.RemoveAt(0);
        //return skill;
    }

    void SetupSKillBuffer()  //스킬 랜덤 재배치
    {
        skillBuffer= new List<SkillSO>();

        for(int i = 0; i < realSkills.Count; i++)
        {
            SkillSO skill = realSkills[i];
            skillBuffer.Add(skill);
        }

        for(int i = 0; i< skillBuffer.Count; i++)
        {
            int random = Random.Range(i,skillBuffer.Count);
            SkillSO temp = skillBuffer[i];
            skillBuffer[i] = skillBuffer[random];
            skillBuffer[random] = temp;
        }
    }

    
}
