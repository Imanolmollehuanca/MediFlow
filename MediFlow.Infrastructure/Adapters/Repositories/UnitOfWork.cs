
using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;
using MediFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace MediFlow.Infrastructure.Adapters.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MediFlowContext _context;

    public UnitOfWork(MediFlowContext context)
    {
        _context = context;

        Appointments = new AppointmentRepository(context);
        Diagnoses = new DiagnosisRepository(context);
        Doctors = new DoctorRepository(context);
        DoctorRooms = new DoctorRoomRepository(context);
        Invoices = new InvoiceRepository(context);
        MedicalRecords = new MedicalRecordRepository(context);
        Medications = new MedicationRepository(context);
        Notifications = new NotificationRepository(context);
        Patients = new PatientRepository(context);
        Payments = new PaymentRepository(context);
        Prescriptions = new PrescriptionRepository(context);
        PrescriptionDetails = new PrescriptionDetailRepository(context);
        Roles = new RoleRepository(context);
        Rooms = new RoomRepository(context);
        Schedules = new ScheduleRepository(context);
        Specialties = new SpecialtyRepository(context);
        Users = new UserRepository(context);
        UserRoles = new UserRoleRepository(context);
    }

    public IAppointmentRepository Appointments { get; }
    public IDiagnosisRepository Diagnoses { get; }
    public IDoctorRepository Doctors { get; }
    public IDoctorRoomRepository DoctorRooms { get; }
    public IInvoiceRepository Invoices { get; }
    public IMedicalRecordRepository MedicalRecords { get; }
    public IMedicationRepository Medications { get; }
    public INotificationRepository Notifications { get; }
    public IPatientRepository Patients { get; }
    public IPaymentRepository Payments { get; }
    public IPrescriptionRepository Prescriptions { get; }
    public IPrescriptionDetailRepository PrescriptionDetails { get; }
    public IRoleRepository Roles { get; }
    public IRoomRepository Rooms { get; }
    public IScheduleRepository Schedules { get; }
    public ISpecialtyRepository Specialties { get; }
    public IUserRepository Users { get; }
    public IUserRoleRepository UserRoles { get; }

    public async Task<int> SaveChangesAsync()
    {
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException
            { SqlState: PostgresErrorCodes.UniqueViolation, ConstraintName: MediFlowContext.ActiveDoctorRoomIndex })
        {
            throw new InvalidOperationException("El médico ya tiene una asignación activa en esta sala.", ex);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException
            { SqlState: PostgresErrorCodes.ExclusionViolation, ConstraintName: MediFlowContext.ScheduleOverlapConstraint })
        {
            throw new InvalidOperationException("El médico ya tiene un horario que se solapa en ese día.", ex);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException
            { SqlState: PostgresErrorCodes.CheckViolation, ConstraintName: var constraintName } &&
            constraintName is MediFlowContext.ScheduleDayConstraint or
                MediFlowContext.ScheduleTimeConstraint or MediFlowContext.ScheduleDurationConstraint)
        {
            throw new ArgumentException("El día, el intervalo o la duración de cita del horario no son válidos.", ex);
        }
        catch (InvalidOperationException ex) when (ex.InnerException is DbUpdateException
            { InnerException: PostgresException { SqlState: PostgresErrorCodes.DeadlockDetected } } updateException &&
            updateException.Entries.Count > 0 && updateException.Entries.All(entry => entry.Entity is Schedule))
        {
            throw new InvalidOperationException(
                "El horario no pudo guardarse por una modificación simultánea. Vuelve a intentarlo.", ex);
        }
    }
}
