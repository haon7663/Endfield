using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillHolder : MonoBehaviour
{
    private Unit _unit;
    private Animator _animator;
    
    public SkillCastingViewer castingViewerPrefab;
    public Transform skillCanvas;

    public List<Skill> skills;
    public List<SkillCastingViewer> castingViewers;

    private const float InitialYPosition = 150f;
    private const float YOffset = 70f;
    
    private static readonly int Attack = Animator.StringToHash("attack");
    private static readonly int IsReady = Animator.StringToHash("isReady");

    private void Start()
    {
        _unit = GetComponent<Unit>();
        _animator = _unit.SpriteTransform.GetComponent<Animator>();
    }

    //모든 스킬 방출
    public IEnumerator Execute()
    {
        var saveCastingViewers = new List<SkillCastingViewer>();
        saveCastingViewers.AddRange(castingViewers);

        foreach (var castingViewer in saveCastingViewers)
        {
            if (castingViewer.Data == null) continue;

            SkillManager.Inst.ApplySkillArea(_unit, castingViewer.Data);

            _animator.SetBool(IsReady, true);
            yield return StartCoroutine(castingViewer.Cast());
            _animator.SetBool(IsReady, false);
            
            for (var i = 0; i < castingViewer.Data.executeCount; i++)
            {
                _animator.SetTrigger(Attack);
                castingViewer.Data.Use(_unit);
                yield return new WaitForSeconds(0.2f / castingViewer.Data.executeCount);
            }
            RemoveCastingViewer(castingViewer);
        }
    }

    public void AddCastingViewer(Skill skill)
    {
        var spawnPosition = GetNextSpawnPosition(castingViewers.Count); 

        var newViewer = Instantiate(castingViewerPrefab, skillCanvas);
        newViewer.Init(skill, spawnPosition);

        castingViewers.Add(newViewer);
    }
    
    private Vector3 GetNextSpawnPosition(int index)
    {
        Vector3 spawnPosition = new Vector3(0, InitialYPosition + (index * YOffset), 0);
        return spawnPosition;
    }
    
    public void RemoveCastingViewer(SkillCastingViewer viewer)
    {
        castingViewers.Remove(viewer);
        if (viewer)
            Destroy(viewer.gameObject);
        RepositionCastingViewers(); 
    }
    
    private void RepositionCastingViewers()
    {
        for (int i = 0; i < castingViewers.Count; i++)
        {
            Vector3 newPosition = GetNextSpawnPosition(i);
            castingViewers[i].rectTransform.anchoredPosition = newPosition; 
        }
    }
}
