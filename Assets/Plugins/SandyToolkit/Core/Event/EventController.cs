using System;
using System.Collections.Generic;
using SandyToolkitCore;

public partial class EventController : BaseController, IEventService
{
    public override ControllerInfo ControllerInfo => new()
    {
        ContainSceneNames = new string[] { },
        Priority = 0,
        UpdateInterval = 0,
        LateUpdateInterval = 0,
        FixedUpdateInterval = 0,
        IsBackProcess = true
    };

    private readonly Dictionary<Type, List<object>> handlers = new Dictionary<Type, List<object>>();
    private readonly Dictionary<string, Action> triggerEvents = new Dictionary<string, Action>();
    private readonly Dictionary<string, Action<float>> floatEvents = new Dictionary<string, Action<float>>();
    private readonly Dictionary<string, Action<int>> intEvents = new Dictionary<string, Action<int>>();
    private readonly Dictionary<string, Action<float, float>> floatFloatEvents = new Dictionary<string, Action<float, float>>();
    private readonly Dictionary<string, Action<int, int>> intIntEvents = new Dictionary<string, Action<int, int>>();
    private readonly Dictionary<string, Action<int, float>> intFloatEvents = new Dictionary<string, Action<int, float>>();
    private readonly Dictionary<string, Action<float, int>> floatIntEvents = new Dictionary<string, Action<float, int>>();

    public void Clear()
    {
        handlers.Clear();
        triggerEvents.Clear();
        floatEvents.Clear();
        intEvents.Clear();
        floatFloatEvents.Clear();
        intIntEvents.Clear();
        intFloatEvents.Clear();
        floatIntEvents.Clear();
    }

    public void SubscribeTrigger(string triggerName, Action action)
    {   
        _logger.Info($"Subscribe Trigger: {triggerName}");

        if (!triggerEvents.ContainsKey(triggerName))
        {
            triggerEvents[triggerName] = action;
        }
        else
        {
            triggerEvents[triggerName] += action;
        }
    }

    public void UnsubscribeTrigger(string triggerName, Action action)
    {
        if (triggerEvents.ContainsKey(triggerName))
        {
            triggerEvents[triggerName] -= action;
            if (triggerEvents[triggerName] == null)
            {
                triggerEvents.Remove(triggerName);
            }
        }
    }

    public void PublishTrigger(string triggerName)
    {
        _logger.Debug($"PublishTrigger: {triggerName}");
        if (triggerEvents.ContainsKey(triggerName))
        {
            triggerEvents[triggerName]?.Invoke();
        }
    }

    public void Subscribe<T>(IEventHandler<T> handler) where T : struct, IEvent
    {
        _logger.Info($"Subscribe Handler: {typeof(T).Name}");

        Type eventType = typeof(T);
        if (!handlers.ContainsKey(eventType))
        {
            handlers[eventType] = new List<object>();
        }
        handlers[eventType].Add(handler);
    }

    public void Unsubscribe<T>(IEventHandler<T> handler) where T : struct, IEvent
    {
        Type eventType = typeof(T);
        if (handlers.ContainsKey(eventType))
        {
            handlers[eventType].Remove(handler);
        }
    }

    public void Publish<T>(T @event) where T : struct, IEvent
    {
        _logger.Info($"Publish Handler: {@event.EventName}");

        Type eventType = typeof(T);
        if (handlers.ContainsKey(eventType))
        {
            foreach (var handler in handlers[eventType])
            {
                ((IEventHandler<T>)handler).Handle(@event);
            }

            _logger.Debug($"Publish Handler Success for {handlers[eventType].Count}");
        }
        else
        {
            _logger.Error($"Publish Handler: {@event.EventName} not found");
        }
    }

    public void PublishFloat(string eventName, float value)
    {
        _logger.Info($"Publish Float: {eventName}");

        if (floatEvents.ContainsKey(eventName))
        {
            floatEvents[eventName]?.Invoke(value);
        }
    }

    public void SubscribeFloat(string eventName, Action<float> action)
    {
        _logger.Info($"Subscribe Float: {eventName}");

        if (!floatEvents.ContainsKey(eventName))
        {
            floatEvents[eventName] = action;
        }
        else
        {
            floatEvents[eventName] += action;
        }
    }

