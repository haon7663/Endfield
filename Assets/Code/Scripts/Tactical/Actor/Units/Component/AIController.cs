using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour
{
    private Movement _movement;
    private Unit _unit;
    private SkillHolder _skillHolder;

    public float actionCool;
    [SerializeField]private float _curActionCool;

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

    private bool isActing;
    private bool _isActing 
    {
        get => isActing;
        set
        {
            isActing = value;
            if (!isActing)
            {
                _curActionCool = actionCool;
            }
        }
    }

   
   

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _unit = GetComponent<Unit>();
        _skillHolder = GetComponent<SkillHolder>();
        _unit.Health.onDeath+=()=> SpawnManager.Inst.EnemyDead();
    }

    private void Start()
    {
        _curActionCool = actionCool;
        SetSkillStartCool();
    }

    private void Update()
    {
        UpdateSkillCoolDown();
        if (!_isActing)
        {
            UpdateCoolDown(  actionCool, EnemyActing);
        }
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

    private void UpdateCoolDown( float maxCool, Action onCooldownComplete)
    {
        if (_curActionCool > 0)
            _curActionCool -= Time.deltaTime;
        else
        {
            onCooldownComplete?.Invoke();
            _curActionCool = maxCool;
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
            _skillAndCools.FirstOrDefault(sc => sc.skill == skillCastingViewer.Data)!.coolTime =
                skillCastingViewer.Data.elixir * 1.5f;
            executeTime += skillCastingViewer.Data.castingTime + 0.3f;
        }
        StartCoroutine(_skillHolder.Execute());
        _isActing = true;

        DOVirtual.DelayedCall(executeTime , () => {  _isActing = false; }).SetUpdate(false);
    }

    private IEnumerator TryAddSkill() //머리 위에 스킬 보이기
    {
        _isActing = true;
        foreach (var skillAndCool in _skillAndCools.Where(skillAndCool => skillAndCool.coolTime <= 0))
        {
            _skillHolder.AddCastingViewer(skillAndCool.skill);
            yield return new WaitForSeconds(0.4f);
        }
        _isActing = false;
    }


    private void EnemyActing() // 적 -> 플레이어쪽으로 이동
    {
        if(_isActing) Debug.Log("버그");
        if (_skillHolder.castingViewers.Count == 0)
        {
            StartCoroutine(TryAddSkill());
            return;
        }
        
        var target = GameManager.Inst.Player;
        var dirX = Mathf.Clamp(target.Tile.Key - _unit.Tile.Key, -1, 1);

        if (dirX != _movement.DirX)
        {
            StartCoroutine(_movement.OnFlip(Mathf.Approximately(transform.localScale.x, 1)));
            return;
        }
        
        int max = -100, min = 100;
        for(int i = 1; i < _skillHolder.castingViewers[0].Data.skillComponents[0].distance; i++) //앞에 적이 있다면
        {
            var tileKey = _unit.Tile.Key + (i * _movement.DirX);
            Unit entity =  GridManager.Inst.GetTile(tileKey)?.content;
            if (entity != null && entity.TryGetComponent(out AIController aIController))
            {
                return;
            }
        }

        List<Tile> areaTile = target.Tile.GetAreaInRange(_skillHolder.castingViewers[0].Data.skillComponents[0].distance);// 한 곳만 때리는거, 전체 때리는거 구분 해야함

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
        
        if (GridManager.Inst.GetTile(_unit.Tile.Key + dirX).content) return;   //앞에 무언가가 있는가
        StartCoroutine(_movement.OnMove(dirX));
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
}
