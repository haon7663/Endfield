using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AIController : MonoBehaviour
{
    private Movement _movement;
    private Unit _unit;
    private SkillHolder _skillHolder;

    [SerializeField] private float skillCoolTime, moveCool, maxSkillCount;
    private float _curSkillCool, _curMoveCool;

    [SerializeField] private List<Skill> skills;
    [SerializeField] private List<float> skillCools = new List<float>();

    bool skillExecute;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _unit = GetComponent<Unit>();
        _skillHolder = GetComponent<SkillHolder>();
        _curSkillCool = skillCoolTime;
        _curMoveCool = moveCool;
        
        for (int i = 0; i < skills.Count;i++)
        {
            skillCools.Add(1);
        }
        SkillCoolTime();
    }

    private void Update()
    {
        if (!skillExecute)
        {
            UpdateCoolDown(ref _curMoveCool, moveCool, EnemyMove);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            SkillExcute();
        }
        
        if (Input.GetKeyDown(KeyCode.N))TryAddSkill();
    }
    
    private void SkillCoolTime()
    {
        for (int i = 0; i < skillCools.Count; i++)
        {
            int index = i;  
            DOVirtual.Float(1, 0, skills[index].castingTime, value =>
            {
                skillCools[index] = value; 
            });
        }
       
    }

    private void UpdateCoolDown(ref float currentCool, float maxCool, Action onCooldownComplete)
    {
        if (currentCool > 0)
            currentCool -= Time.deltaTime;
        else
        {
            onCooldownComplete?.Invoke();
            currentCool = maxCool;
        }
    }
    
    private void SkillExcute() //스킬 방출 실행
    {
        float excuteTime = 0;
        foreach (var skill in _skillHolder.castingViewers)
        {
            excuteTime +=skill.Data.castingTime;
        }
        StartCoroutine(_skillHolder.Execute());
        skillExecute = true;
       
        DOVirtual.DelayedCall(2f+excuteTime, () =>  
        {
            skillExecute = false;
            TryAddSkill();
        });
    }

    private void TryAddSkill() //머리 위에 스킬 보이기
    {
        for (int i = 0; i < skills.Count; i++)
        {
            if (skillCools[i] <= 0)
            {
                _skillHolder.AddCastingViewer(skills[i]);
                //SkillManager.Inst.ConsumeSkill(i);
                //skills[i] = SkillManager.Inst.GetSkillAtIndex(i);
                skillCools[i] = 1;
            }
        }

    }
    
    private void EnemyMove() // 적 -> 플레이어쪽으로 이동
    {
        if (_skillHolder.castingViewers.Count == 0) return;
        Debug.Log(_skillHolder.castingViewers[0].Data.SkillComponents);
        var target = GameManager.Inst.Player;
        Vector2 dir = SetDirection(target); //타겟이 오른쪽에 있는지
        if (dir.x != transform.localScale.x) _movement.OnFlip(Mathf.Approximately(transform.localScale.x, 1));
        
        int max = -100, min = 100;
        for(int i = 1; i < _skillHolder.castingViewers[0].Data.SkillComponents[0].distance; i++) //앞에 적이 있다면
        {
            var tileKey = _unit.Tile.Key + (i * _movement.DirX);
            Unit entity =  GridManager.Inst.GetTile(tileKey)?.content;
            if (entity != null && entity.TryGetComponent(out AIController aIController))
            {
                return;
            }
        }

        List<Tile> areaTile = target.Tile.GetAreaInRange(_skillHolder.castingViewers[0].Data.SkillComponents[0].distance);// 한 곳만 때리는거, 전체 때리는거 구분 해야함

        foreach (Tile tile in areaTile)
        {
            if (tile.Key == _unit.Tile.Key)
            {
                SkillExcute();
                return;
            }
        }
        //이미 범위 안에 플레이어가 있다면 스킬 시전
        //앞에 적이 있는지 없는지 감지해야 하는데 플레이어와 적을 구분하는게 없어 보임

        for(var i = 0; i < areaTile.Count; i++)  //가장 높은 값과 가장 낮은 값을 찾아냄
        {
            if (areaTile[i].Key > max) max = areaTile[i].Key;
            else if (areaTile[i].Key < min) min = areaTile[i].Key;
        }

        Vector2 tileDir = dir;

        var distance = tileDir.x>0?1:-1;
        if (GridManager.Inst.GetTile(_unit.Tile.Key + distance).content) return;   //앞에 무언가가 있는가
        StartCoroutine(_movement.OnMove(distance));
    }

    private bool MoveFarAI(bool targetIsRight, int min, int max)
    {
        if (targetIsRight) //멀리 가려는 AI
        {
            return min > _unit.Tile.Key;
        }
        else
        {
            return max > _unit.Tile.Key;
        }
    }

    private Vector2 SetDirection(Unit target)
    {
        float direction = target.transform.position.x - transform.position.x;
        return direction > 0 ? Vector2.right:Vector2.left;
    }
}
