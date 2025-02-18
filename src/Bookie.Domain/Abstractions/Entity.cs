namespace Bookie.Domain.Abstractions;

public abstract class Entity(Guid id)
{
    public Guid Id { get; init; } = id;

    public override bool Equals(object? obj)
    {
        if (obj is Entity e)
            return Id == e.Id;
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
