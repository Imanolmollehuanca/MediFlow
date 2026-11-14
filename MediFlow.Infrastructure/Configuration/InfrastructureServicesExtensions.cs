using MediFlow.Domain.Port.Repositories;
using MediFlow.Infrastructure.Adapters.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MediFlow.Application.Port.Services;
using MediFlow.Infrastructure.Adapters.Services.Implementation;

namespace MediFlow.Infrastructure.Configuration;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services)
    {
        services.AddScoped(
            typeof(IGenericRepository<>),
            typeof(GenericRepository<>));

        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IDiagnosisRepository, DiagnosisRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IDoctorRoomRepository, DoctorRoomRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
        services.AddScoped<IMedicationRepository, MedicationRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
        services.AddScoped<IPrescriptionDetailRepository, PrescriptionDetailRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<IExcelReportService, ExcelReportService>();

        services.AddHttpContextAccessor();


        return services;
    }
}