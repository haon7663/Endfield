using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour
{
    private Movement _movement;
    private Unit _unit;
    private SkillHolder _skillHolder;

    [SerializeField] private float moveCool;
    private float _curMoveCool;

    [Serializable]
    private class SkillAndCool
    {
        public Skill skill;
        public float coolTime;

        public SkillAndCool(Skill skill, float coolTime)
        {
            this.skill = skill;
            this.coolTime = coolTime;
        }
    }
    private List<SkillAndCool> _skillAndCools;
    private bool _skillExecute;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _unit = GetComponent<Unit>();
        _skillHolder = GetComponent<SkillHolder>();
    }

    private void Start()
    {
        _curMoveCool = moveCool;
        SetSkillStartCool();
    }

    private void Update()
    {
        UpdateSkillCoolDown();
        if (!_skillExecute)
        {
            UpdateCoolDown(ref _curMoveCool, moveCool, EnemyMove);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            SkillExecute();
        }
        
        if (Input.GetKeyDown(KeyCode.N))
            TryAddSkill();
    }
    
    private void SetSkillStartCool()
    {
        _skillAndCools = new List<SkillAndCool>();
        foreach (var skill in _skillHolder.skills)
        {
            _skillAndCools.Add(new SkillAndCool(skill, Random.Range(0f, skill.elixir * 1.5f)));
            //랜덤으로 시작 쿨타임 잡음 0 ~ 엘릭서 * 1.5f
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

    private void UpdateSkillCoolDown()
    {
        foreach (var skillAndCool in _skillAndCools)
        {
            skillAndCool.coolTime -= Time.deltaTime;
        }
    }
    
    private void SkillExecute() //스킬 방출 실행
    {
        float executeTime = 0;
        foreach (var skillCastingViewer in _skillHolder.castingViewers)
        {
            executeTime += skillCastingViewer.Data.castingTime;
            _skillAndCools.FirstOrDefault(sc => sc.skill == skillCastingViewer.Data)!.coolTime =
                skillCastingViewer.Data.elixir * 1.5f;
        }
        StartCoroutine(_skillHolder.Execute());
        _skillExecute = true;

        DOVirtual.DelayedCall(2f + executeTime, () => { _skillExecute = false; });
    }

    private void TryAddSkill() //머리 위에 스킬 보이기
    {
        foreach (var skillAndCool in _skillAndCools.Where(skillAndCool => skillAndCool.coolTime <= 0))
        {
            _skillHolder.AddCastingViewer(skillAndCool.skill);
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
                SkillExecute();
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
        var direction = target.transform.position.x - transform.position.x;
        return direction > 0 ? Vector2.right:Vector2.left;
    }
}
