using MediFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MediFlow.Infrastructure.Configuration;

public static class InfrastructureServicesExtencion
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MediFlowContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddUsersRolesModule();   // angeloide  xd
        // services.AddDoctorsModule();    // integrante 2
        // services.AddSchedulesModule();  // integrante 3
        // services.AddPatientsModule();   // integrante 1

        return services;
    }
}