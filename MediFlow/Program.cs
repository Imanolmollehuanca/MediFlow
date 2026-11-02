using MediFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using MediFlow.Application;
using MediFlow.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Registrar controladores
builder.Services.AddControllers();
// Registrar repositorios de Infrastructure
builder.Services.AddInfrastructureServices();

// Registrar Commands y Queries de MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(
        typeof(ApplicationAssemblyMarker).Assembly));

// Configurar Entity Framework Core con PostgreSQL
builder.Services.AddDbContext<MediFlowContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Configurar Swagger / OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Configuración del entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "MediFlow API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();