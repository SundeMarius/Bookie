using Bookie.Application;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOpenApi()
    .AddControllers();

// Register all application services
CompositionRoot.Compose(builder.Services);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();