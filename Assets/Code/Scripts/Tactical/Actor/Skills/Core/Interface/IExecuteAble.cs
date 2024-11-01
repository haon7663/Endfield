using System;

public interface IExecuteAble
{
    Action OnHit { get; set; }
    Action OnEnd { get; set; }
}