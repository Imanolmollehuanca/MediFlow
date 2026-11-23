using System.Data;
using MediFlow.Domain.Entities;
using MediFlow.Domain.Port.Repositories;
using MediFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
            var schedules = _context.ChangeTracker.Entries<Schedule>()
                .Where(entry => (entry.State is EntityState.Added or EntityState.Modified) && entry.Entity.IsActive)
                .Select(entry => entry.Entity)
                .ToArray();
            var assignments = _context.ChangeTracker.Entries<DoctorRoom>()
                .Where(entry => (entry.State is EntityState.Added or EntityState.Modified) && entry.Entity.IsActive)
                .Select(entry => entry.Entity)
                .ToArray();

            return schedules.Length == 0 && assignments.Length == 0
                ? await _context.SaveChangesAsync()
                : await SaveResourceChangesAsync(schedules, assignments);
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
            updateException.Entries.Count > 0 && updateException.Entries.All(entry => entry.Entity is Schedule or DoctorRoom))
        {
            throw new InvalidOperationException(
                "El horario o la asignación no pudo guardarse por una modificación simultánea. Vuelve a intentarlo.", ex);
        }
    }

    private async Task<int> SaveResourceChangesAsync(Schedule[] schedules, DoctorRoom[] assignments)
    {
        var existingTransaction = _context.Database.CurrentTransaction;
        if (existingTransaction is not null &&
            existingTransaction.GetDbTransaction().IsolationLevel != IsolationLevel.ReadCommitted)
        {
            throw new NotSupportedException("La validación de salas requiere una transacción ReadCommitted.");
        }

        await using var transaction = existingTransaction is null
            ? await _context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted)
            : null;

        try
        {
            // Horarios y asignaciones comparten este bloqueo hasta terminar la transacción.
            // ReadCommitted permite comprobar los cambios confirmados mientras se esperaba.
            await _context.Database.ExecuteSqlRawAsync("SELECT pg_advisory_xact_lock(1296452687, 1)");

            var result = await _context.SaveChangesAsync(acceptAllChangesOnSuccess: false);

            foreach (var schedule in schedules)
            {
                if (await Schedules.HasRoomConflictAsync(schedule.DoctorId, schedule.DayOfWeek,
                        schedule.StartTime, schedule.EndTime, schedule.ScheduleId))
                {
                    throw new InvalidOperationException("Una sala asignada al médico ya está ocupada por otro médico en ese horario.");
                }
            }

            foreach (var assignment in assignments)
            {
                if (await DoctorRooms.HasScheduleConflictAsync(
                        assignment.DoctorId, assignment.RoomId, assignment.DoctorRoomId))
                {
                    throw new InvalidOperationException("La sala ya está ocupada por otro médico en un horario del médico asignado.");
                }
            }

            if (transaction is not null)
                await transaction.CommitAsync();

            _context.ChangeTracker.AcceptAllChanges();
            return result;
        }
        catch
        {
            if (existingTransaction is not null)
                await existingTransaction.RollbackAsync();

            throw;
        }
    }
}
