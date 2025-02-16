using FluentMonads;

namespace Bookie.Infrastructure;

public record DomainError(string Code, string Message) : Error(Code, Message);
