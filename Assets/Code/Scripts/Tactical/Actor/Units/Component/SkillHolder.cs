using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillHolder : MonoBehaviour
{
    private Unit _unit;
    private Animator _animator;
    
    public List<SkillCastingViewer> castingViewers;
    public SkillCastingViewer castingViewerPrefab;
    public Transform skillCanvas;

    public List<Skill> skills;

    private const float InitialYPosition = 150f; // ��� ���� ��ġ
    private const float YOffset = 70f; // ���Ĥ��� �����Ǵ� ���� ���� ����
    
    private static readonly int Attack = Animator.StringToHash("attack");

    private void Start()
    {
        _unit = GetComponent<Unit>();
        _animator = _unit.SpriteTransform.GetComponent<Animator>();
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

    //모든 스킬 방출
    public IEnumerator Execute()
    {
        var saveCastingViewers = new List<SkillCastingViewer>();
        saveCastingViewers.AddRange(castingViewers);
        
        foreach (var castingViewer in saveCastingViewers)
        {
            yield return StartCoroutine(castingViewer.Cast());
            castingViewer.Data?.Use(_unit);
            _animator.SetTrigger(Attack);
            RemoveCastingViewer(castingViewer);
        }
    }

    public void AddCastingViewer(Skill skill)
    {
        if (GameManager.Inst.curElixir < skill.elixir) return;
        
        var spawnPosition = GetNextSpawnPosition(castingViewers.Count); 

        var newViewer = Instantiate(castingViewerPrefab, skillCanvas);
        newViewer.Init(skill, spawnPosition);

        castingViewers.Add(newViewer);

        GameManager.Inst.curElixir -= skill.elixir;
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
