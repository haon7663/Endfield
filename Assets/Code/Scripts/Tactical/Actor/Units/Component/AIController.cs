using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private Movement _movement;
    private Unit _unit;
    private SkillHolder _skillHolder;
    [SerializeField] private float skillCoolTime,moveCool;
    private float _curSkillCool, _curMoveCool;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
        _unit = GetComponent<Unit>();
        _skillHolder = GetComponent<SkillHolder>();
        _curSkillCool = skillCoolTime;
        _curMoveCool = moveCool;
    }

    private void Update()
    {
           


        if (_curMoveCool > 0) _curMoveCool -= Time.deltaTime;
        else { EnemyMove(); _curMoveCool = moveCool; }

        if (_curSkillCool > 0) _curSkillCool -= Time.deltaTime;
        else { _skillHolder.AddCastingViewer(SkillManager.Inst.GetSkillAtIndex(0));_curSkillCool = 2f; }
            
     
    }
    
    private void EnemyMove() // 적 -> 플레이어쪽으로 이동
    {
        var target = GameManager.Inst.player;
        float direction = target.transform.position.x - transform.position.x;
        Debug.Log(direction);
        bool targetIsRight = direction > 0 ? true : false; //타겟이 오른쪽에 있는지
        bool tileIsRight;
        int max = -100, min = 100;


        //List<Tile> areaTile = target.Tile.GetAreaInRange(2);
        List<Tile> areaTile = target.Tile.GetTilesOnRange(2);
        foreach (Tile tile in areaTile) if (tile.Key == _unit.Tile.Key) return; //이미 범위 안에 있다면 리턴 및 스킬 시전

        for(var i = 0;i< areaTile.Count;i++)  //가장 높은 값과 가장 낮은 값을 찾아냄
        {
            if (areaTile[i].Key>max) max= areaTile[i].Key;
            else if (areaTile[i].Key<min) min= areaTile[i].Key;
        }

        if (targetIsRight) //멀리 가려는 AI
        {
            if (min > _unit.Tile.Key) tileIsRight = true;
            else tileIsRight = false;
        }
        else
        {
            if (max > _unit.Tile.Key) tileIsRight = true;
            else tileIsRight = false;
        }

        if (tileIsRight) _movement.OnMove(1);
        else _movement.OnMove(-1);
        
      
    }
}
