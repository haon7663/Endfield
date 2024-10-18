using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillHolder : MonoBehaviour
{
    public List<SkillCastingViewer> castingViewers;
    public SkillCastingViewer castingViewerPrefab;
    public Transform skillCanvas;

    private const float InitialYPosition = 150f; // ��� ���� ��ġ
    private const float YOffset = 70f; // ���Ĥ��� �����Ǵ� ���� ���� ����

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

    public void AddCastingViewer(Skill skill)
    {
        var spawnPosition = GetNextSpawnPosition(castingViewers.Count); 

        var newViewer = Instantiate(castingViewerPrefab, skillCanvas);
        newViewer.Init(skill, spawnPosition);

        castingViewers.Add(newViewer);
    }

    // �ε����� ���� ��ġ ���
    private Vector3 GetNextSpawnPosition(int index)
    {
        Vector3 spawnPosition = new Vector3(0, InitialYPosition + (index * YOffset), 0);
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
}
