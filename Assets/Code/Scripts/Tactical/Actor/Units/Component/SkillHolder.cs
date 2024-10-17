using NUnit.Framework;
using System.Collections;
using System.Linq;
using Unity.Android.Gradle;
using UnityEngine;
using System.Collections.Generic;

public class SkillHolder : MonoBehaviour
{
    public UnitSO unitSO; 
    public float skillSelectCoolDown = 3f;
    public List<SkillCastingViewer> castingViewers;
    public SkillCastingViewer castingViewerPrefab;
    public Transform skillCanvas;



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
                Vector3 spawnPosition = new Vector3(0, 150, 0);

                if (castingViewers.Count > 0)
                {
                    spawnPosition.y += castingViewers.Count * 70;
                }

                SkillCastingViewer newViewer = Instantiate(castingViewerPrefab, skillCanvas);
                newViewer.Init(selectedSkill, spawnPosition);
                castingViewers.Add(newViewer);
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
