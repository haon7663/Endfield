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

    private float initialYPosition = 150f; // ��� ���� ��ġ
    private float yOffset = 70f; // ���Ĥ��� �����Ǵ� ���� ���� ����

    private void Start()
    {
        StartCoroutine(SelectSkillRoutine());
    }

    private void Update()
    {
        // W Ű�� ������ �� ����Ʈ�� 0�� �ε��� ����
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (castingViewers.Count > 0)
            {
                RemoveCastingViewer(castingViewers[0]); // 0�� �ε����� ��� ����
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

    // �ε����� ���� ��ġ ���
    private Vector3 GetNextSpawnPosition(int index)
    {
        Vector3 spawnPosition = new Vector3(0, initialYPosition + (index * yOffset), 0);
        return spawnPosition;
    }

    // ����Ʈ���� �� ���ŵ� �� ȣ���� �Լ�(��ų ����)
    public void RemoveCastingViewer(SkillCastingViewer viewer)
    {
        castingViewers.Remove(viewer);
        Destroy(viewer.gameObject);
        RepositionCastingViewers(); 
    }

    // ���� ��ġ ������ ������
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
