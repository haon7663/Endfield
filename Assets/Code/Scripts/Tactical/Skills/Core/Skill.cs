using System.Collections.Generic;

public class Skill
{
    public string Name;
    public List<SkillComponent> SkillComponents = new();  // 스킬을 구성하는 컴포넌트 리스트

    public void Use(Unit user)
    {
        foreach (var component in SkillComponents) 
        {
            component.Execute(user);  // 각 컴포넌트의 동작 실행
        }
    }
    
    public Skill(string name)
    {
        Name = name;
    }
}
