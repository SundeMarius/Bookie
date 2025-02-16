using Bookie.Infrastructure;
using FluentMonads;

namespace Bookie.Domain.Books;


public sealed class ISBN10
{
    public static Result<ISBN10> Create(uint group, uint publisher, uint title)
    {
        if (group >= 10_000)
            return ISBN10Errors.InvalidGroupNumber(group);

        if ((group.ToString() + publisher.ToString() + title.ToString()).Length != 9)
            return ISBN10Errors.NotValidCombination;

        return Result<ISBN10>.Success(new ISBN10(group, publisher, title));
    }

    public override string ToString()
    {
        return $"{Group}-{Publisher}-{Title}-{CheckDigit}";
    }

    public uint Group { get; private set; }
    public uint Publisher { get; private set; }
    public uint Title { get; private set; }
    public uint CheckDigit { get; private set; }

    private ISBN10(uint group, uint publisher, uint title)
    {
        Group = group;
        Publisher = publisher;
        Title = title;
        CheckDigit = ComputeCheckDigit();
    }

    private uint ComputeCheckDigit()
    {
        var groupDigits = Group.ToString().Select(c => uint.Parse(c.ToString()));
        var publisherDigits = Publisher.ToString().Select(c => uint.Parse(c.ToString()));
        var titleDigits = Title.ToString().Select(c => uint.Parse(c.ToString()));

        var digits = groupDigits.Concat(publisherDigits).Concat(titleDigits).ToArray();
        uint s = 0;
        uint t = 0;
        for (int i = 0; i < 10; ++i)
        {
            t += digits[i];
            s += t;
        }

        return (11 - s % 11) % 11;
    }
}

public static class ISBN10Errors
{
    public static DomainError InvalidGroupNumber(uint group) => new("ISBN10.InvalidGroup", $"The group number {group} is not valid");
    public static DomainError NotValidCombination => new("ISBN10.Invalid", $"The current number combination has to contain nine digits");
}