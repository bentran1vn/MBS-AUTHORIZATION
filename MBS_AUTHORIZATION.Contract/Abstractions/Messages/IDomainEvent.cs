using MassTransit;
namespace MBS_AUTHORIZATION.Contract.Abstractions.Messages;

[ExcludeFromTopology]
public interface IDomainEvent
{
    public Guid IdEvent { get; init; }
}