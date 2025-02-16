using Bookie.Infrastructure;
using EmailValidation;
using FluentMonads;

namespace Bookie.Domain.Customers;

public sealed class Customer : Entity
{
    public static Result<Customer> Create(string firstName, string? lastName, string? email)
    {
        if (string.IsNullOrEmpty(firstName))
            return CustomerErrors.EmptyFirstName;
        if (!string.IsNullOrEmpty(email) && !EmailValidator.Validate(email))
            return CustomerErrors.InvalidEmail(email);
        return new Customer(firstName, lastName, email);
    }

    public void AddRental(Rental rental) => _rentals.Add(rental);
    public void RemoveRental(Guid bookId) => _rentals.RemoveAll(b => b.BookId == bookId);

    public string FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? Email { get; private set; }
    public IReadOnlyList<Rental> Rentals => _rentals.AsReadOnly();

    private Customer(string firstName, string? lastName, string? email) : base(Guid.NewGuid())
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        _rentals = [];
    }
    private List<Rental> _rentals;
}

public static class CustomerErrors
{
    public static Infrastructure.BookieError EmptyFirstName => new("Customer.EmptyFirstName", "The first name has to be set");
    public static Infrastructure.BookieError InvalidEmail(string email) => new("Customer.InvalidEmail", $"The email '{email}' has an invalid format");
}