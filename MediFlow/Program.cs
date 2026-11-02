using MediFlow.Infrastructure.Context;
using MediFlow.Application.UseCases.Appointments;
using MediFlow.Application.UseCases.Schedules;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registrar controladores
builder.Services.AddControllers();

// Configurar Entity Framework Core con PostgreSQL
builder.Services.AddDbContext<MediFlowContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);
builder.Services.AddScoped<AppointmentUseCase>();
builder.Services.AddScoped<ScheduleUseCase>();

// Configurar Swagger / OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Configuración del entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
