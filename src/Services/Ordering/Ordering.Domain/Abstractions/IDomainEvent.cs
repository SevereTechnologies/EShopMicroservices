using MediatR;

namespace Ordering.Domain.Abstractions;

public interface IDomainEvent : INotification // INotification allows domain events to be dispatch through MediatR handlers.
{
    Guid EventId => Guid.NewGuid();
    public DateTime OccurredOn => DateTime.Now;
    public string EventType => GetType().AssemblyQualifiedName;
}
