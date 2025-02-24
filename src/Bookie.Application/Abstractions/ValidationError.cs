using System.Text.Json.Serialization;
using FluentMonads;

namespace Bookie.Application.Abstractions;

public record class ValidationError([property: JsonIgnore] string Error) : Error("Validation error", Error);
