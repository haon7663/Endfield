public interface IEventHandler<T> where T : struct, IEvent
{
    void Handle(T @event);
}