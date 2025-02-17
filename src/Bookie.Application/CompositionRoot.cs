using Bookie.Domain.Books;
using Bookie.Domain.Customers;
using Bookie.Domain.Library;
using Bookie.Infrastructure.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Bookie.Application;

public static class CompositionRoot
{
    public static IServiceCollection Compose(IServiceCollection services)
    {
        services
            .AddSingleton<IBookRepository, BookRepository>()
            .AddSingleton<ICustomerRepository, CustomerRepository>()
            .AddScoped<LibraryService>();
        return services;
    }
}
