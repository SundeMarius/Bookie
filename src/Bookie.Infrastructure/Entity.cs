namespace Bookie.Infrastructure;

public abstract class Entity(Guid id)
{
    public Guid Id { get; init; } = id;
}
