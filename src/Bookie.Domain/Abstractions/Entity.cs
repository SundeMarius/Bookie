namespace Bookie.Domain.Abstractions;

public abstract class Entity(Guid id)
{
    public Guid Id { get; init; } = id;

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        if (obj is not Entity objAsEntity)
            return false;
        else return Equals(objAsEntity);
    }
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    public bool Equals(Entity other)
    {
        if (other == null)
            return false;
        return Id.Equals(other.Id);
    }
}
