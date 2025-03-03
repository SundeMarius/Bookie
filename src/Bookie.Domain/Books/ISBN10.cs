using Bookie.Domain.Abstractions;
using FluentMonads;

namespace Bookie.Domain.Books;

public sealed record ISBN10
{
    public static Result<ISBN10> Create(int group, int publisher, int title)
    {
        if (group >= 10_000)
            return ISBN10Error.InvalidGroupNumber(group);

        if ($"{group}{publisher}{title}".Length != 9)
            return ISBN10Error.NotValidCombination;

        return new ISBN10(group, publisher, title);
    }

    public override string ToString()
    {
        return $"{Group}-{Publisher}-{Title}-{CheckDigit}";
    }

    public int Group { get; private set; }
    public int Publisher { get; private set; }
    public int Title { get; private set; }
    public int CheckDigit { get; private set; }

    private ISBN10(int group, int publisher, int title)
    {
        Group = group;
        Publisher = publisher;
        Title = title;
        CheckDigit = ComputeCheckDigit();
    }

    private int ComputeCheckDigit()
    {
        var groupDigits = Group.ToString().Select(c => int.Parse(c.ToString()));
        var publisherDigits = Publisher.ToString().Select(c => int.Parse(c.ToString()));
        var titleDigits = Title.ToString().Select(c => int.Parse(c.ToString()));

        var digits = groupDigits.Concat(publisherDigits).Concat(titleDigits).ToArray();
        var s = 0;
        var t = 0;
        for (int i = 0; i < 9; ++i)
        {
            t += digits[i];
            s += t;
        }

        return (11 - s % 11) % 11;
    }
}

public static class ISBN10Error
{
    public static DomainError InvalidGroupNumber(int group) => new("ISBN10.InvalidGroup", $"The group number {group} is not valid");
    public static DomainError NotValidCombination => new("ISBN10.InvalidCombination", $"There has to be exactly nine digits from the 'group, publisher, title' combination");
}