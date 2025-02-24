using Bookie.Domain.Abstractions;
using Bookie.Domain.Authorization;
using EmailValidation;
using FluentMonads;

namespace Bookie.Domain.Customers;

public sealed class Customer : Entity
{
    public static Result<Customer> Create(string firstName, string lastName, string email, AuthorizationLevel level = AuthorizationLevel.Low)
    {
        return
            Validate(firstName, lastName, email)
            .Map(_ => new Customer(firstName, lastName, email, level));
    }
    public Result Update(string? firstName = null, string? lastName = null, string? email = null, AuthorizationLevel? level = null)
    {
        var fname = firstName ?? FirstName;
        var lname = lastName ?? LastName;
        var em = email ?? Email;
        var auth = level ?? Authorization;
        return
            Validate(fname, lname, em)
            .OnSuccess(_ =>
            {
                FirstName = fname;
                LastName = lname;
                Email = em;
                Authorization = auth;
                LastUpdated = DateTimeOffset.UtcNow;
            });
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public AuthorizationLevel Authorization { get; private set; }
    public IReadOnlyList<Loan> Loans => _loans.AsReadOnly();

    public void AddLoan(Loan rental) => _loans.Add(rental);
    public void RemoveLoan(Loan rental) => _loans.Remove(rental);

    private static Result Validate(string firstName, string lastName, string email)
    {
        if (string.IsNullOrEmpty(firstName))
            return Result.Failure(CustomerError.MissingFirstName);

        if (string.IsNullOrEmpty(lastName))
            return Result.Failure(CustomerError.MissingLastName);

        if (string.IsNullOrEmpty(email))
            return Result.Failure(CustomerError.MissingEmail);

        if (!EmailValidator.Validate(email))
            return Result.Failure(CustomerError.InvalidEmail(email));

        return Result.Success();
    }

    private Customer(string firstName, string lastName, string email, AuthorizationLevel level)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Authorization = level;
        _loans = [];
    }
    private readonly List<Loan> _loans;
}

public static class CustomerError
{
    public static DomainError MissingFirstName => new("CustomerError.MissingFirstName", "The first name has to be set");
    public static DomainError MissingLastName => new("CustomerError.MissingLastName", "The last name has to be set");
    public static DomainError MissingEmail => new("CustomerError.MissingEmail", "The email has to be set");
    public static DomainError InvalidEmail(string email) => new("CustomerError.InvalidEmailFormat", $"The email '{email}' has an invalid format");
}