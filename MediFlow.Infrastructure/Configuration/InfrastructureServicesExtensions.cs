using MediFlow.Domain.Repositories;
using MediFlow.Infrastructure.Adapters.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MediFlow.Infrastructure.Configuration;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services)
    {
        services.AddScoped(
            typeof(IGenericRepository<>),
            typeof(GenericRepository<>));

        return services;
    }
}