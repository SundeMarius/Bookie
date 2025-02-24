using System.Reflection;
using Bookie.Domain.Books;
using Bookie.Domain.Customers;
using Bookie.Domain.Library;
using Bookie.Infrastructure.Repositories.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Bookie.Application;

public static class CompositionRoot
{
    public static IServiceCollection Compose(IServiceCollection services)
    {
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
            .AddSingleton<IBookRepository, BookRepository>()
            .AddSingleton<ICustomerRepository, CustomerRepository>()
            .AddScoped<LibraryService>();
        return services;
    }
}
