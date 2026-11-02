using MediFlow.Application.Interfaces;
using MediFlow.Application.UseCases.Roles;
using MediFlow.Application.UseCases.Users;
using MediFlow.Domain.Interfaces;
using MediFlow.Infrastructure.Adapters.Repositories;
using MediFlow.Infrastructure.Adapters.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MediFlow.Infrastructure.Configuration;

public static class UsersRolesModule
{
    public static IServiceCollection AddUsersRolesModule(this IServiceCollection services)
    {
        // Adaptadores
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        // Casos de uso: usuarios
        services.AddScoped<CreateUserUseCase>();
        services.AddScoped<GetUserByIdUseCase>();
        services.AddScoped<GetAllUsersUseCase>();
        services.AddScoped<UpdateUserUseCase>();
        services.AddScoped<DeleteUserUseCase>();
        services.AddScoped<AssignRoleToUserUseCase>();
        services.AddScoped<RemoveRoleFromUserUseCase>();

        // Casos de uso: roles
        services.AddScoped<CreateRoleUseCase>();
        services.AddScoped<GetRoleByIdUseCase>();
        services.AddScoped<GetAllRolesUseCase>();
        services.AddScoped<UpdateRoleUseCase>();
        services.AddScoped<DeleteRoleUseCase>();

        return services;
    }
}