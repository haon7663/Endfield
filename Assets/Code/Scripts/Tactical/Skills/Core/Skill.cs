using System.Collections.Generic;

public class Skill
{
    public string name;
    public int elixir;
    public List<SkillComponent> skillComponents = new();  // 스킬을 구성하는 컴포넌트 리스트

    public void Use(Unit user)
    {
        foreach (var component in skillComponents) 
        {
            component.Execute(user);  // 각 컴포넌트의 동작 실행
        }
    }
}
