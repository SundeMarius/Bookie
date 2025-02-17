using Bookie.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMediatR(c => c.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()))
    .AddOpenApi()
    .AddControllers();

// Register all application services
CompositionRoot.Compose(builder.Services);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();