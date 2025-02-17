using Bookie.Domain.Abstractions;
using Bookie.Domain.Authorization;
using EmailValidation;
using FluentMonads;

namespace Bookie.Domain.Customers;

public sealed class Customer : Entity
{
    public static Result<Customer> Create(string firstName, string? lastName, string? email, AuthorizationLevel level = AuthorizationLevel.Low)
    {
        if (string.IsNullOrEmpty(firstName))
            return CustomerErrors.EmptyFirstName;
        if (!string.IsNullOrEmpty(email) && !EmailValidator.Validate(email))
            return CustomerErrors.InvalidEmail(email);
        return new Customer(firstName, lastName, email, level);
    }

    public void AddRental(Rental rental) => _rentals.Add(rental);
    public void RemoveRental(Guid bookId) => _rentals.RemoveAll(b => b.BookId == bookId);

    public string FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? Email { get; private set; }
    public AuthorizationLevel Authorization { get; private set; }
    public IReadOnlyList<Rental> Rentals => _rentals.AsReadOnly();

    private Customer(string firstName, string? lastName, string? email, AuthorizationLevel level)
        : base(Guid.NewGuid())
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Authorization = level;
        _rentals = [];
    }
    private List<Rental> _rentals;
}

public static class CustomerErrors
{
    public static DomainError EmptyFirstName => new("CustomerError.EmptyFirstName", "The first name has to be set");
    public static DomainError InvalidEmail(string email) => new("CustomerError.InvalidEmail", $"The email '{email}' has an invalid format");
}