    public void UnsubscribeFloat(string eventName, Action<float> action)
    {
        if (floatEvents.ContainsKey(eventName))
        {
            floatEvents[eventName] -= action;
            if (floatEvents[eventName] == null)
            {
                floatEvents.Remove(eventName);
            }
        }
    }

    public void PublishInt(string eventName, int value)
    {
        _logger.Info($"Publish Int: {eventName}");
        
        if (intEvents.ContainsKey(eventName))
        {
            intEvents[eventName]?.Invoke(value);
        }
    }

    public void SubscribeInt(string eventName, Action<int> action)
    {
        _logger.Info($"Subscribe Int: {eventName}");

        if (!intEvents.ContainsKey(eventName))
        {
            intEvents[eventName] = action;
        }
        else
        {
            intEvents[eventName] += action;
        }
    }

    public void UnsubscribeInt(string eventName, Action<int> action)
    {
        if (intEvents.ContainsKey(eventName))
        {
            intEvents[eventName] -= action;
            if (intEvents[eventName] == null)
            {
                intEvents.Remove(eventName);
            }
        }
    }

    public void PublishFloatFloat(string eventName, float value1, float value2)
    {
        _logger.Info($"Publish FloatFloat: {eventName}");

        if (floatFloatEvents.ContainsKey(eventName))
        {
            floatFloatEvents[eventName]?.Invoke(value1, value2);
        }
    }

    public void SubscribeFloatFloat(string eventName, Action<float, float> action)
    {
        _logger.Info($"Subscribe FloatFloat: {eventName}");
        
        if (!floatFloatEvents.ContainsKey(eventName))
        {
            floatFloatEvents[eventName] = action;
        }
        else
        {
            floatFloatEvents[eventName] += action;
        }
    }

    public void UnsubscribeFloatFloat(string eventName, Action<float, float> action)
    {
        if (floatFloatEvents.ContainsKey(eventName))
        {
            floatFloatEvents[eventName] -= action;
        }
    }

    public void PublishIntInt(string eventName, int value1, int value2)
    {
        _logger.Info($"Publish IntInt: {eventName}");
        
        if (intIntEvents.ContainsKey(eventName))
        {
            intIntEvents[eventName]?.Invoke(value1, value2);
        }
    }

    public void SubscribeIntInt(string eventName, Action<int, int> action)
    {
        _logger.Info($"Subscribe IntInt: {eventName}");
        
        if (!intIntEvents.ContainsKey(eventName))
        {
            intIntEvents[eventName] = action;
        }
        else
        {
            intIntEvents[eventName] += action;
        }
    }

    public void UnsubscribeIntInt(string eventName, Action<int, int> action)
    {
        if (intIntEvents.ContainsKey(eventName))
        {
            intIntEvents[eventName] -= action;
        }
    }

    public void PublishIntFloat(string eventName, int value1, float value2)
    {
        _logger.Info($"Publish IntFloat: {eventName}");
        
        if (intFloatEvents.ContainsKey(eventName))
        {
            intFloatEvents[eventName]?.Invoke(value1, value2);
        }
    }

    public void SubscribeIntFloat(string eventName, Action<int, float> action)
    {
        _logger.Info($"Subscribe IntFloat: {eventName}");
        
        if (!intFloatEvents.ContainsKey(eventName))
        {
            intFloatEvents[eventName] = action;
        }
        else
        {
            intFloatEvents[eventName] += action;
        }
    }

    public void UnsubscribeIntFloat(string eventName, Action<int, float> action)
    {
        if (intFloatEvents.ContainsKey(eventName))
        {
            intFloatEvents[eventName] -= action;
        }
    }

    public void PublishFloatInt(string eventName, float value1, int value2)
    {
        _logger.Info($"Publish FloatInt: {eventName}");
        
        if (floatIntEvents.ContainsKey(eventName))
        {
            floatIntEvents[eventName]?.Invoke(value1, value2);
        }
    }

    public void SubscribeFloatInt(string eventName, Action<float, int> action)
    {
        _logger.Info($"Subscribe FloatInt: {eventName}");
        
        if (!floatIntEvents.ContainsKey(eventName))
        {
            floatIntEvents[eventName] = action;
        }
        else
        {
            floatIntEvents[eventName] += action;
        }
    }

    public void UnsubscribeFloatInt(string eventName, Action<float, int> action)
    {
        if (floatIntEvents.ContainsKey(eventName))
        {
            floatIntEvents[eventName] -= action;
        }
    }
}
