namespace MediFlow.Domain.Port.Repositories;

public interface IUnitOfWork
{
    IAppointmentRepository Appointments { get; }
    IDiagnosisRepository Diagnoses { get; }
    IDoctorRepository Doctors { get; }
    IDoctorRoomRepository DoctorRooms { get; }
    IInvoiceRepository Invoices { get; }
    IMedicalRecordRepository MedicalRecords { get; }
    IMedicationRepository Medications { get; }
    INotificationRepository Notifications { get; }
    IPatientRepository Patients { get; }
    IPaymentRepository Payments { get; }
    IPrescriptionRepository Prescriptions { get; }
    IPrescriptionDetailRepository PrescriptionDetails { get; }
    IRoleRepository Roles { get; }
    IRoomRepository Rooms { get; }
    IScheduleRepository Schedules { get; }
    ISpecialtyRepository Specialties { get; }
    IUserRepository Users { get; }
    IUserRoleRepository UserRoles { get; }

    Task<int> SaveChangesAsync();
}