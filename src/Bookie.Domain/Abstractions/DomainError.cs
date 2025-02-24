using FluentMonads;

namespace Bookie.Domain.Abstractions;

public record DomainError(string Code, string Message) : Error(Message);
