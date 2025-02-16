using FluentMonads;

namespace Bookie.Infrastructure;

public record BookieError(string Code, string Message) : Error(Code, Message);
