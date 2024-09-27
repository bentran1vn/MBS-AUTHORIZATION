namespace MBS_AUTHORIZATION.Domain.Abstractions.Entities;

public abstract class Entity<T> : IEntity<T>
{
    public T Id { get; set; }

    public bool IsDeleted { get; protected set; }
}