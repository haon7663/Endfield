using System;

public partial interface IEventService
{
    void Subscribe<T>(IEventHandler<T> handler) where T : struct, IEvent;
    void Unsubscribe<T>(IEventHandler<T> handler) where T : struct, IEvent;
    void Publish<T>(T @event) where T : struct, IEvent;
    void Clear();
    void SubscribeTrigger(string triggerName, Action action);
    void UnsubscribeTrigger(string triggerName, Action action);
    void PublishTrigger(string triggerName);
    void SubscribeFloat(string eventName, Action<float> action);
    void UnsubscribeFloat(string eventName, Action<float> action);
    void PublishFloat(string eventName, float value);
    void SubscribeInt(string eventName, Action<int> action);
    void UnsubscribeInt(string eventName, Action<int> action);
    void PublishInt(string eventName, int value);
    void SubscribeFloatFloat(string eventName, Action<float, float> action);
    void UnsubscribeFloatFloat(string eventName, Action<float, float> action);
    void PublishFloatFloat(string eventName, float value1, float value2);
    void SubscribeIntInt(string eventName, Action<int, int> action);
    void UnsubscribeIntInt(string eventName, Action<int, int> action);
    void PublishIntInt(string eventName, int value1, int value2);
    void SubscribeIntFloat(string eventName, Action<int, float> action);
    void UnsubscribeIntFloat(string eventName, Action<int, float> action);
    void PublishIntFloat(string eventName, int value1, float value2);
    void SubscribeFloatInt(string eventName, Action<float, int> action);
    void UnsubscribeFloatInt(string eventName, Action<float, int> action);
    void PublishFloatInt(string eventName, float value1, int value2);
}