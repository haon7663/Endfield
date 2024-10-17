using System.Collections;
using System.Linq;
using UnityEngine;

public class SkillHolder : MonoBehaviour
{
    public UnitSO unitSO; 
    public float skillSelectCoolDown = 3f;

    private void Start()
    {
        StartCoroutine(SelectSkillRoutine());
    }

    private IEnumerator SelectSkillRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(skillSelectCoolDown);

            
            SkillSO selectedSkill = SelectSkill();

            if (selectedSkill != null)
            {
                Debug.Log(selectedSkill.name);
            }
        }
    }


    private SkillSO SelectSkill()
    {
        if (unitSO.skills == null || unitSO.skills.Count == 0) return null;
        
        int maxPriority = unitSO.skills.Max(skill => skill.priority);
      
        return unitSO.skills.Where(skill => skill.priority == maxPriority).ToList().Random(); 
    }
}
