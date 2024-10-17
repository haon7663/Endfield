using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillHolder : MonoBehaviour
{
    public UnitSO unitSO;
    public float skillSelectCoolDown = 3f;
    public List<SkillCastingViewer> castingViewers;
    public SkillCastingViewer castingViewerPrefab;
    public Transform skillCanvas;

    private float initialYPosition = 150f; // 뷰어 생성 위치
    private float yOffset = 70f; // 이후ㅎ에 생성되는 뷰어들 간의 간격

    private void Start()
    {
        StartCoroutine(SelectSkillRoutine());
    }

    private void Update()
    {
        // W 키를 눌렀을 때 리스트의 0번 인덱스 제거
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (castingViewers.Count > 0)
            {
                RemoveCastingViewer(castingViewers[0]); // 0번 인덱스의 뷰어 제거
            }
        }
    }


    private System.Collections.IEnumerator SelectSkillRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(skillSelectCoolDown);

            SkillSO selectedSkill = SelectSkill();

            if (selectedSkill != null)
            {
                Vector3 spawnPosition = GetNextSpawnPosition(castingViewers.Count); 

                SkillCastingViewer newViewer = Instantiate(castingViewerPrefab, skillCanvas);
                newViewer.Init(selectedSkill, spawnPosition);

                castingViewers.Add(newViewer);
            }
        }
    }

    // 인덱스에 따라서 위치 계산
    private Vector3 GetNextSpawnPosition(int index)
    {
        Vector3 spawnPosition = new Vector3(0, initialYPosition + (index * yOffset), 0);
        return spawnPosition;
    }

    // 리스트에서 뷰어가 제거될 때 호출할 함수(스킬 사용시)
    public void RemoveCastingViewer(SkillCastingViewer viewer)
    {
        castingViewers.Remove(viewer);
        Destroy(viewer.gameObject);
        RepositionCastingViewers(); 
    }

    // 뷰어들 위치 재조정 시켜줌
    private void RepositionCastingViewers()
    {
        for (int i = 0; i < castingViewers.Count; i++)
        {
            Vector3 newPosition = GetNextSpawnPosition(i);
            castingViewers[i].rectTransform.anchoredPosition = newPosition; 
        }
    }

    private SkillSO SelectSkill()
    {
        if (unitSO.skills == null || unitSO.skills.Count == 0) return null;

        int maxPriority = unitSO.skills.Max(skill => skill.priority);

        return unitSO.skills.Where(skill => skill.priority == maxPriority).ToList().Random();
    }
}
