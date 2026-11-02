using MediFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using MediFlow.Infrastructure.Controllers;     // NUEVO
using MediFlow.Infrastructure.Configuration;   // NUEVO

var builder = WebApplication.CreateBuilder(args);

// Registrar controladores (viven en MediFlow.Infrastructure)
builder.Services
    .AddControllers()
    .AddApplicationPart(typeof(UsersController).Assembly);   // NUEVO

// Configurar Entity Framework Core con PostgreSQL
builder.Services.AddDbContext<MediFlowContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Manejo de errores (404, 409, 400 desde los casos de uso)   // NUEVO
builder.Services.AddProblemDetails();                          // NUEVO
builder.Services.AddExceptionHandler<ApiExceptionHandler>();   // NUEVO

// Configurar Swagger / OpenAPI
builder.Services.AddOpenApi();

// Repositorios, casos de uso y servicios de cada módulo       // NUEVO
builder.Services.AddInfrastructureServices(builder.Configuration);   // NUEVO



// Registrar controladores
builder.Services.AddControllers();

// Configurar Entity Framework Core con PostgreSQL
builder.Services.AddDbContext<MediFlowContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Configurar Swagger / OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();   // NUEVO (antes del resto del pipeline)

// Configuración del entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();