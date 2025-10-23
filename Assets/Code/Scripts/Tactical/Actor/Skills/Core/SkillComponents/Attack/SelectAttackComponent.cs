using System.Collections;
using System.Collections.Generic;
using Code.Scripts.Tactical.Actor.Tiles;
using UnityEngine;

public class SelectAttackComponent : AttackComponent
{
    public string prefabName;
    public float activeTime;

    protected GameObject obj;
    
    public override void Init(SkillComponentInfo info)
    {
        base.Init(info);
        
        obj = SkillFactory.Create(prefabName);
    }

    public override void Execute(SkillComponentInfo info)
    {
        base.Execute(info);
        
        var currentTile = GridManager.Inst.GetTile(info.tile.Key + distance * info.dirX);

        if (currentTile)
        {
            obj.transform.position = currentTile.transform.position + Vector3.up * 0.5f;
        
            var targetUnit = currentTile?.content;
            var newInfo = new SkillComponentInfo(info, currentTile);
        
            if (targetUnit && targetUnit.TryGetComponent(out Health health))
            {
                OnHit?.Invoke(newInfo);
                
                if (info.user.unitType == UnitType.Player)
                    SoundManager.Inst.Play("Enemy_Hit");
                health.OnDamage(value);
            }

            var task = new Task(EndHandler(newInfo));
            task.Start();
        }
    }

    public override void Print(SkillComponentInfo info)
    {
        base.Print(info);
        
        GridManager.Inst.ApplyGrid(info.user, new List<Tile> { GridManager.Inst.GetTile(info.tile.Key + distance * info.dirX) });
    }
    
    public override void Cancel(SkillComponentInfo info)
    {
        base.Cancel(info);
        
        GridManager.Inst.RevertGrid(info.user);
    }

    private IEnumerator EndHandler(SkillComponentInfo info)
    {
        yield return new WaitForSeconds(activeTime);
        OnEnd?.Invoke(info);
    }
}
